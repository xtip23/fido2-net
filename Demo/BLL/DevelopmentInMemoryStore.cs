//using Fido2NetLib.Development;
using Fido2NetLib.Objects;
using Fido2NetLib;
using Newtonsoft.Json;
using System.Text;

namespace WAFido2.BLL
{
    public class DevelopmentInMemoryStore
    {

        public Fido2User GetOrAddUser(string username, Func<Fido2User> addCallback)
        {
            bool strRs = Redis.checkUser(username);
            Fido2User user = new Fido2User();
            if (strRs)
            {
                user.DisplayName = username;
                user.Name = username;
                user.Id = Encoding.UTF8.GetBytes(username); // byte representation of userID is required
            }
            else
            {
                user = addCallback();
                Redis.setData(user.Name, "");
            }
            return user;
        }

        public Fido2User GetUser(string username)
        {
            //_storedUsers.TryGetValue(username, out var user);
            //return user;

            bool strRs = Redis.checkUser(username);
            Fido2User user = new Fido2User();
            if (strRs)
            {
                user.DisplayName = username;
                user.Name = username;
                user.Id = Encoding.UTF8.GetBytes(username); // byte representation of userID is required
            }
            else
            {
                user = null;
            }
            return user;
        }

        public List<StoredCredential> GetCredentialsByUser(string user)
        {
            string strRs = Redis.getData(user);

            List<StoredCredentialJson> lstJson = JsonConvert.DeserializeObject<List<StoredCredentialJson>>(strRs ?? "") ?? new List<StoredCredentialJson>();

            List<StoredCredential> storedCredentials = new List<StoredCredential>();

            for (int i = 0; i < lstJson.Count; i++)
            {
                StoredCredential sc = new StoredCredential();

                sc.Id = Convert.FromBase64String(lstJson[i].Id);
                sc.Descriptor = new PublicKeyCredentialDescriptor(Convert.FromBase64String(lstJson[i].Id));
                sc.PublicKey = Convert.FromBase64String(lstJson[i].PublicKey);
                sc.UserHandle = Convert.FromBase64String(lstJson[i].UserHandle);
                sc.SignCount = lstJson[i].SignCount;
                sc.AttestationFormat = lstJson[i].AttestationFormat;
                sc.RegDate = DateTimeOffset.UtcNow;
                sc.AaGuid = lstJson[i].AaGuid;
                sc.Transports = lstJson[i].Transports;
                sc.IsBackupEligible = lstJson[i].IsBackupEligible;
                sc.IsBackedUp = lstJson[i].IsBackedUp;
                sc.AttestationObject = Convert.FromBase64String(lstJson[i].AttestationObject);
                sc.AttestationClientDataJson = Convert.FromBase64String(lstJson[i].AttestationClientDataJson);
                sc.UserId = Convert.FromBase64String(lstJson[i].UserId);
                sc.Type = lstJson[i].Type;
                sc.DevicePublicKeys = GetDevicePublicKeysFromJson(lstJson[i].DevicePublicKeys);
                storedCredentials.Add(sc);
            }

            ///return _storedCredentials.Where(c => c.UserId.AsSpan().SequenceEqual(user.Id)).ToList();

            return storedCredentials;
        }

        public StoredCredential GetCredentialById(byte[] id, string user)
        {
            List<StoredCredential> storedCredentials = new List<StoredCredential>();
            storedCredentials = GetCredentialsByUser(user);
            for (int i = 0; i < storedCredentials.Count; i++)
            {
                if (storedCredentials[i].Id.AsSpan().SequenceEqual(id))
                {
                    return storedCredentials[i];
                }
            }
            return null;
            //return _storedCredentials.FirstOrDefault(c => c.Descriptor.Id.AsSpan().SequenceEqual(id));
        }

        public Task<List<StoredCredential>> GetCredentialsByUserHandleAsync(byte[] userHandle, CancellationToken cancellationToken = default, string user = "")
        {
            return Task.FromResult(GetCredentialsByUser(user));
        }

        public void UpdateCounter(byte[] credentialId, uint counter, string user)
        {
            //var cred = _storedCredentials.First(c => c.Descriptor.Id.AsSpan().SequenceEqual(credentialId));
            //cred.SignCount = counter;

            List<StoredCredential> storedCredentials = new List<StoredCredential>();
            storedCredentials = GetCredentialsByUser(user);
            for (int i = 0; i < storedCredentials.Count; i++)
            {
                if (storedCredentials[i].Id.AsSpan().SequenceEqual(credentialId))
                {
                    storedCredentials[i].SignCount = counter;
                }
            }
            string json = JsonConvert.SerializeObject(storedCredentials);
            Redis.setData(user, json);
        }

        public void AddCredentialToUser(Fido2User user, StoredCredential credential)
        {
            //credential.UserId = user.Id;

            // lưu redis
            List<StoredCredential> storedCredentials = new List<StoredCredential>();
            storedCredentials.Add(credential);
            string json = JsonConvert.SerializeObject(storedCredentials);
            Log(user.Name, "Register:::  " + json);

            Redis.setData(user.Name, json);
            // lưu ram
            ///_storedCredentials.Add(credential); 
        }

        //public Task<List<Fido2User>> GetUsersByCredentialIdAsync(byte[] credentialId, CancellationToken cancellationToken = default)
        public Task<List<Fido2User>> GetUsersByCredentialIdAsync(IsCredentialIdUniqueToUserParams userParams, CancellationToken cancellationToken = default)
        {
            // our in-mem storage does not allow storing multiple users for a given credentialId. Yours shouldn't either.
            //var cred = _storedCredentials.FirstOrDefault(c => c.Descriptor.Id.AsSpan().SequenceEqual(credentialId));
            var cred = GetCredentialsByUser(userParams.User.Name);

            if (cred.Count == 0)
                return Task.FromResult(new List<Fido2User>());
            else
            {
                List<Fido2User> users = new List<Fido2User>();
                for (int i = 0; i < cred.Count; i++)
                {
                    if (cred[i].Id.AsSpan().SequenceEqual(userParams.CredentialId))
                    {
                        Fido2User fido2User = new Fido2User();
                        fido2User.Id = Encoding.UTF8.GetBytes(userParams.User.Name);
                        fido2User.Name = userParams.User.Name;
                        fido2User.DisplayName = userParams.User.Name;

                        users.Add(fido2User);
                    }
                }
                return Task.FromResult(users);
            }

        }

        /// <summary>
        /// ghi log
        /// </summary>
        /// <param name="Stk"></param>
        /// <param name="data"></param>
        public void Log(string Stk, string data)
        {
            string FileName = $@"{DateTime.Now.ToString("yyyyMMdd")}__{Stk}";
            string fullFilePath = $"D:\\WebLog\\Fido2\\{DateTime.Now.Year}\\{DateTime.Now.Month.ToString("00")}\\{DateTime.Now.Day.ToString("00")}\\{FileName}";
            //ko duoc nhieu thread cung write vao 1 file => error
            bool Log = WriteFileStatic(fullFilePath, data);
        }

        /// <summary>
        /// tạo file
        /// </summary> 
        /// <param name="fullFilePath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool WriteFileStatic(string fullFilePath, string message)
        {
            try
            {
                // kiem tra folder trong full path, neu ko co thi tao folder
                // System.IO.DirectoryNotFoundException: 'Could not find a part of the path 'M:\WebLog\S6G\CommonLib.Tests'.'
                FileInfo fileInfo = new FileInfo(fullFilePath);
                if (fileInfo.Directory != null)
                {
                    if (!fileInfo.Directory.Exists)
                        fileInfo.Directory.Create();
                }

                using (FileStream stream = new FileStream(fullFilePath, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, true))
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine(message + "\r\n");
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<byte[]> GetDevicePublicKeysFromJson(object[] obj)
        {
            List<byte[]> bytes = new List<byte[]>();
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] != null)
                {
                    bytes.Add(Convert.FromBase64String(obj[i].ToString()));
                }
                else
                {
                    bytes.Add(null);
                }
            }

            return bytes;
        }

    }
}

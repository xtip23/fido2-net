using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Fido2NetLib.Development;
using Fido2NetLib.Objects;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Fido2NetLib
{
    internal class Redis
    {

        // Lấy dữ liệu từ redis ra
        public static string getData(string strServer, int iPort, int iDB, string strKey)
        {
            try
            {
                // Địa chỉ IP và cổng của máy chủ Redis
                string redisServer = strServer; // 10.26.7.84 - test
                int redisPort = iPort;       // 6379 - test

                // Tạo đối tượng Configuration cho Redis
                ConfigurationOptions config = new ConfigurationOptions
                {
                    EndPoints = { $"{redisServer}:{redisPort}" },
                    AbortOnConnectFail = false, // Tùy chọn tùy thuộc vào yêu cầu của bạn
                };

                // Tạo đối tượng ConnectionMultiplexer để kết nối đến Redis
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);

                // Lấy đối tượng Database để thực hiện các thao tác trên Redis
                IDatabase db = redis.GetDatabase(iDB); // 14 - test

                // Ví dụ: Thực hiện một số thao tác trên Redis
                string key = strKey; // FIDO:058C123456

                // Đọc giá trị từ Redis
                string retrievedValue = "";
                retrievedValue = db.StringGet(key).ToString();

                // Bỏ kí tự đặc biệt và giải quyết escape
                //retrievedValue = retrievedValue.Replace("\\\"", "\"").TrimStart('"').TrimEnd('"');

                return retrievedValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Lưu dữ liệu vào redis
        public static bool setData(string strServer, int iPort, int iDB, string strKey, string storedCredential)
        {
            try
            {
                // Địa chỉ IP và cổng của máy chủ Redis
                string redisServer = strServer; // 10.26.7.84 - test
                int redisPort = iPort;       // 6379 - test

                // Tạo đối tượng Configuration cho Redis
                ConfigurationOptions config = new ConfigurationOptions
                {
                    EndPoints = { $"{redisServer}:{redisPort}" },
                    AbortOnConnectFail = false, // Tùy chọn tùy thuộc vào yêu cầu của bạn
                };

                // Tạo đối tượng ConnectionMultiplexer để kết nối đến Redis
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);

                // Lấy đối tượng Database để thực hiện các thao tác trên Redis
                IDatabase db = redis.GetDatabase(iDB); // 14 - test

                // Ví dụ: Thực hiện một số thao tác trên Redis
                string key = strKey; // FIDO:058C123456

                // Lưu giá trị vào 1 key redis
                db.StringSet(key, storedCredential);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool checkUser(string strServer, int iPort, int iDB, string strKey)
        {
            try
            {
                // Địa chỉ IP và cổng của máy chủ Redis
                string redisServer = strServer; // 10.26.7.84 - test
                int redisPort = iPort;       // 6379 - test

                // Tạo đối tượng Configuration cho Redis
                ConfigurationOptions config = new ConfigurationOptions
                {
                    EndPoints = { $"{redisServer}:{redisPort}" },
                    AbortOnConnectFail = false, // Tùy chọn tùy thuộc vào yêu cầu của bạn
                };

                // Tạo đối tượng ConnectionMultiplexer để kết nối đến Redis
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);

                // Lấy đối tượng Database để thực hiện các thao tác trên Redis
                IDatabase db = redis.GetDatabase(iDB); // 14 - test

                // Kiểm tra xem khóa có tồn tại hay không
                bool keyExists = db.KeyExists(strKey);

                List<StoredCredentialJson> storedCredentials = new List<StoredCredentialJson>();
                if (keyExists)
                {
                    string strRs = Redis.getData(strServer, iPort, iDB, strKey);
                    storedCredentials = JsonConvert.DeserializeObject<List<StoredCredentialJson>>(strRs ?? "") ?? new List<StoredCredentialJson>();
                }

                if (keyExists && storedCredentials.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public class RedisModel
    {
        // Các thuộc tính của lớp HocSinh
        public static string? Server { get; set; }
        public static int Port { get; set; }
        public static int DB { get; set; }

        // Constructor mặc định không tham số
        public RedisModel(string strServer, int iPort, int iDB)
        {
            Server = strServer;
            Port = iPort;
            DB = iDB;
        }
    }
}



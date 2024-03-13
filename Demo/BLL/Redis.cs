//using Fido2NetLib.Development;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace WAFido2.BLL
{
    public class Redis
    {
        public static IConfiguration Configuration { get; }
        public static string _Key { get; }
        public static string strServer { get; }
        public static int iPort { get; }
        public static int iDB { get; }

        static Redis()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Configuration = configuration;
            strServer = Configuration["redis:server"];
            iPort = Convert.ToInt32(Configuration["redis:port"]);
            iDB = Convert.ToInt32(Configuration["redis:db"]);
            _Key = Configuration["redis:key"];
        }

        // Lấy dữ liệu từ redis ra
        public static string getData(string user)
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
                string key = _Key + user; // FIDO:058C123456

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
        public static bool setData(string user, string storedCredential)
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
                string key = _Key + user; // FIDO:058C123456

                // Lưu giá trị vào 1 key redis
                db.StringSet(key, storedCredential);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool checkUser(string user)
        {
            try
            {
                // get key
                string strKey = _Key + user;

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
                    string strRs = Redis.getData(user);
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
    
}

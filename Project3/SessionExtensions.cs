using Newtonsoft.Json;

namespace Project3
{
    public static class SessionExtensions
    {
        //tạo đối tượng để  lưu trữ và truy xuất session,chuyển đổi  đối tượng thành chuỗi JSON
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            //chuyển đổi nó thành chuỗi JSON.
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            //chuyển đổi chuỗi JSON trở lại thành đối tượng gốc
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}

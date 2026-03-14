using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ECommerce_WebSite.Extensions
{
    public static class SessionExtensions
    {
        // Extension method to serialize and save an object to the Session
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Extension method to retrieve and deserialize an object from the Session
        public static T? GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default : JsonSerializer.Deserialize<T>(sessionData);
        }
    }
}

using System.Text.Json;

namespace TerracoDaCida.Util
{
    public static class JsonUtil
    {
        public static string ToJson(this Object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static string ToJson(this Object obj, JsonSerializerOptions options)
        {
            return JsonSerializer.Serialize(obj, options);
        }

        public static T ToObject<T>(this string json) where T: class
        {
            return JsonSerializer.Deserialize<T>(json)!;
        }
    }
}

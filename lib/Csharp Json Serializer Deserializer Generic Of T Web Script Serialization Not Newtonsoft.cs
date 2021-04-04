
    public class JsonSerializer<T>
    {
        public string GetJsonStringOfObject(T sourceObject)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(sourceObject);
        }

        public T GetNewObjectFromJsonString(string json)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var deserializedObject = serializer.Deserialize<T>(json);
            return deserializedObject;
        }
    }


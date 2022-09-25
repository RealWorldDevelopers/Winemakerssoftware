using Newtonsoft.Json;
using System;

// https://www.c-sharpcorner.com/UploadFile/20c06b/deserializing-interface-properties-with-json-net/

namespace WMS.Domain
{
    /// <summary>
    /// NewtonSoft.JSON Converter for Interfaces
    /// </summary>
    public class ConcreteConverter<T> : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<T>(reader);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

    }
}

using IFPS.Sales.Domain.Serializers.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace IFPS.Sales.Domain.Serializers.Implementations
{
    public class JsonByteSerializer : IByteSerializer
    {
        public T Deserialize<T>(byte[] arrayToDeserialize)
        {
            if (arrayToDeserialize == null || arrayToDeserialize.Length == 0)
            {
                return default(T);
            }

            using (var stream = new MemoryStream(arrayToDeserialize))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var obj = JsonSerializer.Create().Deserialize(reader, typeof(T));

                    if (!(obj is T))
                    {
                        throw new InvalidOperationException();
                    }
                    else
                    {
                        return (T)obj;
                    }
                }
            }
        }

        object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }

        public object Deserialize(byte[] arrayToDeserialize, Type type)
        {
            if (arrayToDeserialize == null || arrayToDeserialize.Length == 0)
            {
                return GetDefaultValue(type);
            }

            using (var stream = new MemoryStream(arrayToDeserialize))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return JsonSerializer.Create().Deserialize(reader, type);
                }
            }
        }

        public byte[] Serialize<T>(T objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            var json = JsonConvert.SerializeObject(objectToSerialize, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Encoding.UTF8.GetBytes(json);
        }

        public byte[] Serialize(object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            var json = JsonConvert.SerializeObject(objectToSerialize, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Encoding.UTF8.GetBytes(json);
        }
    }
}

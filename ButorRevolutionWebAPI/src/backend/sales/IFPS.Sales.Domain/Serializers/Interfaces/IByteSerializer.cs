using System;

namespace IFPS.Sales.Domain.Serializers.Interfaces
{
    public interface IByteSerializer
    {
        //todo serializable?
        byte[] Serialize<T>(T objectToSerialize);
        T Deserialize<T>(byte[] arrayToDeserialize);

        byte[] Serialize(object objectToSerialize);
        object Deserialize(byte[] arrayToDeserialize, Type type);
    }
}

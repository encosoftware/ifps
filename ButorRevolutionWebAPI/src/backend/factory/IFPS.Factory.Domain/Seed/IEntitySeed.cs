namespace IFPS.Factory.Domain.Seed
{
    public class IEntitySeed<T> where T : class
    {
        T[] Entities { get; }
    }
}

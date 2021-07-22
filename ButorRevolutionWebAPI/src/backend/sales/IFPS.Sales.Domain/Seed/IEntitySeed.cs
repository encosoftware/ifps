namespace IFPS.Sales.Domain.Seed
{
    interface IEntitySeed<T> where T : class
    {
        T[] Entities { get; }
    }
}

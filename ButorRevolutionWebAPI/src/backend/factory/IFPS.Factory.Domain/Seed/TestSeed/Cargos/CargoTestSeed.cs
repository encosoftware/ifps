using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class CargoTestSeed : IEntitySeed<Cargo>
    {
        public Cargo[] Entities => new[]
        {
            new Cargo("Test Cargo Name", 10001, 10000, 10000, null, null, null, null, "Es gab eine Überraschung!", new DateTime(2019, 8, 31), null, null) { Id = 10000 },
            new Cargo("Super Test Cargo Name", 10001, 10000, 10001, null, null, null, null, "Tja!", new DateTime(2019, 10, 6), null, null) { Id = 10001 },
            new Cargo("King Cargo", 10001, 10000, 10001, null, null, null, null, "Your highness!", new DateTime(2019, 10, 6), null, null) { Id = 10002 },
            new Cargo("Delete Cargo", 10001, 10000, 10001, null, null, null, null, "You will be exterminated!", new DateTime(2019, 10, 6), null, null) { Id = 10003 },
            new Cargo("Super Super Cargo", 10001, 10000, 10001, null, null, null, null, "Awesome!", new DateTime(2019, 10, 31), null, null) { Id = 10004 },
            new Cargo("Super Stock Cargo", 10001, 10001, 10001, null, null, null, null, "Wow, stock is here!", new DateTime(2019, 12, 31), null, null) { Id = 10005 },
            new Cargo("Excellent Stock Cargo", 10001, 10001, 10001, null, null, null, null, "Welcome to Fabulous Stock City!", new DateTime(2019, 12, 31), null, null) { Id = 10006 }
        };
    }
}

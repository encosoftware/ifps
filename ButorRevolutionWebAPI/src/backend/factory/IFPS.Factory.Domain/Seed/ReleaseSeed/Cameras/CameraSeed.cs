using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class CameraSeed : IEntitySeed<Camera>
    {
        public Camera[] Entities => new[]
        {
            new Camera("Camera 01", "IP", "241.101.1.5") { Id = 1 },
            new Camera("Camera 02", "IP", "241.101.1.6") { Id = 2 },
            new Camera("Camera 03", "IP", "241.101.1.7") { Id = 3 },
        };
    }
}

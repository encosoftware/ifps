using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Interfaces
{
    public interface ICncCodeGenerationService
    {
        Task<string> GenerateIsoGCode(FurnitureComponent component);

        // if there are machines with unique CNC code generation add more functions here
    }
}

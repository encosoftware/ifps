using IFPS.Factory.Domain.Helper;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Implementations
{
    public class CncCodeGenerationService : ICncCodeGenerationService
    {
        public async Task<string> GenerateIsoGCode(FurnitureComponent component)
        {
            var componentCodeList = CNCCodeConverter.ConvertComponent(component);
            var componentCodeText = string.Join("\n", componentCodeList.ToArray());

            return componentCodeText;
        }
    }
}

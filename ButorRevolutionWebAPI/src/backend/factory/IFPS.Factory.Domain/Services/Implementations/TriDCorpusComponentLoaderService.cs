using IFPS.Factory.Domain.Helper;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Implementations
{
    public class TriDCorpusComponentLoaderService : ITriDCorpusComponentLoaderService
    {
        public void LoadComponentFromXXLFile(ref FurnitureComponent furnitureComponent, string fileContent)
        {
            var lines = fileContent.Split('\n');
            var xxlParser = new XXLParser(furnitureComponent);

            xxlParser.ParseProgramString(lines);
            
        }
    }
}

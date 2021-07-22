using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Interfaces
{
    public interface ITriDCorpusComponentLoaderService
    {
        void LoadComponentFromXXLFile(ref FurnitureComponent furnitureComponent, string fileContent);
    }
}

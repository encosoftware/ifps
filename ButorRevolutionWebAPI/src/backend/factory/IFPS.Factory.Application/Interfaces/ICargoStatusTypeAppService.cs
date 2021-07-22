﻿using IFPS.Factory.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ICargoStatusTypeAppService
    {
        Task<List<CargoStatusTypeDropdownListDto>> GetCargoStatusTypeForDropdownAsync();
    }
}

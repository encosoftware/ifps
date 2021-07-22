using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto.Cargos
{
    public class UpdateCargoStockDto
    {
        public List<OrderedPackageUpdateDto> Package { get; set; }

        public UpdateCargoStockDto()
        {

        }
    }
}

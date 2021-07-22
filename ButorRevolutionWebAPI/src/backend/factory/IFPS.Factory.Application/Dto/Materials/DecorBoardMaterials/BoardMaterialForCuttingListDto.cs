using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class BoardMaterialForCuttingListDto
    {
        public Guid Guid { get; set; }

        public double Width { get; set; }

        public double Length { get; set; }

        public double Thickness { get; set; }

        public bool HasFiberDirection { get; set; }

        public BoardMaterialForCuttingListDto(BoardMaterial boardMaterial)
        {
            Guid = boardMaterial.Id;
            // intentionally swapped width and length
            Width = boardMaterial.Dimension.Length;
            Length = boardMaterial.Dimension.Width;
            // --
            Thickness = boardMaterial.Dimension.Thickness;
            HasFiberDirection = boardMaterial.HasFiberDirection;
        }
    }
}

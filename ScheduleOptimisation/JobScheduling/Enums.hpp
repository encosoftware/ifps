#pragma once
#define NOMINMAX

namespace operations_research {

	enum ProductionType
	{
		LAYOUT = 1,
		CNC = 2,
		EDGING = 3,
		SORTING = 4,
		ASSEMBLY = 5,
		PACKING = 6,
		BORDER = 7,
		BREAK = 8,

		NONE = 0
	};
}
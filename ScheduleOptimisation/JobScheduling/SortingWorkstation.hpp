#pragma once
#define NOMINMAX

#include <iostream>
#include "Workstation.hpp"

namespace operations_research {

	class SortingWorkstation : public Workstation
	{
	public:

		SortingWorkstation() {};
		SortingWorkstation(int id, std::string name, ProductionType type, int sspc, BRDateTime lpe) : Workstation(id, name, type, lpe), SortingSecPerComponent(sspc) {};
		~SortingWorkstation() {};

		int GetSortingSecPerComponent() { return SortingSecPerComponent; }

	private:

		int SortingSecPerComponent;
	};
}
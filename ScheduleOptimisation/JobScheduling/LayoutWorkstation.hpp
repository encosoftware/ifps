#pragma once
#define NOMINMAX

#include <iostream>
#include "Workstation.hpp"

namespace operations_research {

	class LayoutWorkstation : public Workstation
	{
	public:

		LayoutWorkstation() {};
		LayoutWorkstation(int id, std::string name, ProductionType type, int cspc, BRDateTime lpe) : Workstation(id, name, type, lpe), CuttingSecPerComponent(cspc) {};
		~LayoutWorkstation() {};

		int GetCuttingSecPerComponent() { return CuttingSecPerComponent; }

	private:

		int CuttingSecPerComponent;
	};
}
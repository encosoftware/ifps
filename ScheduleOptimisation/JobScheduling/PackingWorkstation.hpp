#pragma once
#define NOMINMAX

#include <iostream>
#include "Workstation.hpp"

namespace operations_research {

	class PackingWorkstation : public Workstation
	{
	public:

		PackingWorkstation() {};
		PackingWorkstation(int id, std::string name, ProductionType type, int pspc, BRDateTime lpe) : Workstation(id, name, type, lpe), PackingSecPerComponent(pspc) {};
		~PackingWorkstation() {};

		int GetPackingSecPerComponent() { return PackingSecPerComponent; }

	private:

		int PackingSecPerComponent;
	};
}
#pragma once
#define NOMINMAX

#include <iostream>
#include "Workstation.hpp"

namespace operations_research {

	class EdgingWorkstation : public Workstation
	{
	public:

		EdgingWorkstation() {};
		EdgingWorkstation(int id, std::string name, ProductionType type, int spe, BRDateTime lpe) : Workstation(id, name, type, lpe), SecPerEdge(spe) {};
		~EdgingWorkstation() {};

		int GetSecPerEdge() { return SecPerEdge; }

	private:

		int SecPerEdge;
	};
}
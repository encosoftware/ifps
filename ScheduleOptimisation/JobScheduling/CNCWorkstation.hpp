#pragma once
#define NOMINMAX

#include <iostream>
#include "Workstation.hpp"

namespace operations_research {

	class CNCWorkstation : public Workstation
	{
	public:
	
		CNCWorkstation() {};
		CNCWorkstation(int id, std::string name, ProductionType type, int dps, int sph, BRDateTime lpe) : Workstation(id, name, type, lpe), DistancePerSec(dps), SecPerHole(sph) {};
		~CNCWorkstation() {};

		int GetDistancePerSec() { return DistancePerSec; }
		int GetSecPerHole() { return SecPerHole; }

	private:

		int DistancePerSec;
		int SecPerHole;
	};
}
#pragma once
#define NOMINMAX

#include <iostream>
#include "Workstation.hpp"

namespace operations_research {

	class AssemblyWorkstation : public Workstation
	{
	public:

		AssemblyWorkstation() {};
		AssemblyWorkstation(int id, std::string name, ProductionType type, int aspc, int spa, BRDateTime lpe) : Workstation(id, name, type, lpe), AssemblySecPerComponent(aspc), 
			SecPerAccessory(spa) {};
		~AssemblyWorkstation() {};

		int GetAssemblySecPerComponent() { return AssemblySecPerComponent; }
		int GetSecPerAccessory() { return SecPerAccessory; }

	private:

		int AssemblySecPerComponent;
		int SecPerAccessory;
	};
}
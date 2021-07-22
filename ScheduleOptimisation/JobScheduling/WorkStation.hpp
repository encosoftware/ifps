#pragma once
#define NOMINMAX

#include <iostream>
#include "Enums.hpp"

namespace operations_research {

	class Workstation
	{
	public:

		Workstation() {};
		Workstation(int id, std::string name, ProductionType type, BRDateTime last_plan_end) : Id(id), Name(name), ProdType(type), LastPlanEnd(last_plan_end) {};
		~Workstation() {};

		int GetId() { return Id; }
		std::string GetName() { return Name; }
		ProductionType GetProductionType() { return ProdType; }
		BRDateTime GetLastPlanEnd() { return LastPlanEnd; }

	protected:

		int Id;
		std::string Name;
		ProductionType ProdType;

		BRDateTime LastPlanEnd;
	};
}
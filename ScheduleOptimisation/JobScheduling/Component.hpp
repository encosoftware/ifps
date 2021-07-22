#pragma once
#define NOMINMAX

#include <iostream>

namespace operations_research {

	class Component 
	{
	public:

		int Id;
		int UnitId;
		int CncDistance;
		int CncHoles;
		int FoilNum;
		std::string OrderGuid;
		std::string Name;
		int CfcProductionStatus;

		Component() {};
		Component(int id, int unit_id, std::string order_guid, std::string name, int cnc_d, int cnc_h, int foil_num, int prod_stat) : Id(id), UnitId(unit_id), OrderGuid(order_guid), Name(name),
			CncDistance(cnc_d), CncHoles(cnc_h), FoilNum(foil_num), CfcProductionStatus(prod_stat) {};
		~Component() {};
	};
}
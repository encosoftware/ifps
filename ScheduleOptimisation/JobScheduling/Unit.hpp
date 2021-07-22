#pragma once
#define NOMINMAX

#include <iostream>

namespace operations_research {

	class Unit 
	{
	public:

		int Id;
		int Accessories;
		std::string Name;
		std::string OrderGuid;
		std::vector<int> ComponentIds;
		int CfuProductionStatus;

		Unit() {};
		Unit(int id, std::string order_guid, std::string name, std::vector<int> component_ids, int accessories, int prod_stat) : Id(id), OrderGuid(order_guid), Name(name), 
			ComponentIds(component_ids), Accessories(accessories), CfuProductionStatus(prod_stat) {};
		~Unit() {};
	};
}
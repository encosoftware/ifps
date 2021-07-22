#pragma once
#define NOMINMAX

#include "Plan.hpp"

namespace operations_research {

	class AssemblyPlan : public Plan
	{
	public:

		AssemblyPlan() {};
		AssemblyPlan(int ws_id, std::string order_guid, int unit_id, std::vector<int> component_ids, int accessories) : Plan(ws_id), OrderGuid(order_guid), ComponentIds(component_ids), 
			UnitId(unit_id), Accessories(accessories) {
			ProdType = ProductionType::ASSEMBLY;
			Name = "Assembly-Plan</br>O-" + order_guid + "</br>U-" + std::to_string(unit_id);
		};
		~AssemblyPlan() {};

		int EstimateTime(AssemblyWorkstation ws);
		friend std::ostream& operator<< (std::ostream& out, const AssemblyPlan& plan);

		std::string PrintHtml(sat::CpSolverResponse result, int minute_pixel_rate);
		json ToJson(sat::CpSolverResponse result, int wsh, int shift_num, int shift_len);

		std::string GetOrderGuid() { return OrderGuid; }
		int GetUnitId() { return UnitId; }
		int GetAccessories() { return Accessories; }
		std::vector<int> GetComponentId() { return ComponentIds; }

	private:

		int UnitId;
		int Accessories;
		std::string OrderGuid;
		std::vector<int> ComponentIds;
	};
}
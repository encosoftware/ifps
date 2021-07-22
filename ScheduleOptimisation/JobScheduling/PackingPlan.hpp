#pragma once
#define NOMINMAX

#include "Plan.hpp"

namespace operations_research {

	class PackingPlan : public Plan
	{
	public:

		PackingPlan() {};
		PackingPlan(int ws_id, std::string order_guid, int unit_id, std::vector<int> component_ids) : Plan(ws_id), OrderGuid(order_guid), ComponentIds(component_ids), UnitId(unit_id) {
			ProdType = ProductionType::PACKING;
			Name = "Packing-Plan</br>O-" + order_guid + "</br>U-" + std::to_string(unit_id);
		};
		~PackingPlan() {};

		int EstimateTime(PackingWorkstation ws);
		friend std::ostream& operator<< (std::ostream& out, const PackingPlan& plan);

		std::string PrintHtml(sat::CpSolverResponse result, int minute_pixel_rate);
		json ToJson(sat::CpSolverResponse result, int wsh, int shift_num, int shift_len);

		std::string GetOrderId() { return OrderGuid; }
		int GetUnitId() { return UnitId; }
		std::vector<int> GetComponentId() { return ComponentIds; }

	private:

		std::string OrderGuid;
		int UnitId;
		std::vector<int> ComponentIds;
	};
}
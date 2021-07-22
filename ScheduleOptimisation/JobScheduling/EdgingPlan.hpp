#pragma once
#define NOMINMAX

#include "Plan.hpp"

namespace operations_research {

	class EdgingPlan : public Plan
	{
	public:

		EdgingPlan() {};
		EdgingPlan(int ws_id, int component_id, std::string order_guid, int foil_num) : Plan(ws_id), OrderGuid(order_guid), ComponentId(component_id), FoilNum(foil_num) {
			ProdType = ProductionType::EDGING;
			Name = "Edging-Plan</br>O-" + order_guid + "</br>C-" + std::to_string(component_id);
		};
		~EdgingPlan() {};

		int EstimateTime(EdgingWorkstation ws);
		friend std::ostream& operator<< (std::ostream& out, const EdgingPlan& plan);

		std::string PrintHtml(sat::CpSolverResponse result, int minute_pixel_rate);
		json ToJson(sat::CpSolverResponse result, int wsh, int shift_num, int shift_len);

		std::string GetOrderGuid() { return OrderGuid; }
		int GetComponentId() { return ComponentId; }
		int GetFoilNum() { return FoilNum; }

	private:

		std::string OrderGuid;
		int ComponentId;
		int FoilNum;
	};
}
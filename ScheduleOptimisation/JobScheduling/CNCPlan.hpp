#pragma once
#define NOMINMAX

#include "Plan.hpp"

namespace operations_research {

	class CNCPlan : public Plan
	{
	public:

		CNCPlan() {};
		CNCPlan(int ws_id, int component_id, std::string order_guid, int cnc_distance, int cnc_holes) : Plan(ws_id), OrderGuid(order_guid), ComponentId(component_id),
			CncDistance(cnc_distance), CncHoles(cnc_holes) {
			ProdType = ProductionType::CNC;
			Name = "CNC-Plan</br>O-" + order_guid + "</br>C-" + std::to_string(component_id);
		};
		~CNCPlan() {};

		int EstimateTime(CNCWorkstation ws, float cnc_distance_multiplier);
		friend std::ostream& operator<< (std::ostream& out, const CNCPlan& plan);

		std::string PrintHtml(sat::CpSolverResponse result, int minute_pixel_rate);
		json ToJson(sat::CpSolverResponse result, int wsh, int shift_num, int shift_len);

		std::string GetOrderId() { return OrderGuid; }
		int GetComponentId() { return ComponentId; }
		int GetCncDistance() { return CncDistance; }
		int GetCncHoles() { return CncHoles; }

	private:

		int ComponentId;
		int CncDistance;
		int CncHoles;
		std::string OrderGuid;
	};
}
#pragma once
#define NOMINMAX

#include "Plan.hpp"

namespace operations_research {

	class LayoutPlan : public Plan
	{
	public:

		LayoutPlan() {};
		LayoutPlan(int ws_id, std::vector<int> component_ids, std::vector<std::string> order_guids, std::string board_guid, std::vector<Cutting> cuttings) : Plan(ws_id),
			ComponentIds(component_ids), OrderGuids(order_guids), BoardGuid(board_guid), Cuttings(cuttings) {
				ProdType = ProductionType::LAYOUT;
				Name = "Layout-Plan";
				for (auto& guid : order_guids) { Name += "</br>O-" + guid.substr(0,4); }
		};
		~LayoutPlan() {};

		int EstimateTime(LayoutWorkstation ws);
		friend std::ostream& operator<< (std::ostream& out, const LayoutPlan& plan);

		std::string PrintHtml(sat::CpSolverResponse result, int minute_pixel_rate);
		json ToJson(sat::CpSolverResponse result, int wsh, int shift_num, int shift_len);

		std::vector<std::string> GetOrderIds() { return OrderGuids; }
		std::vector<int> GetComponentIds() { return ComponentIds; }
		std::vector<Cutting> GetCuttings() { return Cuttings; }
		std::string GetMaterialGuid() { return BoardGuid; }

	private:

		std::vector<int> ComponentIds;
		std::vector<std::string> OrderGuids;
		std::vector<Cutting> Cuttings;
		std::string BoardGuid;
	};
}
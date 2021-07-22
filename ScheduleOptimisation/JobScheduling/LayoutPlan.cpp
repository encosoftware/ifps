#include "LayoutPlan.hpp"

namespace operations_research {

	std::ostream& operator<< (std::ostream& out, const LayoutPlan& plan) {

		out << "  * " << plan.Name << std::endl;
		out << "     Workstation ID: " << plan.WorkStationId << std::endl;
		out << "     Orders          ";
		for (auto& o_guid : plan.OrderGuids) { out << o_guid << " "; }
		out << std::endl;
		out << "     Components:     ";
		for (auto& c_id : plan.ComponentIds) { out << c_id << " "; }
		out << std::endl;
		out << "     Execution time: " << plan.Schedule.SizeVar() << " min";

		return out;
	}

	int LayoutPlan::EstimateTime(LayoutWorkstation ws) {

		int component_num = ComponentIds.size();
		float time_estimate = std::ceil(component_num * ws.GetCuttingSecPerComponent() / 60);

		return std::max((int)time_estimate, 1);
	}

	std::string LayoutPlan::PrintHtml(sat::CpSolverResponse result, int minute_pixel_rate) {

		int job_length = sat::SolutionIntegerValue(result, Schedule.SizeVar()) * minute_pixel_rate - 2;
		int job_start = sat::SolutionIntegerValue(result, Schedule.StartVar()) * minute_pixel_rate;
		std::string html_content = "<div class='job j-" + std::to_string(ProdType) + "' style='width:" + std::to_string(job_length) +
			"px; left:" + std::to_string(job_start) + "px;'><p>" +
			Name + "</br>" +
			"T: <b>" + std::to_string(sat::SolutionIntegerValue(result, Schedule.SizeVar())) + " min</b>" +
			+ "</p></div>";

		return html_content;
	}

	json LayoutPlan::ToJson(sat::CpSolverResponse result, int wsh, int shift_num, int shift_len) {

		std::vector<json> cuttings;
		for (auto& c : Cuttings) {
			json tmp_c = {
				{ "topLeftX", c.TopLeftX },
				{ "topLeftY", c.TopLeftY },
				{ "width", c.Width },
				{ "length", c.Length },
				{ "componentId", c.ComponentId }
			};
			cuttings.push_back(tmp_c);
		}

		std::vector<std::string> order_ids;
		for (auto& id : OrderGuids)
		{
			order_ids.push_back(id);
		}

		BRDateTime start_dt = CalculateDateTime(sat::SolutionIntegerValue(result, Schedule.StartVar()), wsh, shift_num, shift_len);
		BRDateTime end_dt = CalculateDateTime(sat::SolutionIntegerValue(result, Schedule.EndVar()), wsh, shift_num, shift_len);

		json plan = {
			{ "orderIds", order_ids },
			{ "scheduledDate", start_dt.DayToString() },
			{ "scheduledStartHour", start_dt.Hour },
			{ "scheduledStartMinute", start_dt.Minute },
			{ "scheduledEndHour", end_dt.Hour },
			{ "scheduledEndMinute", end_dt.Minute },
			{ "concreteFurnitureComponentId", nullptr },
			{ "concreteFurnitureUnitId", nullptr },
			{ "workStationId", WorkStationId },
			{ "boardId", BoardGuid },
			{ "cuttings", cuttings }
		};

		return plan;
	}
}
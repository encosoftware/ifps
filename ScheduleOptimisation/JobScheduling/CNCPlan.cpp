#include "CNCPlan.hpp"

namespace operations_research {

	std::ostream& operator<< (std::ostream& out, const CNCPlan& plan) {

		out << "  * " << plan.Name << std::endl;
		out << "     Workstation ID: " << plan.WorkStationId << std::endl;
		out << "     Order Guid:     " << plan.OrderGuid << std::endl;
		out << "     Component ID:   " << plan.ComponentId << std::endl;
		out << "     Execution time: " << plan.Schedule.SizeVar() << " min";

		return out;
	}
	
	int CNCPlan::EstimateTime(CNCWorkstation ws, float cnc_distance_multiplier) {

		float time_estimate = std::ceil(((((float)CncDistance * cnc_distance_multiplier) / (float)ws.GetDistancePerSec()) + ((float)CncHoles * (float)ws.GetSecPerHole())) / 60);

		return std::max(int(time_estimate), 1);
	}

	std::string CNCPlan::PrintHtml(sat::CpSolverResponse result, int minute_pixel_rate) {

		int job_length = sat::SolutionIntegerValue(result, Schedule.SizeVar()) * minute_pixel_rate - 2;
		int job_start = sat::SolutionIntegerValue(result, Schedule.StartVar()) * minute_pixel_rate;
		std::string html_content = "<div class='job j-" + std::to_string(ProdType) + "' style='width:" + std::to_string(job_length) +
			"px; left:" + std::to_string(job_start) + "px;'><p>" +
			Name + "</br>" +
			"T: <b>" + std::to_string(sat::SolutionIntegerValue(result, Schedule.SizeVar())) + " min</b>" +
			+ "</p></div>";

		return html_content;
	}

	json CNCPlan::ToJson(sat::CpSolverResponse result, int wsh, int shift_num, int shift_len) {

		BRDateTime start_dt = CalculateDateTime(sat::SolutionIntegerValue(result, Schedule.StartVar()), wsh, shift_num, shift_len);
		BRDateTime end_dt = CalculateDateTime(sat::SolutionIntegerValue(result, Schedule.EndVar()), wsh, shift_num, shift_len);

		json plan = {
			{ "orderId", OrderGuid },
			{ "scheduledDate", start_dt.DayToString() },
			{ "scheduledStartHour", start_dt.Hour },
			{ "scheduledStartMinute", start_dt.Minute },
			{ "scheduledEndHour", end_dt.Hour },
			{ "scheduledEndMinute", end_dt.Minute },
			{ "concreteFurnitureComponentId", ComponentId },
			{ "concreteFurnitureUnitId", nullptr },
			{ "workStationId", WorkStationId }
		};
		
		return plan;
	}
}
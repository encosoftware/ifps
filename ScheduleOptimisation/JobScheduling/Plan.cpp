#include "Plan.hpp"

namespace operations_research {

	std::string Plan::PrintHtml(sat::CpSolverResponse result, int minute_pixel_rate) {

		std::string html_content;
		if (ProdType == ProductionType::BREAK) {
			int job_length = sat::SolutionIntegerValue(result, Schedule.SizeVar()) * minute_pixel_rate - 2;
			int job_start = sat::SolutionIntegerValue(result, Schedule.StartVar()) * minute_pixel_rate;
			html_content = "<div class='job break' style='width:" + std::to_string(job_length) +
				"px; left:" + std::to_string(job_start) + "px;'><p>Lunch break</p></div>";
		}
		else {
			int job_length = sat::SolutionIntegerValue(result, Schedule.SizeVar()) * minute_pixel_rate - 2;
			int job_start = sat::SolutionIntegerValue(result, Schedule.StartVar()) * minute_pixel_rate;
			html_content = "<div class='job border' style='width:" + std::to_string(job_length) +
				"px; left:" + std::to_string(job_start) + "px;'></div>";
		}

		return html_content;
	}
}
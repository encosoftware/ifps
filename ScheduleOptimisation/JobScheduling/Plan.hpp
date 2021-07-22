#pragma once
#define NOMINMAX

#include <iostream>
#include <vector>
#include <algorithm>

#include "Enums.hpp"
#include "Utils.hpp"

#include "Workstation.hpp"
#include "LayoutWorkstation.hpp"
#include "CNCWorkstation.hpp"
#include "EdgingWorkstation.hpp"
#include "SortingWorkstation.hpp"
#include "AssemblyWorkstation.hpp"
#include "PackingWorkstation.hpp"
#include "Cutting.hpp"

#include "ortools/sat/cp_model.h"
#include "json.hpp"

using json = nlohmann::json;
using json_data = nlohmann::detail::iteration_proxy_value<nlohmann::detail::iter_impl<nlohmann::json> >;
using json_array = nlohmann::detail::iteration_proxy<nlohmann::detail::iter_impl<nlohmann::json> >;
using json_value = nlohmann::json::value_type;

namespace operations_research {

	class Plan
	{
	public:
		
		ProductionType ProdType;
		std::string Name;

		Plan() {};
		Plan(int ws_id) : WorkStationId(ws_id) {};

		void SetSchedule(sat::IntervalVar schedule) { Schedule = schedule; };
		sat::IntervalVar GetSchedule() { return Schedule; };
		int GetWorkStationId() { return WorkStationId; };

		virtual std::string PrintHtml(sat::CpSolverResponse result, int minute_pixel_rate);

	protected:

		sat::IntervalVar Schedule;
		int WorkStationId;
	};
}
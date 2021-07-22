#pragma once
#define NOMINMAX

#include <iostream>
#include <vector>
#include <fstream>

#include "Component.hpp"
#include "Unit.hpp"
#include "Order.hpp"

#include "LayoutPlan.hpp"
#include "CNCPlan.hpp"
#include "EdgingPlan.hpp"
#include "SortingPlan.hpp"
#include "AssemblyPlan.hpp"
#include "PackingPlan.hpp"

#include "Workstation.hpp"
#include "LayoutWorkstation.hpp"
#include "CNCWorkstation.hpp"
#include "EdgingWorkstation.hpp"
#include "SortingWorkstation.hpp"
#include "AssemblyWorkstation.hpp"
#include "PackingWorkstation.hpp"

#include "ortools/sat/cp_model.h"
#include "json.hpp"

using json = nlohmann::json;
using json_data = nlohmann::detail::iteration_proxy_value<nlohmann::detail::iter_impl<nlohmann::json> >;
using json_array = nlohmann::detail::iteration_proxy<nlohmann::detail::iter_impl<nlohmann::json> >;
using json_value = nlohmann::json::value_type;

namespace operations_research {

	class DataLoader
	{
	public:

		DataLoader() {};
		~DataLoader() {};

		std::vector<Component> LoadComponents(json_array component_array);
		std::vector<Unit> LoadUnits(json_array unit_array);
		std::vector<Order> LoadOrders(json_array order_array);

		std::vector<LayoutWorkstation> LoadLayoutWorkstations(json_array layout_workstation_array);
		std::vector<CNCWorkstation> LoadCNCWorkstations(json_array cnc_workstation_array);
		std::vector<EdgingWorkstation> LoadEdgingWorkstations(json_array edging_workstation_array);
		std::vector<SortingWorkstation> LoadSortingWorkstations(json_array sorting_workstation_array);
		std::vector<AssemblyWorkstation> LoadAssemblyWorkstations(json_array assembly_workstation_array);
		std::vector<PackingWorkstation> LoadPackingWorkstations(json_array packing_workstation_array);

	private:

	};
}
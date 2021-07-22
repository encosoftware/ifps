#include "DataLoader.hpp"

namespace operations_research {

	std::vector<Component> DataLoader::LoadComponents(json_array component_array) {
		LOG(INFO) << "\nStarted loading Components...";

		std::vector<Component> components;

		for (auto& component_data : component_array) {

			int id = component_data.value()["id"];
			int unit_id = component_data.value()["concreteFurnitureUnitId"];
			std::string order_guid = component_data.value()["orderGuid"];
			int cnc_d = component_data.value()["cncDistance"];
			int cnc_h = component_data.value()["cncHoles"];
			int foil_num = component_data.value()["foilNumber"];
			std::string name = component_data.value()["name"];
			int prod_stat = component_data.value()["cfcProductionStatus"];

			Component component(id, unit_id, order_guid, name, cnc_d, cnc_h, foil_num, prod_stat);
			components.push_back(component);
		}

		LOG(INFO) << "  Finished loading Components!";

		return components;
	}

	std::vector<Unit> DataLoader::LoadUnits(json_array unit_array) {
		LOG(INFO) << "\nStarted loading Units...";

		std::vector<Unit> units;

		for (auto& unit_data : unit_array) {

			int id = unit_data.value()["id"];
			std::string order_guid = unit_data.value()["orderGuid"];
			std::string name = unit_data.value()["name"];
			std::vector<int> component_ids = unit_data.value()["componentIds"];
			int accessories = unit_data.value()["accessories"];
			int prod_stat = unit_data.value()["cfuProductionStatus"];

			Unit unit(id, order_guid, name, component_ids, accessories, prod_stat);
			units.push_back(unit);
		}

		LOG(INFO) << "  Finished loading Units!";

		return units;
	}

	std::vector<Order> DataLoader::LoadOrders(json_array order_array) {
		LOG(INFO) << "\nStarted loading Orders...";

		std::vector<Order> orders;

		for (auto& order_data : order_array) {

			std::string guid = order_data.value()["guid"];
			std::string name = order_data.value()["orderName"];
			std::string deadline_str = order_data.value()["deadline"];

			Order order(guid, name, deadline_str);
			orders.push_back(order);
		}

		LOG(INFO) << "  Finished loading Orders!";

		return orders;
	}

	std::vector<LayoutWorkstation> DataLoader::LoadLayoutWorkstations(json_array layout_workstation_array) {
	
		LOG(INFO) << "\nStarted loading LayoutWorkstations...";

		std::vector<LayoutWorkstation> workstations;

		for (auto& workstation_data : layout_workstation_array) {

			int id = workstation_data.value()["id"];
			std::string name = workstation_data.value()["name"];
			ProductionType type = static_cast<ProductionType>(workstation_data.value()["type"]);
			int cspc = workstation_data.value()["cuttingSecPerComponent"];

			std::string date_str = workstation_data.value()["lastPlanEnd"]["date"];
			int h = workstation_data.value()["lastPlanEnd"]["hour"];
			int m = workstation_data.value()["lastPlanEnd"]["minute"];

			BRDateTime last_plan_end(date_str, h, m);

			LayoutWorkstation ws(id, name, type, cspc, last_plan_end);
			workstations.push_back(ws);
		}

		LOG(INFO) << "  Finished loading LayoutWorkstations!";

		return workstations;
	}

	std::vector<CNCWorkstation> DataLoader::LoadCNCWorkstations(json_array cnc_workstation_array) {

		LOG(INFO) << "\nStarted loading CNCWorkstations...";

		std::vector<CNCWorkstation> workstations;

		for (auto& workstation_data : cnc_workstation_array) {

			int id = workstation_data.value()["id"];
			std::string name = workstation_data.value()["name"];
			ProductionType type = static_cast<ProductionType>(workstation_data.value()["type"]);
			int dps = workstation_data.value()["distancePerSec"];
			int sph = workstation_data.value()["secPerHole"];

			std::string date_str = workstation_data.value()["lastPlanEnd"]["date"];
			int h = workstation_data.value()["lastPlanEnd"]["hour"];
			int m = workstation_data.value()["lastPlanEnd"]["minute"];

			BRDateTime last_plan_end(date_str, h, m);

			CNCWorkstation ws(id, name, type, dps, sph, last_plan_end);
			workstations.push_back(ws);
		}

		LOG(INFO) << "  Finished loading CNCWorkstations!";

		return workstations;
	}

	std::vector<EdgingWorkstation> DataLoader::LoadEdgingWorkstations(json_array edging_workstation_array) {

		LOG(INFO) << "\nStarted loading EdgingWorkstations...";

		std::vector<EdgingWorkstation> workstations;

		for (auto& workstation_data : edging_workstation_array) {

			int id = workstation_data.value()["id"];
			std::string name = workstation_data.value()["name"];
			ProductionType type = static_cast<ProductionType>(workstation_data.value()["type"]);
			int spe = workstation_data.value()["secPerEdge"];

			std::string date_str = workstation_data.value()["lastPlanEnd"]["date"];
			int h = workstation_data.value()["lastPlanEnd"]["hour"];
			int m = workstation_data.value()["lastPlanEnd"]["minute"];

			BRDateTime last_plan_end(date_str, h, m);

			EdgingWorkstation ws(id, name, type, spe, last_plan_end);
			workstations.push_back(ws);
		}

		LOG(INFO) << "  Finished loading EdgingWorkstations!";

		return workstations;
	}

	std::vector<SortingWorkstation> DataLoader::LoadSortingWorkstations(json_array sorting_workstation_array) {

		LOG(INFO) << "\nStarted loading SortingWorkstations...";

		std::vector<SortingWorkstation> workstations;

		for (auto& workstation_data : sorting_workstation_array) {

			int id = workstation_data.value()["id"];
			std::string name = workstation_data.value()["name"];
			ProductionType type = static_cast<ProductionType>(workstation_data.value()["type"]);
			int sspc = workstation_data.value()["sortingSecPerComponent"];

			std::string date_str = workstation_data.value()["lastPlanEnd"]["date"];
			int h = workstation_data.value()["lastPlanEnd"]["hour"];
			int m = workstation_data.value()["lastPlanEnd"]["minute"];

			BRDateTime last_plan_end(date_str, h, m);

			SortingWorkstation ws(id, name, type, sspc, last_plan_end);
			workstations.push_back(ws);
		}

		LOG(INFO) << "  Finished loading SortingWorkstations!";

		return workstations;
	}

	std::vector<AssemblyWorkstation> DataLoader::LoadAssemblyWorkstations(json_array assembly_workstation_array) {

		LOG(INFO) << "\nStarted loading AssemblyWorkstations...";

		std::vector<AssemblyWorkstation> workstations;

		for (auto& workstation_data : assembly_workstation_array) {

			int id = workstation_data.value()["id"];
			std::string name = workstation_data.value()["name"];
			ProductionType type = static_cast<ProductionType>(workstation_data.value()["type"]);
			int aspc = workstation_data.value()["assemblySecPerComponent"];
			int spa = workstation_data.value()["secPerAccessory"];

			std::string date_str = workstation_data.value()["lastPlanEnd"]["date"];
			int h = workstation_data.value()["lastPlanEnd"]["hour"];
			int m = workstation_data.value()["lastPlanEnd"]["minute"];

			BRDateTime last_plan_end(date_str, h, m);

			AssemblyWorkstation ws(id, name, type, aspc, spa, last_plan_end);
			workstations.push_back(ws);
		}

		LOG(INFO) << "  Finished loading AssemblyWorkstations!";

		return workstations;
	}

	std::vector<PackingWorkstation> DataLoader::LoadPackingWorkstations(json_array packing_workstation_array) {

		LOG(INFO) << "\nStarted loading PackingWorkstations...";

		std::vector<PackingWorkstation> workstations;

		for (auto& workstation_data : packing_workstation_array) {

			int id = workstation_data.value()["id"];
			std::string name = workstation_data.value()["name"];
			ProductionType type = static_cast<ProductionType>(workstation_data.value()["type"]);
			int pspc = workstation_data.value()["packingSecPerComponent"];

			std::string date_str = workstation_data.value()["lastPlanEnd"]["date"];
			int h = workstation_data.value()["lastPlanEnd"]["hour"];
			int m = workstation_data.value()["lastPlanEnd"]["minute"];

			BRDateTime last_plan_end(date_str, h, m);

			PackingWorkstation ws(id, name, type, pspc, last_plan_end);
			workstations.push_back(ws);
		}

		LOG(INFO) << "  Finished loading PackingWorkstations!";

		return workstations;
	}
}
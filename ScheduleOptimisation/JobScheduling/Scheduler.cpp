#include "Scheduler.hpp"

namespace operations_research {

	Scheduler::Scheduler(int shift_num, int shift_length_hours, SchedulerSettings settings, std::string output_dir, std::string html_dir) {

		LOG(INFO) << "\nInitializing Scheduler...";

		ShiftNumber = shift_num;
		ShiftLengthHours = shift_length_hours;
		OutputDir = output_dir;
		HtmlDir = html_dir;
		sat::CpModelBuilder cpmodel;
		MODEL = cpmodel;

		if (settings.AvgNumOfComponentsPerHour <= 0) {
			throw std::out_of_range("Average component number per hour <= 0");
		}
		
		if (settings.WorkStartHour <= 0 || settings.WorkStartHour > 24) {
			throw std::out_of_range("Work start hour <= 0 or > 24");
		}
		
		if (settings.HoursTillBreak <= 0) {
			throw std::out_of_range("Hours till break <= 0");
		}
		
		if (settings.LunchDurationMinutes <= 0) {
			throw std::out_of_range("Lunch duration  <= 0");
		}

		if (shift_length_hours < settings.HoursTillBreak + (float)settings.LunchDurationMinutes / 60) {
			throw std::out_of_range("Lunch break outside of shift!");
		}

		Settings = settings;

		TodayStart = BRDateTime(settings.WorkStartHour);
		LOG(INFO) << "  Date of scheduling: " << TodayStart.Day;

		LOG(INFO) << "  Scheduler ready!";
	}

	void Scheduler::Init(std::string input_data_file, std::string cuttings_data_file, bool reschedule) {

		LOG(INFO) << "\nDATA LOADING";
		LOG(INFO) << "  Reading from file: " << input_data_file;
		std::ifstream input_data_file_handler;
		input_data_file_handler.open(input_data_file);
		static json DATA;
		input_data_file_handler >> DATA;

		DataLoader data_loader;

		auto orders = data_loader.LoadOrders(DATA["orderDetails"].items());
		if (orders.size() < 1) {
			throw std::exception("Number of orders < 1");
		}
		auto components = data_loader.LoadComponents(DATA["concreteFurnitureComponents"].items());
		if (components.size() < 1) {
			throw std::exception("Number of components < 1");
		}
		auto units = data_loader.LoadUnits(DATA["concreteFurnitureUnits"].items());
		if (units.size() < 1) {
			throw std::exception("Number of units < 1");
		}

		auto layout_workstations = data_loader.LoadLayoutWorkstations(DATA["workstations"]["layoutWorkstations"].items());
		if (layout_workstations.size() < 1) {
			throw std::exception("0 layout workstations were provided!");
		}
		auto cnc_workstations = data_loader.LoadCNCWorkstations(DATA["workstations"]["cncWorkstations"].items());
		if (cnc_workstations.size() < 1) {
			throw std::exception("0 CNC workstations were provided!");
		}
		auto edging_workstations = data_loader.LoadEdgingWorkstations(DATA["workstations"]["edgingWorkstations"].items());
		if (edging_workstations.size() < 1) {
			throw std::exception("0 edging workstations were provided!");
		}
		auto sorting_workstations = data_loader.LoadSortingWorkstations(DATA["workstations"]["sortingWorkstations"].items());
		if (sorting_workstations.size() < 1) {
			throw std::exception("0 sorting workstations were provided!");
		}
		auto assembly_workstations = data_loader.LoadAssemblyWorkstations(DATA["workstations"]["assemblyWorkstations"].items());
		if (assembly_workstations.size() < 1) {
			throw std::exception("0 assembly workstations were provided!");
		}
		auto packing_workstations = data_loader.LoadPackingWorkstations(DATA["workstations"]["packingWorkstations"].items());
		if (packing_workstations.size() < 1) {
			throw std::exception("0 packing workstations were provided!");
		}

		int estimated_work_hours = std::ceil((float)components.size() / Settings.AvgNumOfComponentsPerHour);
		int work_hours_per_day = ShiftNumber * ShiftLengthHours;
		int planned_days = std::ceil((float)estimated_work_hours / (float)work_hours_per_day);
		Domain schedule_domain = CalculateDomain(planned_days);

		bool plan_details = true;

		auto layout_plans = CreateLayoutPlans(schedule_domain, cuttings_data_file, layout_workstations, plan_details);
		auto cnc_plans = CreateCNCPlans(schedule_domain, components, cnc_workstations, plan_details);
		auto edging_plans = CreateEdgingPlans(schedule_domain, components, edging_workstations, plan_details);
		auto sorting_plans = CreateSortingPlans(schedule_domain, units, sorting_workstations, plan_details);
		auto assembly_plans = CreateAssemblyPlans(schedule_domain, units, assembly_workstations, plan_details);
		auto packing_plans = CreatePackingPlans(schedule_domain, units, packing_workstations, plan_details);

		AddWorkstationNoOverlap(layout_workstations, layout_plans, reschedule);
		AddWorkstationNoOverlap(cnc_workstations, cnc_plans, reschedule);
		AddWorkstationNoOverlap(edging_workstations, edging_plans, reschedule);
		AddWorkstationNoOverlap(sorting_workstations, sorting_plans, reschedule);
		AddWorkstationNoOverlap(assembly_workstations, assembly_plans, reschedule);
		AddWorkstationNoOverlap(packing_workstations, packing_plans, reschedule);
		
		auto lunch_breaks = CreateLunchBreaks(planned_days);
		AddPairwiseNoOverlap(lunch_breaks, layout_plans);
		AddPairwiseNoOverlap(lunch_breaks, cnc_plans);
		AddPairwiseNoOverlap(lunch_breaks, edging_plans);
		AddPairwiseNoOverlap(lunch_breaks, sorting_plans);
		AddPairwiseNoOverlap(lunch_breaks, assembly_plans);
		AddPairwiseNoOverlap(lunch_breaks, packing_plans);

		std::vector<Plan> borders_and_breaks = lunch_breaks;

		if (planned_days > 1) {
			auto borders = CreateDayBorders(planned_days);
			AddPairwiseNoOverlap(borders, layout_plans);
			AddPairwiseNoOverlap(borders, cnc_plans);
			AddPairwiseNoOverlap(borders, edging_plans);
			AddPairwiseNoOverlap(borders, sorting_plans);
			AddPairwiseNoOverlap(borders, assembly_plans);
			AddPairwiseNoOverlap(borders, packing_plans);
			borders_and_breaks.insert(borders_and_breaks.end(), std::make_move_iterator(borders.begin()), std::make_move_iterator(borders.end()));
		}
		
		std::vector<sat::IntVar> search_variables;
		AddIntervalStartVars(search_variables, layout_plans);
		AddIntervalStartVars(search_variables, cnc_plans);
		AddIntervalStartVars(search_variables, edging_plans);
		AddIntervalStartVars(search_variables, sorting_plans);
		AddIntervalStartVars(search_variables, assembly_plans);
		AddIntervalStartVars(search_variables, packing_plans);

		AddLayoutCNCPrecedence(layout_plans, cnc_plans);
		AddCNCEdgingPrecedence(cnc_plans, edging_plans);
		AddEdgingSortingPrecedence(edging_plans, sorting_plans);
		AddSortingAssemblyPrecedence(sorting_plans, assembly_plans);
		AddAssemblyPackingPrecendece(assembly_plans, packing_plans);

		if (reschedule) {
			PrioritizeByDeadline(packing_plans, orders);
		}
		
		LOG(INFO) << "\nSCHEDULING";
		LOG(INFO) << "  Component num:     " << components.size();
		LOG(INFO) << "  Comp/h:            " << Settings.AvgNumOfComponentsPerHour;
		LOG(INFO) << "  Estimated hours:   " << estimated_work_hours << " h";
		LOG(INFO) << "  Shift number:      " << ShiftNumber;
		LOG(INFO) << "  Shift length:      " << ShiftLengthHours << " h";
		LOG(INFO) << "  Planned days:      " << planned_days;
		LOG(INFO) << "  Domain:            [" << schedule_domain.Min() << " " << schedule_domain.Max() << "]";

		AddObjective(packing_plans, schedule_domain);
		auto result = Search(search_variables);

		if (result.status() == sat::CpSolverStatus::OPTIMAL || result.status() == sat::CpSolverStatus::FEASIBLE) {
			LOG(INFO) << "\n*SUCCESS*";
			if (result.status() == sat::CpSolverStatus::OPTIMAL) {
				LOG(INFO) << "  Status:      Optimal";
			}
			else {
				LOG(INFO) << "  Status:      Feasible";
			}
			LOG(INFO) << "  Obj:         " << result.objective_value();
			LOG(INFO) << "  Total time:  " << std::setprecision(1) << (result.wall_time() / 60) << " m";
			LOG(INFO) << "  Branches:    " << result.num_branches();

			std::ofstream html = CreateHtml();
			PrintWorkstation(html, result, layout_workstations, layout_plans, borders_and_breaks, schedule_domain.Max(), reschedule);
			PrintWorkstation(html, result, cnc_workstations, cnc_plans, borders_and_breaks, schedule_domain.Max(), reschedule);
			PrintWorkstation(html, result, edging_workstations, edging_plans, borders_and_breaks, schedule_domain.Max(), reschedule);
			PrintWorkstation(html, result, sorting_workstations, sorting_plans, borders_and_breaks, schedule_domain.Max(), reschedule);
			PrintWorkstation(html, result, assembly_workstations, assembly_plans, borders_and_breaks, schedule_domain.Max(), reschedule);
			PrintWorkstation(html, result, packing_workstations, packing_plans, borders_and_breaks, schedule_domain.Max(), reschedule);
			CloseHtml(html);

			std::ofstream json_handle;
			std::string json_file_name = OutputDir + "/result.json";
			json_handle.open(json_file_name);

			LOG(INFO) << "  Generating json output to: " << json_file_name;

			json output = {
				{ "layoutPlans", PlanTypeToJson(result, layout_plans) },
				{ "cncPlans", PlanTypeToJson(result, cnc_plans) },
				{ "edgingPlans", PlanTypeToJson(result, edging_plans) },
				{ "sortingPlans", PlanTypeToJson(result, sorting_plans) },
				{ "assemblyPlans", PlanTypeToJson(result, assembly_plans) },
				{ "packingPlans", PlanTypeToJson(result, packing_plans) },
			};

			json_handle << output;
			json_handle.close();
		}
		else if (result.status() == sat::CpSolverStatus::INFEASIBLE) {
			throw std::logic_error("Model infesible!");
		}
		else if (result.status() == sat::CpSolverStatus::MODEL_INVALID) {
			throw std::logic_error("Model invalid!");
		}
		else if (result.status() == sat::CpSolverStatus::UNKNOWN && result.wall_time() > (Settings.MaxTimeInSeconds - 0.2)) {
			throw std::logic_error("Solver ran out of time!");
		}
		else {
			throw std::logic_error("Unknown error!");
		}
	}

	sat::CpSolverResponse Scheduler::Search(std::vector<sat::IntVar> search_variables) {

		sat::Model M;
		sat::SatParameters MParams;

		MParams.set_max_time_in_seconds(Settings.MaxTimeInSeconds);
		MParams.set_num_search_workers(Settings.NumSearchWorkers);
		MParams.set_log_search_progress(Settings.LogSearchProgress);
		MParams.set_search_branching(sat::SatParameters::FIXED_SEARCH);

		M.Add(sat::NewSatParameters(MParams));
		
		int solution_num = 1;

		M.Add(sat::NewFeasibleSolutionObserver(
			[&](const sat::CpSolverResponse& r) {
				char* time_str;
				LOG(INFO) << "   #" << solution_num << "  Obj: " << r.objective_value() << "  Time: " << std::setprecision(1) << (r.wall_time() / 60) << " m";
				solution_num++;
			}
		));

		MODEL.AddDecisionStrategy(search_variables, sat::DecisionStrategyProto::CHOOSE_FIRST, sat::DecisionStrategyProto::SELECT_MIN_VALUE);

		LOG(INFO) << "\nSTARTING search...";
		sat::CpSolverResponse result = sat::SolveCpModel(MODEL.Build(), &M);
		LOG(INFO) << "Search FINISHED!";

		return result;
	}

	std::vector<Plan> Scheduler::CreateLunchBreaks(int planned_days) {

		std::vector<Plan> lunch_breaks;

		for (int i = 0; i < planned_days; i++) {

			for (int s = 0; s < ShiftNumber; s++) {

				int break_start = i + (i * ShiftNumber * ShiftLengthHours * 60) + (s * ShiftLengthHours * 60) + (Settings.HoursTillBreak * 60);

				Plan lunch_break(-1);
				lunch_break.ProdType = ProductionType::BREAK;
				lunch_break.Name = "Lunch break d" + std::to_string(i+1) + " s" + std::to_string(s+1);

				sat::IntVar start = MODEL.NewConstant(break_start);
				sat::IntVar size = MODEL.NewConstant(Settings.LunchDurationMinutes);
				sat::IntVar end = MODEL.NewConstant(break_start + Settings.LunchDurationMinutes);

				sat::IntervalVar schedule = MODEL.NewIntervalVar(start, size, end);

				lunch_break.SetSchedule(schedule);
				lunch_breaks.push_back(lunch_break);
			}
		}

		return lunch_breaks;
	}

	std::vector<Plan> Scheduler::CreateDayBorders(int planned_days) {

		int day_length = 60 * ShiftNumber * ShiftLengthHours;

		std::vector<Plan> borders;

		for (int i = 1; i < planned_days; i++) {

			Plan border(-1);
			border.ProdType = ProductionType::BORDER;
			border.Name = "Border " + std::to_string(i);

			sat::IntVar start = MODEL.NewConstant(day_length * i + (i - 1));
			sat::IntVar size = MODEL.NewConstant(1);
			sat::IntVar end = MODEL.NewConstant(day_length * i + i);

			sat::IntervalVar schedule = MODEL.NewIntervalVar(start, size, end);

			border.SetSchedule(schedule);
			borders.push_back(border);
		}

		return borders;
	}

	std::vector<LayoutPlan> Scheduler::CreateLayoutPlans(static Domain schedule_domain, std::string cuttings_data_file, std::vector<LayoutWorkstation> workstations, bool verbose) {

		LOG(INFO) << "\nCreating LayoutPlans...";

		LOG(INFO) << "  Reading from file: " << cuttings_data_file;
		std::ifstream cuttings_data_file_handler;
		cuttings_data_file_handler.open(cuttings_data_file);
		static json layout_plan_array;
		cuttings_data_file_handler >> layout_plan_array;

		std::vector<LayoutPlan> layout_plans;

		for (auto& layout_plan_data : layout_plan_array["layoutPlans"].items()) {

			std::vector<sat::BoolVar> presence_vars;

			for (auto& workstation : workstations) {

				std::vector<int> component_ids = layout_plan_data.value()["componentIds"];
				std::vector<std::string> order_guids = layout_plan_data.value()["orderGuids"];
				std::string board_guid = layout_plan_data.value()["boardGuid"];

				std::vector<Cutting> cuttings;
				for (auto& c_data : layout_plan_data.value()["cuttings"].items()) {
					int comp_id = c_data.value()["componentId"];
					int tlx = c_data.value()["topLeftX"];
					int tly = c_data.value()["topLeftY"];
					int w = c_data.value()["width"];
					int l = c_data.value()["length"];
					Cutting c(comp_id, tlx, tly, w, l);
					cuttings.push_back(c);
				}
 
				LayoutPlan new_layout_plan(workstation.GetId(), component_ids, order_guids, board_guid, cuttings);

				int exec_time = new_layout_plan.EstimateTime(workstation);
					
				sat::IntVar start = MODEL.NewIntVar(schedule_domain);
				sat::IntVar size = MODEL.NewConstant(exec_time);
				sat::IntVar end = MODEL.NewIntVar(schedule_domain);
				sat::BoolVar presence = MODEL.NewBoolVar();
					
				sat::IntervalVar schedule = MODEL.NewOptionalIntervalVar(start, size, end, presence);

				new_layout_plan.SetSchedule(schedule);

				layout_plans.push_back(new_layout_plan);
				presence_vars.push_back(presence);

				if (verbose) {
					LOG(INFO) << new_layout_plan;
				}
			}

			sat::LinearExpr presence_sum = sat::LinearExpr::BooleanSum(presence_vars);
			MODEL.AddEquality(presence_sum, 1);
		}

		LOG(INFO) << "  Created " << layout_plans.size() << " LayoutPlans!";

		return layout_plans;
	}

	std::vector<CNCPlan> Scheduler::CreateCNCPlans(static Domain schedule_domain, std::vector<Component> components, std::vector<CNCWorkstation> workstations, bool verbose) {

		LOG(INFO) << "\nCreating CNCPlans...";

		std::vector<CNCPlan> cnc_plans;

		for (auto& component : components) {
			
			if (component.CfcProductionStatus < CFC_CNC_COMPLETED) {
				
				std::vector<sat::BoolVar> presence_vars;

				for (auto& workstation : workstations) {

					CNCPlan new_cnc_plan(workstation.GetId(), component.Id, component.OrderGuid, component.CncDistance, component.CncHoles);

					int exec_time = new_cnc_plan.EstimateTime(workstation, Settings.CncDistanceMultiplier);

					sat::IntVar start = MODEL.NewIntVar(schedule_domain);
					sat::IntVar size = MODEL.NewConstant(exec_time);
					sat::IntVar end = MODEL.NewIntVar(schedule_domain);
					sat::BoolVar presence = MODEL.NewBoolVar();

					sat::IntervalVar schedule = MODEL.NewOptionalIntervalVar(start, size, end, presence);

					new_cnc_plan.SetSchedule(schedule);

					cnc_plans.push_back(new_cnc_plan);
					presence_vars.push_back(presence);

					if (verbose) {
						LOG(INFO) << new_cnc_plan;
					}
				}

				sat::LinearExpr presence_sum = sat::LinearExpr::BooleanSum(presence_vars);
				MODEL.AddEquality(presence_sum, 1);
			}
		}

		LOG(INFO) << "  Created " << cnc_plans.size() << " CNCPlans!";

		return cnc_plans;
	}

	std::vector<EdgingPlan> Scheduler::CreateEdgingPlans(static Domain schedule_domain, std::vector<Component> components, std::vector<EdgingWorkstation> workstations, bool verbose) {

		LOG(INFO) << "\nCreating EdgingPlans...";

		std::vector<EdgingPlan> edging_plans;

		for (auto& component : components) {

			if (component.CfcProductionStatus < CFC_EDGING_COMPLETED) {

				std::vector<sat::BoolVar> presence_vars;

				for (auto& workstation : workstations) {

					EdgingPlan new_edging_plan(workstation.GetId(), component.Id, component.OrderGuid, component.FoilNum);

					int exec_time = new_edging_plan.EstimateTime(workstation);

					sat::IntVar start = MODEL.NewIntVar(schedule_domain);
					sat::IntVar size = MODEL.NewConstant(exec_time);
					sat::IntVar end = MODEL.NewIntVar(schedule_domain);
					sat::BoolVar presence = MODEL.NewBoolVar();

					sat::IntervalVar schedule = MODEL.NewOptionalIntervalVar(start, size, end, presence);

					new_edging_plan.SetSchedule(schedule);

					edging_plans.push_back(new_edging_plan);
					presence_vars.push_back(presence);

					if (verbose) {
						LOG(INFO) << new_edging_plan;
					}
				}

				sat::LinearExpr presence_sum = sat::LinearExpr::BooleanSum(presence_vars);
				MODEL.AddEquality(presence_sum, 1);
			}
		}

		LOG(INFO) << "  Created " << edging_plans.size() << " EdgingPlans!";

		return edging_plans;
	}

	std::vector<SortingPlan> Scheduler::CreateSortingPlans(static Domain schedule_domain, std::vector<Unit> units, std::vector<SortingWorkstation> workstations, bool verbose) {

		LOG(INFO) << "\nCreating SoringPlans...";

		std::vector<SortingPlan> sorting_plans;

		for (auto& unit : units) {

			if (unit.CfuProductionStatus < CFU_SORTING_COMPLETED) {

				std::vector<sat::BoolVar> presence_vars;

				for (auto& workstation : workstations) {

					SortingPlan new_sorting_plan(workstation.GetId(), unit.OrderGuid, unit.Id, unit.ComponentIds);

					int exec_time = new_sorting_plan.EstimateTime(workstation);

					sat::IntVar start = MODEL.NewIntVar(schedule_domain);
					sat::IntVar size = MODEL.NewConstant(exec_time);
					sat::IntVar end = MODEL.NewIntVar(schedule_domain);
					sat::BoolVar presence = MODEL.NewBoolVar();

					sat::IntervalVar schedule = MODEL.NewOptionalIntervalVar(start, size, end, presence);

					new_sorting_plan.SetSchedule(schedule);

					sorting_plans.push_back(new_sorting_plan);
					presence_vars.push_back(presence);

					if (verbose) {
						LOG(INFO) << new_sorting_plan;
					}
				}

				sat::LinearExpr presence_sum = sat::LinearExpr::BooleanSum(presence_vars);
				MODEL.AddEquality(presence_sum, 1);
			}
		}

		LOG(INFO) << "  Created " << sorting_plans.size() << " SortingPlans!";

		return sorting_plans;
	}

	std::vector<AssemblyPlan> Scheduler::CreateAssemblyPlans(static Domain schedule_domain, std::vector<Unit> units, std::vector<AssemblyWorkstation> workstations, bool verbose) {

		LOG(INFO) << "\nCreating AssemblyPlans...";

		std::vector<AssemblyPlan> assembly_plans;

		for (auto& unit : units) {

			if (unit.CfuProductionStatus < CFU_ASSEMBLY_COMPLETED) {

				std::vector<sat::BoolVar> presence_vars;

				for (auto& workstation : workstations) {

					AssemblyPlan new_assembly_plan(workstation.GetId(), unit.OrderGuid, unit.Id, unit.ComponentIds, unit.Accessories);

					int exec_time = new_assembly_plan.EstimateTime(workstation);

					sat::IntVar start = MODEL.NewIntVar(schedule_domain);
					sat::IntVar size = MODEL.NewConstant(exec_time);
					sat::IntVar end = MODEL.NewIntVar(schedule_domain);
					sat::BoolVar presence = MODEL.NewBoolVar();

					sat::IntervalVar schedule = MODEL.NewOptionalIntervalVar(start, size, end, presence);

					new_assembly_plan.SetSchedule(schedule);

					assembly_plans.push_back(new_assembly_plan);
					presence_vars.push_back(presence);

					if (verbose) {
						LOG(INFO) << new_assembly_plan;
					}
				}

				sat::LinearExpr presence_sum = sat::LinearExpr::BooleanSum(presence_vars);
				MODEL.AddEquality(presence_sum, 1);
			}
		}

		LOG(INFO) << "  Created " << assembly_plans.size() << " AssemblyPlans!";

		return assembly_plans;
	}

	std::vector<PackingPlan> Scheduler::CreatePackingPlans(static Domain schedule_domain, std::vector<Unit> units, std::vector<PackingWorkstation> workstations, bool verbose) {

		LOG(INFO) << "\nCreating PackingPlans...";

		std::vector<PackingPlan> packing_plans;

		for (auto& unit : units) {

			if (unit.CfuProductionStatus < CFU_PACKING_COMPLETED) {

				std::vector<sat::BoolVar> presence_vars;

				for (auto& workstation : workstations) {

					PackingPlan new_packing_plan(workstation.GetId(), unit.OrderGuid, unit.Id, unit.ComponentIds);

					int exec_time = new_packing_plan.EstimateTime(workstation);

					sat::IntVar start = MODEL.NewIntVar(schedule_domain);
					sat::IntVar size = MODEL.NewConstant(exec_time);
					sat::IntVar end = MODEL.NewIntVar(schedule_domain);
					sat::BoolVar presence = MODEL.NewBoolVar();

					sat::IntervalVar schedule = MODEL.NewOptionalIntervalVar(start, size, end, presence);

					new_packing_plan.SetSchedule(schedule);

					packing_plans.push_back(new_packing_plan);
					presence_vars.push_back(presence);

					if (verbose) {
						LOG(INFO) << new_packing_plan;
					}
				}

				sat::LinearExpr presence_sum = sat::LinearExpr::BooleanSum(presence_vars);
				MODEL.AddEquality(presence_sum, 1);
			}
		}

		LOG(INFO) << "  Created " << packing_plans.size() << " PackingPlans!";

		return packing_plans;
	}
	
	void Scheduler::AddLayoutCNCPrecedence(std::vector<LayoutPlan> layout_plans, std::vector<CNCPlan> cnc_plans) {

		for (auto& layout_p : layout_plans) {

			for (int c_id : layout_p.GetComponentIds()) {

				for (auto& cnc_p : cnc_plans) {

					if (c_id == cnc_p.GetComponentId()) {
						MODEL.AddLessOrEqual(layout_p.GetSchedule().EndVar(), cnc_p.GetSchedule().StartVar());
					}
				}
			}
		}
	}

	void Scheduler::AddCNCEdgingPrecedence(std::vector<CNCPlan> cnc_plans, std::vector<EdgingPlan> edging_plans) {

		for (auto& cnc_p : cnc_plans) {

			for (auto& edging_p : edging_plans) {

				if (cnc_p.GetComponentId() == edging_p.GetComponentId()) {
					MODEL.AddLessOrEqual(cnc_p.GetSchedule().EndVar(), edging_p.GetSchedule().StartVar());
				}
			}
		}
	}

	void Scheduler::AddEdgingSortingPrecedence(std::vector<EdgingPlan> edging_plans, std::vector<SortingPlan> sorting_plans) {

		for (auto& sorting_p : sorting_plans) {

			for (int c_id : sorting_p.GetComponentIds()) {

				for (auto& edging_p : edging_plans) {

					if (c_id == edging_p.GetComponentId()) {
						MODEL.AddLessOrEqual(edging_p.GetSchedule().EndVar(), sorting_p.GetSchedule().StartVar());
					}
				}
			}
		}
	}

	void Scheduler::AddSortingAssemblyPrecedence(std::vector<SortingPlan> sorting_plans, std::vector<AssemblyPlan> assembly_plans) {

		for (auto& sorting_p : sorting_plans) {

			for (auto& assembly_p : assembly_plans) {

				if (assembly_p.GetUnitId() == sorting_p.GetUnitId()) {
					MODEL.AddLessOrEqual(sorting_p.GetSchedule().EndVar(), assembly_p.GetSchedule().StartVar());
				}
			}
		}
	}

	void Scheduler::AddAssemblyPackingPrecendece(std::vector<AssemblyPlan> assembly_plans, std::vector<PackingPlan> packing_plans) {

		for (auto& assembly_p : assembly_plans) {

			for (auto& packing_p : packing_plans) {

				if (assembly_p.GetUnitId() == packing_p.GetUnitId()) {
					MODEL.AddLessOrEqual(assembly_p.GetSchedule().EndVar(), packing_p.GetSchedule().StartVar());
				}
			}
		}
	}

	template <typename P>
	std::vector<json> Scheduler::PlanTypeToJson(sat::CpSolverResponse result, std::vector<P> plans) {

		std::vector<json> json_plans;

		for (auto& p : plans) {
			if (sat::SolutionBooleanValue(result, p.GetSchedule().PresenceBoolVar())) {
				json_plans.push_back(p.ToJson(result, Settings.WorkStartHour, ShiftNumber, ShiftLengthHours));
			}
		}

		return json_plans;
	}

	template <typename P>
	void Scheduler::AddObjective(std::vector<P> plans, Domain schedule_domain) {

		std::vector<sat::IntVar> ends;
		for (auto& p : plans) {
			ends.push_back(p.GetSchedule().EndVar());
		}
		sat::IntVar objective = MODEL.NewIntVar(schedule_domain);
		MODEL.AddMaxEquality(objective, ends);
		MODEL.Minimize(objective);
	}

	template <typename Ws, typename P>
	void Scheduler::AddWorkstationNoOverlap(std::vector<Ws> workstations, std::vector<P> plans, bool reschedule) {

		for (auto& ws : workstations) {

			std::vector<sat::IntervalVar> no_overlap_intervals;
			for (auto& p : plans) {

				if (p.GetWorkStationId() == ws.GetId()) {

					no_overlap_intervals.push_back(p.GetSchedule());
				}
			}

			int ws_occupied_minutes = CalculateOccupiedMinutes(reschedule, ws.GetLastPlanEnd());
			if (ws_occupied_minutes > 0) {

				sat::IntVar placeholder_start = MODEL.NewConstant(0);
				sat::IntVar placeholder_size = MODEL.NewConstant(ws_occupied_minutes);
				sat::IntVar placeholder_end = MODEL.NewConstant(ws_occupied_minutes);

				sat::IntervalVar placeholder = MODEL.NewIntervalVar(placeholder_start, placeholder_size, placeholder_end);

				no_overlap_intervals.push_back(placeholder);
			}

			MODEL.AddNoOverlap(no_overlap_intervals);
		}
	}

	template <typename E, typename P>
	void Scheduler::AddPairwiseNoOverlap(std::vector<E> excluded_times, std::vector<P> other_plans) {

		for (auto& excluded : excluded_times) {

			for (auto& plan : other_plans) {

				MODEL.AddNoOverlap({ excluded.GetSchedule(), plan.GetSchedule() });
			}
		}
	}

	template <typename P>
	void Scheduler::AddIntervalStartVars(std::vector<sat::IntVar>& search_variables, std::vector<P> plans) {

		for (auto& p : plans) {
			search_variables.push_back(p.GetSchedule().StartVar());
		}
	}

	template <typename Ws, typename P>
	void Scheduler::PrintWorkstation(std::ofstream& html, sat::CpSolverResponse result, std::vector<Ws> workstations, std::vector<P> plans, std::vector<Plan> borders_and_breaks, int width, bool reschedule) {

		int minute_pixel_rate = 6;

		for (auto& ws : workstations) {

			html << "<h4>" + ws.GetName() + "</h4>";
			html << "<div class='workstation ws-" + std::to_string(ws.GetProductionType()) + "' style='width:" + std::to_string(width*minute_pixel_rate) + "px;'>";

			int ws_occupied_minutes = CalculateOccupiedMinutes(reschedule, ws.GetLastPlanEnd());
			if (ws_occupied_minutes > 0) {
				html << "<div class='placeholder' style='left:0px; width:" + std::to_string(ws_occupied_minutes * minute_pixel_rate) + "px;'><p>Occupied</p></div>";
			}

			int q_count = 0;
			int h = Settings.WorkStartHour - 1;
			int day_offset = 0;
			int day_elapsed_hours = -1;
			for (int i = 0; i < width; i++) {
				if (i % 60 == 0) {
					h++;
					day_elapsed_hours++;
					if (day_elapsed_hours % ShiftLengthHours == 0) {
						html << "<div class='m-60 shift-border' style='width:" + std::to_string(minute_pixel_rate - 1) + "px; left:" + std::to_string((i + day_offset) * minute_pixel_rate) + "px;'><p>" + std::to_string(h) + ":00</p></div>";
					} 
					else{
						html << "<div class='m-60' style='width:" + std::to_string(minute_pixel_rate - 1) + "px; left:" + std::to_string((i + day_offset) * minute_pixel_rate) + "px;'><p>" + std::to_string(h) + ":00</p></div>";
					}
					
					if (h > Settings.WorkStartHour - 1 + ShiftLengthHours * ShiftNumber) {
						h = Settings.WorkStartHour;
						day_offset++;
						day_elapsed_hours = 0;
					}
				}
				else if (i % 15 == 0) {
					q_count++;
					if (q_count == 1) {
						html << "<div class='m-quarter' style='width:" + std::to_string(minute_pixel_rate - 1) + "px; left:" + std::to_string((i + day_offset) * minute_pixel_rate) + "px;'><p>" + std::to_string(h) + ":15</p></div>";
					}
					else if (q_count == 2) {
						html << "<div class='m-quarter' style='width:" + std::to_string(minute_pixel_rate - 1) + "px; left:" + std::to_string((i + day_offset) * minute_pixel_rate) + "px;'><p>" + std::to_string(h) + ":30</p></div>";
					}
					else if (q_count == 3) {
						html << "<div class='m-quarter' style='width:" + std::to_string(minute_pixel_rate - 1) + "px; left:" + std::to_string((i + day_offset) * minute_pixel_rate) + "px;'><p>" + std::to_string(h) + ":45</p></div>";
						q_count = 0;
					}
				}
				else {
					html << "<div class='minute' style='width:" + std::to_string(minute_pixel_rate - 1) + "px; left:" + std::to_string(i * minute_pixel_rate) + "px;'></div>";
				}
			}

			for (auto& p : plans) {
				
				if (p.GetWorkStationId() == ws.GetId()) {

					if (sat::SolutionBooleanValue(result, p.GetSchedule().PresenceBoolVar())) {

						html << p.PrintHtml(result, minute_pixel_rate);
					}
				}
			}

			for (auto& bb : borders_and_breaks) {
			
				html << bb.PrintHtml(result, minute_pixel_rate);
			}

			html << "</div>";
		}
	}

	void Scheduler::PrioritizeByDeadline(std::vector<PackingPlan> packing_plans, std::vector<Order> orders) {

		std::sort(orders.begin(), orders.end());

		for (size_t i = 0; i < orders.size(); i++) {
			
			for (size_t j = (i+1); j < orders.size(); j++) {
				
				for (auto& i_plan : packing_plans) {

					if (i_plan.GetOrderId() == orders[i].Guid) {
						
						for (auto& j_plan : packing_plans) {

							if (j_plan.GetOrderId() == orders[j].Guid) {
								MODEL.AddLessOrEqual(i_plan.GetSchedule().EndVar(), j_plan.GetSchedule().StartVar());
							}
						}
					}
				}
			}
		}
	}

	int Scheduler::CalculateOccupiedMinutes(bool reschedule, BRDateTime wsLastPlanEnd) {

		int occupied_mins = 0;

		if (reschedule) {
			// occupy workstation until now + reschedule delay from today start
			BRDateTime br_now = BRDateTime(Settings);
			occupied_mins = CalculateMinutesFromTodayStart(TodayStart, br_now, ShiftNumber, ShiftLengthHours) + Settings.RescheduleDelayMinutes;
		}
		else {
			// add placeholder plan to exclude already occupied intervals from prevoius schedules based on workstations last plan end
			occupied_mins = CalculateMinutesFromTodayStart(TodayStart, wsLastPlanEnd, ShiftNumber, ShiftLengthHours);
		}

		return occupied_mins;
	}

	Domain Scheduler::CalculateDomain(int planned_days) {

		// in minutes
		int horizon = planned_days * 60 * ShiftNumber * ShiftLengthHours + (planned_days - 1); 
		
		return Domain(0, horizon);
	}

	std::ofstream Scheduler::CreateHtml() {

		std::ofstream html;
		std::string html_file_name = HtmlDir + "/schedule.html";
		html.open(html_file_name);

		LOG(INFO) << "\n  Generating html output to: " << html_file_name;

		html << "<!DOCTYPE html><html><head><title>Job Schedule</title><link rel='stylesheet' type='text/css' href='style.css'></head><body>";
		html << "<div class='container'>";
		html << "<h2>Job Schedule</h2>";
		html << "<p>schedule_on" + GetDateTimeAsString() + ".html</p>";

		return html;
	}

	void Scheduler::CloseHtml(std::ofstream& html) {

		html << "</div>";
		html << "</body></html>";
		html.close();
	}

	std::string Scheduler::GetDateTimeAsString() {

		std::stringstream ss;
		struct tm buf;
		auto now = std::chrono::system_clock::now();
		std::time_t now_time = std::chrono::system_clock::to_time_t(now);
		localtime_s(&buf, &now_time);

		auto time_obj = std::put_time(&buf, "_%Y-%m-%d_%H-%M-%S");
		ss << time_obj;

		std::string time_str = ss.str();

		return time_str;
	}

	std::string Scheduler::GetOrdersAsString(json_array orders) {

		std::string orders_string = "O";

		for (auto& o : orders) {
			std::string guid = o.value()["guid"];
			orders_string += "-" + guid.substr(0,4);
		}

		return orders_string;
	}
}
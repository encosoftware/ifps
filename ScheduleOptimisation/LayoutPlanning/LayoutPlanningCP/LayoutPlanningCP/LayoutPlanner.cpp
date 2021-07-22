#include "LayoutPlanner.hpp"

namespace operations_research {

	LayoutPlanner::LayoutPlanner(std::string search_settings_file, std::string output_dir, std::string html_dir, ComponentSortingStrategy strategy) {

		std::ifstream search_settings_file_handle;
		search_settings_file_handle.open(search_settings_file);
		static json SETTINGS;
		search_settings_file_handle >> SETTINGS;

		LeftoverLengthMultiplier = SETTINGS["searchSettings"]["leftoverLengthMultiplier"];
		LeftoverStep = SETTINGS["searchSettings"]["leftoverStep"];
		LeftoverThreshold = SETTINGS["searchSettings"]["leftoverThreshold"];
		MaxTimeInSeconds = SETTINGS["searchSettings"]["maxTimeInSeconds"];
		AreaMultiplier = SETTINGS["searchSettings"]["areaMultiplier"];
		ParallelWorkerNum = SETTINGS["searchSettings"]["parallelWorkerNum"];
		DetailedSolverLogs = SETTINGS["searchSettings"]["detailedSolverLogs"];
		LogDir = SETTINGS["searchSettings"]["logDir"].get<std::string>();
		OutputDir = output_dir;
		HtmlDir = html_dir;
		
		ComponentSorting = strategy;
		CoordinateUse = CoordinateUseStrategy::X_START;
		VariableOrdering = sat::DecisionStrategyProto::CHOOSE_FIRST;
		ValueOrdering = sat::DecisionStrategyProto::SELECT_MIN_VALUE;
	}

	void LayoutPlanner::Print() {

		LOG(INFO) << "\nLayout planner settings:" << std::endl;
		LOG(INFO) << "   LeftoverLengthMultiplier: " << LeftoverLengthMultiplier  << std::endl;
		LOG(INFO) << "   LeftoverStep:             " << LeftoverStep << std::endl;
		LOG(INFO) << "   LeftoverThreshold:        " << LeftoverThreshold << std::endl;
		LOG(INFO) << "   MaxTimeInSeconds:         " << MaxTimeInSeconds << std::endl;
		LOG(INFO) << "   AreaMultiplier:           " << AreaMultiplier << std::endl;
		LOG(INFO) << "   ParallelWorkerNum:        " << ParallelWorkerNum << std::endl;
		LOG(INFO) << "   DetailedSolverLogs:       " << DetailedSolverLogs << std::endl;
		LOG(INFO) << "   Component sorting:        " << CSSMap(ComponentSorting);
		LOG(INFO) << "   Coordinate use:           " << CUSMap(CoordinateUse);
		LOG(INFO) << "   Output directory:         " << OutputDir << std::endl;
	}

	int LayoutPlanner::InitSearch(std::string data_file_name) {

		LOG(INFO) << "\nPROCESSED FILE: " << data_file_name << std::endl;
		
		// load data from json
		try {
			std::ifstream data_file;
			data_file.open(data_file_name);
			static json DATA;
			data_file >> DATA;

			try {
				std::vector<json> layout_jsons;

				// board loop
				for (auto& board : DATA["boards"].items()) {
					ProcessBoard(DATA["concreteFurnitureComponents"].items(), board, layout_jsons);
				}

				json layout_plans = {
					{ "layoutPlans", layout_jsons }
				};

				std::ofstream o;
				std::string result_file_name = OutputDir + "/cuttings.json";
				o.open(result_file_name);
				o << layout_plans << std::endl;
				o.close();

				LOG(INFO) << "\nResult saved to: " << result_file_name << std::endl << std::endl;

				return 1;
			}
			catch (const std::exception &e) {
				LOG(ERROR) << e.what() << std::endl;
				return 2;
			}
			catch (...) {
				return 3;
			}
		}
		catch (json::parse_error& e) {
			LOG(ERROR) << "Invalid JSON file or filepath!" << std::endl;
			LOG(ERROR) << e.what() << std::endl;
			return 4;
		}
	}

	void LayoutPlanner::ProcessBoard(JsonArray component_array, JsonData board_data, std::vector<json>& layout_jsons) {

		std::string boardGuid = board_data.value()["guid"];
		LOG(INFO) << "\n-------------------\nProcessing board with Guid: " << boardGuid << std::endl;

		int w_board = (int)board_data.value()["width"];
		int l_board = (int)board_data.value()["length"];
		if (w_board <= 0 || l_board <= 0) {
			throw std::domain_error("Board width or length is zero!");
		}
		LOG(INFO) << "   One board area:         " << (float)(w_board * l_board) / 1000000 << " m2" << std::endl;

		int sum_component_area = CalculateSumComponentArea(component_array, board_data.value()["guid"]);
		LOG(INFO) << "   Sum component area:     " << (float)sum_component_area / 1000000 << " m2" << std::endl;

		int board_num = sum_component_area / (w_board * l_board) + 1;
		LOG(INFO) << "   Base board num:         " << board_num << std::endl;

		int leftover_area = board_num * w_board * l_board - sum_component_area;
		int leftover_length;

		if (leftover_area < (w_board * l_board * AreaMultiplier)) {
			board_num++;
			leftover_area += w_board * l_board;
			leftover_length = w_board * LeftoverLengthMultiplier;
			LOG(INFO) << "   Free space under threshold, +1 board." << std::endl;
		}
		else {
			leftover_length = (leftover_area / l_board + 1) * LeftoverLengthMultiplier;
			
		}

		// round leftover lenght to hundreds
		leftover_length = std::round(leftover_length / 100) * 100;

		LOG(INFO) << "   Final board num:        " << board_num << std::endl;
		LOG(INFO) << "   Leftover area:          " << (float)leftover_area / 1000000 << " m2" << std::endl;

		int iter_num = 1;
		while (true) {

			LOG(INFO) << "\n[" + std::to_string(iter_num) + "]" << std::endl;

			if (leftover_length < 0) {
				LOG(INFO) << "LO length under 0, adding one more board" << std::endl;
				board_num++;
				LOG(INFO) << "New board num: " << board_num << std::endl;
				leftover_length = l_board * LeftoverLengthMultiplier;
			}
			else if (leftover_length < LeftoverThreshold) {
				LOG(INFO) << "LO length under threshold, set to 0" << std::endl;
				leftover_length = 0;
			}
			LOG(INFO) << "LO length : " << leftover_length << std::endl;

			int status = 0;
			bool is_rotatable = !(bool)board_data.value()["hasFiberDirection"]; // if has fiber direction it is not rotatable

			status = RunCPIteration(component_array, board_data, leftover_length, board_num, is_rotatable, layout_jsons);

			if (status == 1) {
				LOG(INFO) << "SUCCESS: Found valid solution." << std::endl;
				break;
			}
			else if (status == 2) {
				LOG(INFO) << "FAILURE: Problem infeasible, decreasing leftover area" << std::endl;
				leftover_length = leftover_length - LeftoverStep;
			}
			else if (status == 3) {
				LOG(INFO) << "FAILURE: Ran out of time, decreasing leftover area" << std::endl << std::endl;
				leftover_length = leftover_length - LeftoverStep;
			}
			else {
				LOG(INFO) << "FAILURE: unknown, status: " + std::to_string(status) + "\nexiting..." << std::endl;
				break;
			}

			iter_num++;
		}
	}

	template <typename T>
	void LayoutPlanner::GenerateHtmlLayout(const sat::CpSolverResponse result, std::vector<T> components, std::vector<Rectangle> borders, Rectangle leftover,
		JsonData board_data, int board_num, bool with_leftover) {

		int w_board = board_data.value()["width"];
		int l_board = board_data.value()["length"];
		std::string board_guid = board_data.value()["guid"];

		std::ofstream layout_file;
		std::string layout_filename = HtmlDir + "/board_" + board_guid + "_" + GetDateTimeAsString() + ".html";
		layout_file.open(layout_filename);

		layout_file << "<!DOCTYPE html><html><head><title>Layout planning</title><link rel='stylesheet' type='text/css' href='style.css'></head><body>";
		layout_file << "<div class='layout-container' style='width:" + std::to_string(w_board * board_num + (board_num - 1)) + "px; height:" + std::to_string(l_board) + "px;'>";

		int comp_n = 1;
		for (auto& component : components) {
			component.Print(layout_file, result, 1, comp_n, component.OrderGuid);
			comp_n++;
		}

		for (auto& border : borders) {
			border.Print(layout_file, result, 2, -1, "-");
		}

		if (with_leftover) {
			leftover.Print(layout_file, result, 3, -1, "-");
		}

		layout_file << "</div></body></html>";
		layout_file.close();
	}

	int LayoutPlanner::RunCPIteration(JsonArray component_array, JsonData board_data, int leftover_length, int board_num, bool is_rotatable, std::vector<json>& layout_jsons) {

		// SAT CP model init
		sat::CpModelBuilder BoardCPModel;
		sat::NoOverlap2DConstraint no_overlap = BoardCPModel.AddNoOverlap2D();

		std::vector<Rectangle> border_variables = AddBorderVariables(BoardCPModel, no_overlap, board_data, board_num);

		bool with_leftover = false;
		Rectangle leftover_variable;
		if (leftover_length > LeftoverThreshold) {
			leftover_variable = AddLeftoverVariable(BoardCPModel, no_overlap, board_data, board_num, leftover_length);
			with_leftover = true;
		}

		// define model and meta search parameters
		sat::Model M;
		sat::SatParameters params;

		params.set_max_time_in_seconds(MaxTimeInSeconds);
		params.set_num_search_workers(ParallelWorkerNum);
		params.set_log_search_progress(DetailedSolverLogs);
		params.set_search_branching(sat::SatParameters::FIXED_SEARCH);

		M.Add(sat::NewSatParameters(params));

		// Rotatable or default search case
		if (is_rotatable) {
			LOG(INFO) << "ROTATABLE search";
			std::vector<RotatableRectangle> rotatable_component_variables = AddRotatableComponentVariables(BoardCPModel, no_overlap, component_array, board_data, board_num);
			DefineRotatableRectangleSearchStrategy(BoardCPModel, rotatable_component_variables);

			LOG(INFO) << "Started solving.." << std::endl;
			const sat::CpSolverResponse result = sat::SolveCpModel(BoardCPModel.Build(), &M);
			LOG(INFO) << "Solver finished!" << std::endl;

			LOG(INFO) << "Wall time:  " << result.wall_time() << " s";
			LOG(INFO) << "Branches:   " << result.num_branches();

			if (result.status() == sat::CpSolverStatus::FEASIBLE) {
				GenerateHtmlLayout(result, rotatable_component_variables, border_variables, leftover_variable, board_data, board_num, with_leftover);
				SaveLayoutAsJson(result, rotatable_component_variables, board_data, board_num, layout_jsons);
				return 1;
			}
			else if (result.status() == sat::CpSolverStatus::INFEASIBLE) {
				return 2;
			}
			else if (result.status() == sat::CpSolverStatus::UNKNOWN && result.wall_time() > MaxTimeInSeconds - 0.2) {
				return 3;
			}
			else {
				return result.status();
			}
		}
		else {
			LOG(INFO) << "DEFAULT search";
			std::vector<Rectangle> component_variables = AddDefaultComponentVariables(BoardCPModel, no_overlap, component_array, board_data, board_num);
			DefineRectangleSearchStrategy(BoardCPModel, component_variables);

			LOG(INFO) << "Started solving.." << std::endl;
			const sat::CpSolverResponse result = sat::SolveCpModel(BoardCPModel.Build(), &M);
			LOG(INFO) << "Solver finished!" << std::endl;

			LOG(INFO) << "Wall time:  " << result.wall_time() << " s";
			LOG(INFO) << "Branches:   " << result.num_branches();

			if (result.status() == sat::CpSolverStatus::FEASIBLE) {
				GenerateHtmlLayout(result, component_variables, border_variables, leftover_variable, board_data, board_num, with_leftover);
				SaveLayoutAsJson(result, component_variables, board_data, board_num, layout_jsons);
				return 1;
			}
			else if (result.status() == sat::CpSolverStatus::INFEASIBLE) {
				return 2;
			}
			else if (result.status() == sat::CpSolverStatus::UNKNOWN && result.wall_time() > MaxTimeInSeconds - 0.2) {
				return 3;
			}
			else {
				return result.status();
			}
		}
	}

	int LayoutPlanner::CalculateSumComponentArea(JsonArray component_array, JsonValue board_guid) {

		int sum = 0;
		for (auto& component : component_array) {
			if (board_guid == component.value()["boardGuid"]) {
				int w_comp = (int)component.value()["width"];
				int l_comp = (int)component.value()["length"];
				int a_comp = w_comp * l_comp;
				sum += a_comp;
			}
		}
		return sum;
	}

	std::vector<Rectangle> LayoutPlanner::AddDefaultComponentVariables(sat::CpModelBuilder &CPModel, sat::NoOverlap2DConstraint &no_overlap,
		JsonArray component_array, JsonData board_data, const int board_num) {

		std::vector<Rectangle> component_variables;

		const Domain x_domain(0, (int)board_data.value()["width"] * board_num + board_num);
		const Domain y_domain(0, (int)board_data.value()["length"]);

		for (auto& component : component_array) {
			if (board_data.value()["guid"] == component.value()["boardGuid"] && component.value()["cfcProductionStatus"] < CFC_LAYOUT_COMPLETED) {

				sat::IntVar tmp_x_start = CPModel.NewIntVar(x_domain);
				sat::IntVar tmp_x_size = CPModel.NewConstant((int)component.value()["width"]);
				sat::IntVar tmp_x_end = CPModel.NewIntVar(x_domain);
				sat::IntervalVar tmp_x = CPModel.NewIntervalVar(tmp_x_start, tmp_x_size, tmp_x_end);

				sat::IntVar tmp_y_start = CPModel.NewIntVar(y_domain);
				sat::IntVar tmp_y_size = CPModel.NewConstant((int)component.value()["length"]);
				sat::IntVar tmp_y_end = CPModel.NewIntVar(y_domain);
				sat::IntervalVar tmp_y = CPModel.NewIntervalVar(tmp_y_start, tmp_y_size, tmp_y_end);

				int area = (int)component.value()["width"] * (int)component.value()["length"];

				Rectangle tmp_component(tmp_x, tmp_y, component.value()["name"], area, component.value()["orderGuid"], component.value()["id"]);
				component_variables.push_back(tmp_component);
				no_overlap.AddRectangle(tmp_component.X, tmp_component.Y);
			}
		}
		return component_variables;
	}

	std::vector<RotatableRectangle> LayoutPlanner::AddRotatableComponentVariables(sat::CpModelBuilder &CPModel, sat::NoOverlap2DConstraint &no_overlap,
		JsonArray component_array, JsonData board_data, const int board_num) {

		std::vector<RotatableRectangle> component_variables;

		const Domain x_domain(0, (int)board_data.value()["width"] * board_num + board_num);
		const Domain y_domain(0, (int)board_data.value()["length"]);

		for (auto& component : component_array) {
			if (board_data.value()["guid"] == component.value()["boardGuid"] && component.value()["cfcProductionStatus"] < CFC_LAYOUT_COMPLETED) {

				int w_comp = (int)component.value()["width"];
				int l_comp = (int)component.value()["length"];
				int area = w_comp * l_comp;

				const Domain w_domain = Domain::FromValues({ 0, w_comp });
				const Domain l_domain = Domain::FromValues({ 0, l_comp });

				// X
				sat::IntVar x_start = CPModel.NewIntVar(x_domain);
				sat::IntVar x_size = CPModel.NewIntVar(w_domain);
				sat::IntVar x_end = CPModel.NewIntVar(x_domain);
				sat::BoolVar x_presence = CPModel.NewBoolVar();
				sat::IntervalVar x = CPModel.NewOptionalIntervalVar(x_start, x_size, x_end, x_presence);

				// Y
				sat::IntVar y_start = CPModel.NewIntVar(y_domain);
				sat::IntVar y_size = CPModel.NewIntVar(l_domain);
				sat::IntVar y_end = CPModel.NewIntVar(y_domain);
				sat::BoolVar y_presence = CPModel.NewBoolVar();
				sat::IntervalVar y = CPModel.NewOptionalIntervalVar(y_start, y_size, y_end, y_presence);

				// X_rot
				sat::IntVar x_rot_start = CPModel.NewIntVar(x_domain);
				sat::IntVar x_rot_size = CPModel.NewIntVar(l_domain);
				sat::IntVar x_rot_end = CPModel.NewIntVar(x_domain);
				sat::BoolVar x_rot_presence = CPModel.NewBoolVar();
				sat::IntervalVar x_rot = CPModel.NewOptionalIntervalVar(x_rot_start, x_rot_size, x_rot_end, x_rot_presence);

				// Y_rot
				sat::IntVar y_rot_start = CPModel.NewIntVar(y_domain);
				sat::IntVar y_rot_size = CPModel.NewIntVar(w_domain);
				sat::IntVar y_rot_end = CPModel.NewIntVar(y_domain);
				sat::BoolVar y_rot_presence = CPModel.NewBoolVar();
				sat::IntervalVar y_rot = CPModel.NewOptionalIntervalVar(y_rot_start, y_rot_size, y_rot_end, y_rot_presence);

				RotatableRectangle tmp_component(x, y, x_rot, y_rot, component.value()["name"], area, component.value()["orderGuid"], component.value()["id"]);

				// enforce the default-oriented component's size variables if the presence literal is false
				CPModel.AddEquality(tmp_component.X.SizeVar(), 0).OnlyEnforceIf(Not(tmp_component.X.PresenceBoolVar()));
				CPModel.AddEquality(tmp_component.Y.SizeVar(), 0).OnlyEnforceIf(Not(tmp_component.Y.PresenceBoolVar()));
				CPModel.AddNotEqual(tmp_component.X.SizeVar(), 0).OnlyEnforceIf(tmp_component.X.PresenceBoolVar());
				CPModel.AddNotEqual(tmp_component.Y.SizeVar(), 0).OnlyEnforceIf(tmp_component.Y.PresenceBoolVar());

				// enforce the rotated component's size variables if the presence literal is false
				CPModel.AddEquality(tmp_component.X_rot.SizeVar(), 0).OnlyEnforceIf(Not(tmp_component.X_rot.PresenceBoolVar()));
				CPModel.AddEquality(tmp_component.Y_rot.SizeVar(), 0).OnlyEnforceIf(Not(tmp_component.Y_rot.PresenceBoolVar()));
				CPModel.AddNotEqual(tmp_component.X_rot.SizeVar(), 0).OnlyEnforceIf(tmp_component.X_rot.PresenceBoolVar());
				CPModel.AddNotEqual(tmp_component.Y_rot.SizeVar(), 0).OnlyEnforceIf(tmp_component.Y_rot.PresenceBoolVar());

				// internal constraints
				CPModel.AddEquality(tmp_component.X.PresenceBoolVar(), tmp_component.Y.PresenceBoolVar());
				CPModel.AddEquality(tmp_component.X_rot.PresenceBoolVar(), tmp_component.Y_rot.PresenceBoolVar());

				std::vector<sat::BoolVar> bools = {
					tmp_component.X.PresenceBoolVar(),
					tmp_component.Y.PresenceBoolVar(),
					tmp_component.X_rot.PresenceBoolVar(),
					tmp_component.Y_rot.PresenceBoolVar()
				};
				CPModel.AddEquality(2, sat::LinearExpr::BooleanSum(bools));

				no_overlap.AddRectangle(tmp_component.X, tmp_component.Y);
				no_overlap.AddRectangle(tmp_component.X_rot, tmp_component.Y_rot);

				component_variables.push_back(tmp_component);
			}
		}
		return component_variables;
	}

	std::vector<Rectangle> LayoutPlanner::AddBorderVariables(sat::CpModelBuilder &CPModel, sat::NoOverlap2DConstraint &no_overlap, JsonData board_data, const int board_num) {

		std::vector<Rectangle> borders;

		int w_board = (int)board_data.value()["width"];
		int l_board = (int)board_data.value()["length"];

		for (int i = 1; i < board_num; i++) {

			sat::IntVar border_x_start = CPModel.NewConstant(w_board * i + (i - 1));
			sat::IntVar border_x_size = CPModel.NewConstant(1);
			sat::IntVar border_x_end = CPModel.NewConstant(w_board * i + i);
			sat::IntervalVar border_x = CPModel.NewIntervalVar(border_x_start, border_x_size, border_x_end);

			sat::IntVar border_y_start = CPModel.NewConstant(0);
			sat::IntVar border_y_size = CPModel.NewConstant(l_board);
			sat::IntVar border_y_end = CPModel.NewConstant(l_board);
			sat::IntervalVar border_y = CPModel.NewIntervalVar(border_y_start, border_y_size, border_y_end);

			std::string border_name = "Border" + std::to_string(i);

			Rectangle border(border_x, border_y, border_name, -1, "-", -1);
			borders.push_back(border);
			no_overlap.AddRectangle(border.X, border.Y);
		}
		return borders;
	}

	Rectangle LayoutPlanner::AddLeftoverVariable(sat::CpModelBuilder &CPModel, sat::NoOverlap2DConstraint &no_overlap, JsonData board_data, const int board_num, const int leftover_length) {

		int w_board = (int)board_data.value()["width"];
		int l_board = (int)board_data.value()["length"];

		sat::IntVar leftover_x_start = CPModel.NewConstant((board_num) * w_board + (board_num - 1) - leftover_length);
		sat::IntVar leftover_x_size = CPModel.NewConstant(leftover_length);
		sat::IntVar leftover_x_end = CPModel.NewConstant(board_num * w_board + (board_num - 1));
		sat::IntervalVar leftover_x = CPModel.NewIntervalVar(leftover_x_start, leftover_x_size, leftover_x_end);

		sat::IntVar leftover_y_start = CPModel.NewConstant(0);
		sat::IntVar leftover_y_size = CPModel.NewConstant(l_board);
		sat::IntVar leftover_y_end = CPModel.NewConstant(l_board);
		sat::IntervalVar leftover_y = CPModel.NewIntervalVar(leftover_y_start, leftover_y_size, leftover_y_end);

		Rectangle leftover(leftover_x, leftover_y, "Left-over", -1, "-", -1);
		no_overlap.AddRectangle(leftover.X, leftover.Y);

		return leftover;
	}

	void LayoutPlanner::DefineRectangleSearchStrategy(sat::CpModelBuilder &CPModel, std::vector<Rectangle> &components) {

		switch (ComponentSorting)
		{
		case ComponentSortingStrategy::BY_AREA_DESC:
			std::sort(components.begin(), components.end(), Rectangle::DescArea);
			break;
		case ComponentSortingStrategy::BY_ORDER_ASC:
			std::sort(components.begin(), components.end(), Rectangle::AscOrder);
			break;
		default:
			LOG(ERROR) << "Invalid component sorting case in search strategy!";
		}

		std::vector<sat::IntVar> search_vars;
		for (auto& component : components) {
			switch (CoordinateUse)
			{
			case CoordinateUseStrategy::X_START:
				search_vars.push_back(component.X.StartVar());
				break;
			case CoordinateUseStrategy::Y_START:
				search_vars.push_back(component.Y.StartVar());
				break;
			default:
				LOG(ERROR) << "Invalid search case in search strategy!";
				break;
			}
		}
		CPModel.AddDecisionStrategy(search_vars, VariableOrdering, ValueOrdering);
	}

	void LayoutPlanner::DefineRotatableRectangleSearchStrategy(sat::CpModelBuilder &CPModel, std::vector<RotatableRectangle> &components) {

		switch (ComponentSorting)
		{
		case ComponentSortingStrategy::BY_AREA_DESC:
			std::sort(components.begin(), components.end(), RotatableRectangle::DescArea);
			break;
		case ComponentSortingStrategy::BY_ORDER_ASC:
			std::sort(components.begin(), components.end(), RotatableRectangle::AscOrder);
			break;
		default:
			LOG(ERROR) << "Invalid component sorting case in search strategy!";
		}

		std::vector<sat::IntVar> search_vars;
		for (auto& component : components) {
			switch (CoordinateUse)
			{
			case CoordinateUseStrategy::X_START:
				search_vars.push_back(component.X.StartVar());
				search_vars.push_back(component.X_rot.StartVar());
				break;
			case CoordinateUseStrategy::Y_START:
				search_vars.push_back(component.Y.StartVar());
				search_vars.push_back(component.Y_rot.StartVar());
				break;
			default:
				LOG(ERROR) << "Invalid search case in search strategy!";
				break;
			}
		}
		CPModel.AddDecisionStrategy(search_vars, VariableOrdering, ValueOrdering);
	}

	std::string LayoutPlanner::CSSMap(ComponentSortingStrategy strategy) {
		switch (strategy)
		{
		case ComponentSortingStrategy::BY_ORDER_ASC:
			return "By-order-asc";
		case ComponentSortingStrategy::BY_AREA_DESC:
			return "By-area-desc";
		default:
			return "Invalid CSS option!";
		}
	}

	std::string LayoutPlanner::CUSMap(CoordinateUseStrategy strategy) {
		switch (strategy)
		{
		case CoordinateUseStrategy::X_START:
			return "X-start";
		case CoordinateUseStrategy::Y_START:
			return "Y-start";
		default:
			return "Invalid CUS option!";
		}
	}

	template <typename T>
	void LayoutPlanner::SaveLayoutAsJson(sat::CpSolverResponse result, std::vector<T> components, JsonData board_data, int board_num, std::vector<json>& layout_jsons) {

		int w_board = (int)board_data.value()["width"];
		int l_board = (int)board_data.value()["length"];
		std::string board_guid = board_data.value()["guid"];

		LOG(INFO) << std::endl;
			
		// loop for layout plans
		for (int bn = 0; bn < board_num; bn++) {

			int sum_component_area_on_board = 0;

			std::vector<int> component_ids;
			std::vector<std::string> order_guids;

			std::vector<json> cuttings;
			// loop for cuttings
			for (auto &component : components) {
				if (bn == component.CalculateBoardNum(result, w_board)) {
					json cutting = component.ToJson(result, bn, w_board);
					cuttings.push_back(cutting);
					component_ids.push_back(component.ComponentId);
					sum_component_area_on_board += component.Area;
					if (!(std::find(order_guids.begin(), order_guids.end(), component.OrderGuid) != order_guids.end())) {
						order_guids.push_back(component.OrderGuid);
					}
				}
			}

			if (component_ids.size() > 0) {

				float utilization = (float)sum_component_area_on_board / (float)(w_board * l_board);

				LOG(INFO) << bn << ". board utilization: " << utilization * 100 << " %" << std::endl;
				
				json layout = {
				{ "boardGuid", board_guid },
				{ "utilization", utilization },
				{ "cuttings", cuttings },
				{ "componentIds", component_ids },
				{ "orderGuids", order_guids }
				};

				layout_jsons.push_back(layout);
			}
		}
	}
}
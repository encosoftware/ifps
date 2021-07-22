#pragma once

#define NOMINMAX

#include <iostream>
#include <fstream>
#include <map>
#include "ortools/sat/cp_model.h"
#include "json.hpp"

#include "Rectangle.hpp"
#include "RotatableRectangle.hpp"
#include "Utils.hpp"

using json = nlohmann::json;
using JsonData = nlohmann::detail::iteration_proxy_value<nlohmann::detail::iter_impl<nlohmann::json> >;
using JsonArray = nlohmann::detail::iteration_proxy<nlohmann::detail::iter_impl<nlohmann::json> >;
using JsonValue = nlohmann::json::value_type;

namespace operations_research {

	enum ComponentSortingStrategy
	{
		BY_ORDER_ASC = 1,
		BY_AREA_DESC = 2,
	};

	enum CoordinateUseStrategy
	{
		X_START = 1,
		Y_START = 2
	};

	class LayoutPlanner
	{
	public:
		
		// maximum solving time for the individual solvers before termination
		float MaxTimeInSeconds;
		// the minimum length of reusable board fragment, under this no leftover is considered
		int LeftoverThreshold;
		// the size of the leftover lenght decrease at each iteration
		int LeftoverStep;
		// if the free space on the board under this proportion, new board is added
		float AreaMultiplier;
		// if new board was added, this fraction controlls the initial lefover length (based on board length)
		float LeftoverLengthMultiplier;
		// number of cores working on the solving process
		int ParallelWorkerNum;
		// whether the user wants detailed logs from inside the solver or not
		bool DetailedSolverLogs;
		// path to the file where the results will be written
		std::string OutputDir;
		// folder to save the logs
		std::string LogDir;
		// folder to save all the html cuttign plans
		std::string HtmlDir;
		// enum for component presolving strategy
		ComponentSortingStrategy ComponentSorting;
		// enum to control which coordinate is considered during search
		CoordinateUseStrategy CoordinateUse;
		// enum for variable ordering
		sat::DecisionStrategyProto::VariableSelectionStrategy VariableOrdering;
		// enum for value ordering
		sat::DecisionStrategyProto::DomainReductionStrategy ValueOrdering;		
		
		LayoutPlanner(std::string search_settings_file, std::string output_dir, std::string html_dir, ComponentSortingStrategy strategy = ComponentSortingStrategy::BY_AREA_DESC);
		~LayoutPlanner() {};

		int InitSearch(std::string data_file_name);
		void ProcessBoard(JsonArray component_array, JsonData board_data, std::vector<json>& layout_jsons);

		template <typename T>
		void GenerateHtmlLayout(const sat::CpSolverResponse result, std::vector<T> components, std::vector<Rectangle> borders, Rectangle leftover,
			JsonData board_data, int board_num, bool with_leftover);

		int CalculateSumComponentArea(JsonArray component_array, JsonValue board_id);
		int RunCPIteration(JsonArray component_array, JsonData board_data, int leftover_length, int board_num, bool is_rotatable, std::vector<json>& layout_jsons);
	
		std::vector<Rectangle> AddBorderVariables(sat::CpModelBuilder &CPModel, sat::NoOverlap2DConstraint &no_overlap, JsonData board_data, const int board_num);
		Rectangle AddLeftoverVariable(sat::CpModelBuilder &CPModel, sat::NoOverlap2DConstraint &no_overlap, JsonData board_data, 
			const int board_num, const int leftover_length);

		std::vector<Rectangle> AddDefaultComponentVariables(sat::CpModelBuilder &CPModel, sat::NoOverlap2DConstraint &no_overlap,
			JsonArray component_array, JsonData board_data, const int board_num);
		std::vector<RotatableRectangle> AddRotatableComponentVariables(sat::CpModelBuilder &CPModel, sat::NoOverlap2DConstraint &no_overlap,
			JsonArray component_array, JsonData board_data, const int board_num);
		
		void DefineRectangleSearchStrategy(sat::CpModelBuilder &CPModel, std::vector<Rectangle> &components);
		void DefineRotatableRectangleSearchStrategy(sat::CpModelBuilder &CPModel, std::vector<RotatableRectangle> &components);

		template <typename T>
		void SaveLayoutAsJson(sat::CpSolverResponse result, std::vector<T> components, JsonData board_data, int board_num, std::vector<json>& layout_jsons);

		static std::string CSSMap(ComponentSortingStrategy strategy);
		static std::string CUSMap(CoordinateUseStrategy strategy);

		void Print();

		// for component status check
		const int CFC_LAYOUT_COMPLETED = 1;
	};
}
#pragma once

#define NOMINMAX

#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>
#include <iomanip>
#include <chrono>

#include "DataLoader.hpp"
#include "Workstation.hpp"
#include "Component.hpp"
#include "Unit.hpp"
#include "Plan.hpp"
#include "LayoutPlan.hpp"
#include "CNCPlan.hpp"
#include "EdgingPlan.hpp"
#include "SortingPlan.hpp"
#include "AssemblyPlan.hpp"
#include "PackingPlan.hpp"

#include "ortools/sat/cp_model.h"
#include "json.hpp"

namespace operations_research {

	// Handles the whole search process
	class Scheduler
	{
	public:

		sat::CpModelBuilder MODEL;

		Scheduler() {};
		Scheduler(int shift_num, int shift_length_hours, SchedulerSettings settings, std::string output_dir, std::string html_dir);
		~Scheduler() {};
		
		// Initialises the search procedure
		void Init(std::string input_data_file, std::string cuttings_data_file, bool reschedule);

		std::vector<LayoutPlan> CreateLayoutPlans(static Domain schedule_domain, std::string cuttings_data_file, std::vector<LayoutWorkstation> workstations, bool verbose = false);
		std::vector<CNCPlan> CreateCNCPlans(static Domain schedule_domain, std::vector<Component> components, std::vector<CNCWorkstation> workstations, bool verbose = false);
		std::vector<EdgingPlan> CreateEdgingPlans(static Domain schedule_domain, std::vector<Component> components, std::vector<EdgingWorkstation> workstations, bool verbose = false);
		std::vector<SortingPlan> CreateSortingPlans(static Domain schedule_domain, std::vector<Unit> units, std::vector<SortingWorkstation> workstations, bool verbose = false);
		std::vector<AssemblyPlan> CreateAssemblyPlans(static Domain schedule_domain, std::vector<Unit> units, std::vector<AssemblyWorkstation> workstations, bool verbose = false);
		std::vector<PackingPlan> CreatePackingPlans(static Domain schedule_domain, std::vector<Unit> units, std::vector<PackingWorkstation> workstations, bool verbose = false);
		
		// Creates the borders between the days to avoid job overlapping
		std::vector<Plan> CreateDayBorders(int planned_days);
		// Creates the lunchbreaks for each shift
		std::vector<Plan> CreateLunchBreaks(int planned_days);

		Domain CalculateDomain(int planned_days);
		
		// Adds the precedence constraints between the components on layout plans and the CNC plans
		void AddLayoutCNCPrecedence(std::vector<LayoutPlan> layout_plans, std::vector<CNCPlan> cnc_plans);
		// Adds the precedence constraints between the CNC and edging plans for each component
		void AddCNCEdgingPrecedence(std::vector<CNCPlan> cnc_plans, std::vector<EdgingPlan> edging_plans);
		// Adds the precedence constraints between the units sorting plan and all of it's compoents edging plans
		void AddEdgingSortingPrecedence(std::vector<EdgingPlan> edging_plans, std::vector<SortingPlan> sorting_plans);
		// Adds the precedence constraints between the units sorting and assembly plans
		void AddSortingAssemblyPrecedence(std::vector<SortingPlan> sorting_plans, std::vector<AssemblyPlan> assembly_plans);
		// Adds the precedence constraints between the units assembly and packing plans
		void AddAssemblyPackingPrecendece(std::vector<AssemblyPlan> assembly_plans, std::vector<PackingPlan> packing_plans);

		template <typename P>
		std::vector<json> PlanTypeToJson(sat::CpSolverResponse result, std::vector<P> plans);

		// Defines the objective as the minimization of the last plan's end in the received plan vector
		template <typename P>
		void AddObjective(std::vector<P> plans, Domain schedule_domain);

		// Constraints that plans on the same workstation cannot overlap and creates placeholder intervals to exclude already occupied times
		template <typename Ws, typename P>
		void AddWorkstationNoOverlap(std::vector<Ws> workstations, std::vector<P> plans, bool reschedule);
		
		// Helper function to define pairwise no-overlap between intervals, used to fully exclude day and lunch
		// breaks from the domain
		template <typename E, typename P>
		void AddPairwiseNoOverlap(std::vector<E> excluded_times, std::vector<P> other_plans);

		// Helper function to collect the starting variables of the given plan's intervals, it is used when determining
		// the value and variable ordering strategies of the seach
		template <typename P>
		void AddIntervalStartVars(std::vector<sat::IntVar>& search_variables, std::vector<P> plans);

		// Html helper function for visualization
		template <typename Ws, typename P>
		void PrintWorkstation(std::ofstream &html, sat::CpSolverResponse result, std::vector<Ws> workstations, std::vector<P> plans, std::vector<Plan> borders_and_breaks, int width, bool reschedule);

		void PrioritizeByDeadline(std::vector<PackingPlan> packing_plans, std::vector<Order> orders);

		// Starts and handles the search process - model params, intermediate solutions, decision strategy
		sat::CpSolverResponse Search(std::vector<sat::IntVar> search_variables);

		int CalculateOccupiedMinutes(bool reschedule, BRDateTime wsLastPlanEnd);
		
		std::ofstream CreateHtml();
		void CloseHtml(std::ofstream& html);

		std::string GetDateTimeAsString();
		std::string GetOrdersAsString(json_array orders);

		std::string GetOutputDir() { return OutputDir; }
		SchedulerSettings GetSettings() { return Settings; };
		BRDateTime GetToday() { return TodayStart; }

		// component status checks
		const int CFC_LAYOUT_COMPLETED = 1;
		const int CFC_CNC_COMPLETED = 2;
		const int CFC_EDGING_COMPLETED = 3;

		// unit status checks
		const int CFU_SORTING_COMPLETED = 1;
		const int CFU_ASSEMBLY_COMPLETED = 2;
		const int CFU_PACKING_COMPLETED = 3;

	private:

		int ShiftNumber;
		int ShiftLengthHours;
		SchedulerSettings Settings;
		std::string OutputDir;
		std::string HtmlDir;
		BRDateTime TodayStart;
	};
}
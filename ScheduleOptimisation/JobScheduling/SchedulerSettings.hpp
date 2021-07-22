#pragma once
#define NOMINMAX

#include <fstream>

#include "json.hpp"

using json = nlohmann::json;
using json_data = nlohmann::detail::iteration_proxy_value<nlohmann::detail::iter_impl<nlohmann::json> >;
using json_array = nlohmann::detail::iteration_proxy<nlohmann::detail::iter_impl<nlohmann::json> >;
using json_value = nlohmann::json::value_type;

namespace operations_research {

	// This class stores the settings for the scheduler and it's created from a settings json file
	class SchedulerSettings
	{
	public:

		// The CNC distance for each component is multiplied by this number to compensate the repositioning distances
		// not included in the cutting distance
		float CncDistanceMultiplier;
		// This parameter is factory specific, it helps determining the end of the domain for the selected orders
		float AvgNumOfComponentsPerHour;

		// The start of the work in the factory in 0-24 format
		int WorkStartHour;
		// The time untill the "lunch" break from the start of the shift, not necessary a whole number
		float HoursTillBreak;
		// The length of the break in minutes
		int LunchDurationMinutes;
		
		int RescheduleDelayMinutes;

		int TimezoneOffset;

		// Parameter of the solver, this is the maximum time for each search, if it is exceeded the best result
		// returned so far
		int MaxTimeInSeconds;
		// Parameter of the solver, this is the number of parallel processes used in the search, it should be less or
		// equal with the number of physical cores in the CPU
		int NumSearchWorkers;
		// Parameter of the solver, this boolean decides if the detailed logs about the search are required or not
		bool LogSearchProgress;
		// Parameter of the solver, determines the directory of the logs
		std::string LogDirectory;

		SchedulerSettings() {}
		SchedulerSettings(std::string settings_file_name) {
		
			std::ifstream settings_file_handler;
			settings_file_handler.open(settings_file_name);
			static json settings_data;
			settings_file_handler >> settings_data;
		
			CncDistanceMultiplier = settings_data["settings"]["cncDistanceMultiplier"];
			AvgNumOfComponentsPerHour = settings_data["settings"]["avgNumOfComponentsPerHour"];
			WorkStartHour = settings_data["settings"]["workStartHour"];
			HoursTillBreak = settings_data["settings"]["hoursTillBreak"];
			LunchDurationMinutes = settings_data["settings"]["lunchDurationMinutes"];
			RescheduleDelayMinutes = settings_data["settings"]["rescheduleDelayMinutes"];
			TimezoneOffset = settings_data["settings"]["timezoneOffset"];

			MaxTimeInSeconds = settings_data["settings"]["solver"]["maxTimeInSeconds"];
			NumSearchWorkers = settings_data["settings"]["solver"]["numSearchWorkers"];
			LogSearchProgress = settings_data["settings"]["solver"]["logSearchProgress"];
			LogDirectory = settings_data["settings"]["solver"]["logDirectory"].get<std::string>();
		}

		~SchedulerSettings() {}
	};
}
#pragma once
#define NOMINMAX

#include <iostream>
#include <vector>
#include <sstream>

#include "date.h"
#include "SchedulerSettings.hpp"

namespace operations_research {

	// Helper to split std::string by delimiter char
	std::vector<std::string> SplitString(std::string input, char delim);

	// Helper to create year_month_day objects from std::string input
	date::year_month_day CreateDateFromString(std::string date_str);

	class BRDateTime
	{
	public:
		date::year_month_day Day;
		int Hour;
		int Minute;

		BRDateTime() {}

		// used to create now BRDateTime with hours and minutes
		BRDateTime(SchedulerSettings sSettings) {

			auto tp_now = std::chrono::system_clock::now();
			auto tpm = date::floor<std::chrono::minutes>(tp_now);
			auto dp = date::floor<date::days>(tp_now);
			auto time = date::make_time(tpm - dp);

			Minute = time.minutes().count();
			Hour = time.hours().count() + sSettings.TimezoneOffset; // timezone correction from config file
			Day = date::year_month_day{ date::floor<date::days>(tp_now) };
		};

		// used for creating exact date-time for workstations
		BRDateTime(std::string d, int h, int m) : Hour(h), Minute(m) {
			Day = CreateDateFromString(d);
		};

		// used for tracking today workstart
		BRDateTime(int work_start_h) : Hour(work_start_h) {
			Minute = 0;
			Day = date::year_month_day{ date::floor<date::days>(std::chrono::system_clock::now()) };
		};

		// used for order deadlines where time does not matter
		BRDateTime(std::string d) {
			Day = CreateDateFromString(d);
			Hour = 23;
			Minute = 59;
		};

		// used to create datetime for plans
		BRDateTime(int days_from_today, int h, int m) : Hour(h), Minute(m) {;
			Day = date::year_month_day{ date::floor<date::days>(std::chrono::system_clock::now()) + date::days{days_from_today} };
		}

		~BRDateTime() {};

		std::string DayToString();

		friend std::ostream& operator<< (std::ostream& out, BRDateTime dt) {
			out << dt.DayToString() << " " << std::to_string(dt.Hour) << ":" << std::to_string(dt.Minute) << std::endl;
			return out;
		}
	};

	int CalculateMinutesFromTodayStart(BRDateTime t_start, BRDateTime t_ws, int shift_num, int shift_len);

	int CalculateDays(int domain_min, int work_start_hour, int shift_num, int shift_len);

	BRDateTime CalculateDateTime(int solution_minute, int work_start_hour, int shift_num, int shift_len);
}
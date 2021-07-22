#include "Utils.hpp"

namespace operations_research {

	std::vector<std::string> SplitString(std::string input, char delim) {

		std::vector<std::string> tokens;
		std::stringstream ss;
		ss << input;
		std::string token;
		while (std::getline(ss, token, delim)) {
			tokens.push_back(token);
		}

		return tokens;
	}

	date::year_month_day CreateDateFromString(std::string date_str) {

		using namespace date;

		std::vector<std::string> date_parts = SplitString(date_str, '-');
		int y = std::stoi(date_parts[0]);
		int m = std::stoi(date_parts[1]);
		int d = std::stoi(date_parts[2]);

		return year{ y } / m / d;
	}

	int CalculateMinutesFromTodayStart(BRDateTime t_start, BRDateTime t_ws, int shift_num, int shift_len) {

		// days
		auto day_duration = date::floor<date::days>(date::sys_days{ t_ws.Day } - date::sys_days{ t_start.Day });
		int day_diff = day_duration.count();
		
		if (day_diff < 0) {
			return 0;
		}
		int day_diff_in_minutes = day_diff * shift_num * shift_len * 60 + day_diff;

		// minutes and hours
		int hour_diff_in_minutes = (t_ws.Hour - t_start.Hour) * 60;
		int minute_diff = t_ws.Minute - t_start.Minute;

		return std::max(0, (day_diff_in_minutes + hour_diff_in_minutes + minute_diff));
	}

	std::string BRDateTime::DayToString() {

		std::string date_str = std::to_string((int)Day.year()) + "-" + std::to_string((unsigned)Day.month()) + "-" + std::to_string((unsigned)Day.day());

		return date_str;
	}

	int CalculateDays(int domain_min, int work_start_hour, int shift_num, int shift_len) {

		float day_minutes = shift_len * shift_num * 60.0;
		int minutes_left = domain_min;
		int days = 0;

		while (minutes_left >= day_minutes) {
			minutes_left = minutes_left - day_minutes;
			if (minutes_left > 0) {
				days++;
				minutes_left = minutes_left - 1;
			}
		}

		return days;
	}

	BRDateTime CalculateDateTime(int domain_minute, int work_start_hour, int shift_num, int shift_len) {

		int real_days = CalculateDays(domain_minute, work_start_hour, shift_num, shift_len);

		int last_day_minutes = domain_minute - (real_days * shift_num * shift_len * 60) - real_days;
		int flat_hours = std::floor((float)last_day_minutes / 60.0);
		int real_hours = flat_hours + work_start_hour;
		int real_minutes = last_day_minutes % 60;

		return BRDateTime(real_days, real_hours, real_minutes);
	}
}
#include "Utils.hpp"

std::string GetDateTimeAsString() {

	std::stringstream ss;
	struct tm buf;
	auto now = std::chrono::system_clock::now();
	std::time_t now_time = std::chrono::system_clock::to_time_t(now);
	localtime_s(&buf, &now_time);

	auto time_obj = std::put_time(&buf, "%Y-%m-%d_%H-%M-%S");
	ss << time_obj;

	std::string time_str = ss.str();

	return time_str;
}
#define NOMINMAX

#include "Scheduler.hpp"

using namespace operations_research;

int main(int argc, char** argv) {

	try {
		::google::InitGoogleLogging(argv[0]);
		FLAGS_alsologtostderr = true;
		FLAGS_log_prefix = false;
		FLAGS_minloglevel = 0;

		if (argc != 9) {
			throw std::invalid_argument("Incorrect number of command line parameters provided!");
		}
		
		std::string input_data_file = argv[1];
		std::string cuttings_data_file = argv[2];
		std::string settings_file = argv[3];

		SchedulerSettings settings(settings_file);
		FLAGS_log_dir = settings.LogDirectory;
		LOG(INFO) << "\nSettings file:   " << settings_file;
		LOG(INFO) << "Logging to:      " << settings.LogDirectory;

		std::string output_dir = argv[4];
		std::string html_dir = argv[8];
		
		int shift_num = std::stoi(argv[5]);
		if (shift_num < 1 || shift_num > 4) {
			throw std::invalid_argument("Invalid value for shift number! (<1) or (>4)");
		}
		
		int shift_length_hours = std::stoi(argv[6]);
		if (shift_length_hours < 3 || shift_length_hours > 12) {
			throw std::invalid_argument("Invalid value for shift length hours! (<4) or (>12)");
		}

		// TODO valahogy elkapni az OR Tools belsõ check fail eseményeket és rendes exceptiont dobni

		std::string mode = argv[7];
		Scheduler scheduler(shift_num, shift_length_hours, settings, output_dir, html_dir);

		if (mode == "MODE_ADD_ORDERS") {
			LOG(INFO) << "MODE:   normal";
			scheduler.Init(input_data_file, cuttings_data_file, false);
		}
		else if (mode == "MODE_RESCHEDULE") {
			LOG(INFO) << "MODE:   reschedule";
			scheduler.Init(input_data_file, cuttings_data_file, true);
		}
		else {
			throw std::invalid_argument("Invalid mode!");
		}
	}
	catch (json::exception& e) {
		LOG(ERROR) << "JSON error detected: " << e.what();
		return 1;
	}
	catch (std::invalid_argument& e) {
		LOG(ERROR) << "Argument error: " << e.what();
		return 2;
	}
	catch (std::out_of_range& e) {
		LOG(ERROR) << "Invalid setting: " << e.what();
		return 3;
	}
	catch (std::logic_error& e) {
		LOG(ERROR) << "\n*FAILURE*";
		LOG(ERROR) << "  Cause: " << e.what();
		return 4;
	}
	catch (std::exception& e) {
		LOG(ERROR) << "Something went wrong! More info:\n   " << e.what();
		return 5;
	}
	catch (...) {
		LOG(ERROR) << "Unexpected exception, shutting down...";
		return 6;
	}

	return 0;
}
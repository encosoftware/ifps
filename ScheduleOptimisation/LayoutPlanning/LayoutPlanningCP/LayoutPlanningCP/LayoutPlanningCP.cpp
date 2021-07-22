#define NOMINMAX

#include <iostream>

#include "LayoutPlanner.hpp"

using namespace operations_research;

int main(int argc, char** argv) {

	// NOTE:
	// width = x
	// lenth = y

	//TODO a 12-es order 11-es boardnál fura hiba, ez is csöndben elhasal, meg kell nézni és valahogy jelezni - csak rotatable esetben
	//TODO ha teljesen le van zárva a task takarítsunk össze a fájlokban
	//TODO nem priority task, de ha marad idõ akkor a maradék boardok kezelése feature
	//TODO exceptionos application flow?
	
	// parse command line arguments
	std::string data_file_name = argv[1];
	std::string search_case = argv[2];
	std::string search_settings_file = argv[3];
	std::string output_dir = argv[4];
	std::string html_dir = argv[5];
	
	ComponentSortingStrategy strategy;
	if (search_case == "BY_ORDER_ASC") {
		strategy = ComponentSortingStrategy::BY_ORDER_ASC;
	}
	else if (search_case == "BY_AREA_DESC") {
		strategy = ComponentSortingStrategy::BY_AREA_DESC;
	}
	else {
		return 5;
	}

	// set parameters
	LayoutPlanner planner(search_settings_file, output_dir, html_dir, strategy);

	// logging setup
	::google::InitGoogleLogging(argv[0]);
	FLAGS_alsologtostderr = true;
	FLAGS_log_prefix = false;
	FLAGS_minloglevel = 0;
	FLAGS_log_dir = planner.LogDir;

	planner.Print();

	int exit_code = planner.InitSearch(data_file_name);
	LOG(INFO) << "EXIT CODE: " << exit_code << std::endl;
	
	return exit_code;

	// status 1: success
	// status 2: caught exception, check logs
	// status 3: unknown internal error
	// status 4: invalid json file
	// status 5: invalid strategy
}
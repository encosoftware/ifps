#pragma once

#define NOMINMAX

#include <iostream>
#include <fstream>
#include "ortools/sat/cp_model.h"
#include "json.hpp"

using json = nlohmann::json;

namespace operations_research {

	class Rectangle
	{
	public:

		sat::IntervalVar X;
		sat::IntervalVar Y;
		std::string Name;
		int Area;
		std::string OrderGuid;
		int ComponentId;

		Rectangle() {};
		Rectangle(sat::IntervalVar x, sat::IntervalVar y, std::string name, int area, std::string order_guid, int component_id) : 
			X(x), Y(y), Name(name), Area(area), OrderGuid(order_guid), ComponentId(component_id) {};
		~Rectangle() {};

		static bool DescArea(const Rectangle &a, const Rectangle &b);
		static bool AscOrder(const Rectangle &a, const Rectangle &b);

		void Print(std::ofstream& ofile, sat::CpSolverResponse result, int type, int comp_n, std::string order_guid);

		int CalculateBoardNum(sat::CpSolverResponse result, int w_board);
		json ToJson(sat::CpSolverResponse result, int bn, int w_board);
	};
}
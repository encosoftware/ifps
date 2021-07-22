#pragma once

#define NOMINMAX

#include <iostream>
#include <fstream>
#include "ortools/sat/cp_model.h"

#include "Rectangle.hpp"

namespace operations_research {

	class RotatableRectangle : public Rectangle
	{
	public:

		sat::IntervalVar X_rot;
		sat::IntervalVar Y_rot;

		RotatableRectangle() {};
		RotatableRectangle(sat::IntervalVar x, sat::IntervalVar y, sat::IntervalVar x_rot, sat::IntervalVar y_rot, std::string name, int area, std::string order_guid, int component_id) :
			Rectangle(x, y, name, area, order_guid, component_id), X_rot(x_rot), Y_rot(y_rot) {};
		~RotatableRectangle() {};

		static bool DescArea(const RotatableRectangle &a, const RotatableRectangle &b);
		static bool AscOrder(const RotatableRectangle &a, const RotatableRectangle &b);

		void Print(std::ofstream& ofile, sat::CpSolverResponse result, int type, int comp_n, std::string order_guid);
		
		int CalculateBoardNum(sat::CpSolverResponse result, int w_board);
		json ToJson(sat::CpSolverResponse result, int bn, int w_board);
	};
}
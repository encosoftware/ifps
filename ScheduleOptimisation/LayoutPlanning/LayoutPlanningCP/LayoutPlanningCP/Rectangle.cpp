#include "Rectangle.hpp"

namespace operations_research {

	bool Rectangle::DescArea(const Rectangle &a, const Rectangle &b) {
		return a.Area > b.Area;
	}

	bool Rectangle::AscOrder(const Rectangle &a, const Rectangle &b) {
		// TODO - by deadline!
		return a.OrderGuid < b.OrderGuid;
	}

	void Rectangle::Print(std::ofstream& ofile, sat::CpSolverResponse result, int type, int comp_n, std::string order_guid) {

		int left = sat::SolutionIntegerValue(result, X.StartVar());
		int width = sat::SolutionIntegerValue(result, X.SizeVar());

		int top = sat::SolutionIntegerValue(result, Y.StartVar());
		int height = sat::SolutionIntegerValue(result, Y.SizeVar());

		std::string area_type = "";
		if (type == 1) {
			area_type = "component";
		}
		else if (type == 2) {
			area_type = "border";
		}
		else if (type == 3) {
			area_type = "leftover";
		}
		else {
			LOG(ERROR) << "Unknown type!" << std::endl;
		}

		ofile << "<div class='" + area_type + "' style='left:"
			+ std::to_string(left) + "px; width:"
			+ std::to_string(width) + "px; top:"
			+ std::to_string(top) + "px; height:"
			+ std::to_string(height) + "px;'>" + Name + "&nbsp;&nbsp;&nbsp;</br><b>Num:" + std::to_string(comp_n) + ".</b></br>ORDER: " + order_guid + "</div>";
	}

	int Rectangle::CalculateBoardNum(sat::CpSolverResponse result, int w_board) {
		int x_end = sat::SolutionIntegerValue(result, X.EndVar());
		int bn = std::floor(x_end / (w_board + 1));
		return bn;
	}

	json Rectangle::ToJson(sat::CpSolverResponse result, int bn, int w_board) {

		int tlx = sat::SolutionIntegerValue(result, X.StartVar()) - bn * w_board - bn;
		int tly = sat::SolutionIntegerValue(result, Y.StartVar());
		int w = sat::SolutionIntegerValue(result, X.SizeVar());
		int l = sat::SolutionIntegerValue(result, Y.SizeVar());

		json cutting = {
			{ "topLeftX", tlx },
			{ "topLeftY", tly },
			{ "width", w },
			{ "length", l},
			{ "componentId", ComponentId }
		};

		return cutting;
	}

}
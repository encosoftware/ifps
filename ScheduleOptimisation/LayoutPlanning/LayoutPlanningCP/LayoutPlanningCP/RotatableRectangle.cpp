#include "RotatableRectangle.hpp"

namespace operations_research {

	bool RotatableRectangle::DescArea(const RotatableRectangle &a, const RotatableRectangle &b) {
		return a.Area > b.Area;
	}

	bool RotatableRectangle::AscOrder(const RotatableRectangle &a, const RotatableRectangle &b) {
		// TODO - by deadline!
		return a.OrderGuid < b.OrderGuid;
	}

	void RotatableRectangle::Print(std::ofstream& ofile, sat::CpSolverResponse result, int type, int comp_n, std::string order_guid) {

		bool is_deafult_orientation = sat::SolutionBooleanValue(result, X.PresenceBoolVar());
		int left, width, top, height;

		if (is_deafult_orientation) {
			left = sat::SolutionIntegerValue(result, X.StartVar());
			width = sat::SolutionIntegerValue(result, X.SizeVar());

			top = sat::SolutionIntegerValue(result, Y.StartVar());
			height = sat::SolutionIntegerValue(result, Y.SizeVar());
		}
		else {
			left = sat::SolutionIntegerValue(result, X_rot.StartVar());
			width = sat::SolutionIntegerValue(result, X_rot.SizeVar());

			top = sat::SolutionIntegerValue(result, Y_rot.StartVar());
			height = sat::SolutionIntegerValue(result, Y_rot.SizeVar());
		}

		std::string area_type = "";
		if (type == 1) {
			if (is_deafult_orientation) {
				area_type = "component default";
			}
			else {
				area_type = "component rotated";
			}
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
			+ std::to_string(height) + "px;'>" + Name + "&nbsp;&nbsp;&nbsp;</br><b>Num:" + std::to_string(comp_n) + ".</b></br>ORDER: 00" + order_guid + "</div>";
	}

	int RotatableRectangle::CalculateBoardNum(sat::CpSolverResponse result, int w_board) {
		
		int bn = -1; int x_end = 0;
		
		bool is_rotated = sat::SolutionBooleanValue(result, X_rot.PresenceBoolVar());
		if (is_rotated) {
			x_end = sat::SolutionIntegerValue(result, X_rot.EndVar());
			bn = std::floor(x_end / (w_board + 1));
		}
		else {
			x_end = sat::SolutionIntegerValue(result, X.EndVar());
			bn = std::floor(x_end / (w_board + 1));
		}

		return bn;
	}

	json RotatableRectangle::ToJson(sat::CpSolverResponse result, int bn, int w_board) {
		
		bool is_rotated = sat::SolutionBooleanValue(result, X_rot.PresenceBoolVar());
		if (is_rotated) {

			int tlx = sat::SolutionIntegerValue(result, X_rot.StartVar()) - bn * w_board - bn;
			int tly = sat::SolutionIntegerValue(result, Y_rot.StartVar());
			int w = sat::SolutionIntegerValue(result, X_rot.SizeVar());
			int l = sat::SolutionIntegerValue(result, Y_rot.SizeVar());

			json cutting = {
				{ "topLeftX", tlx },
				{ "topLeftY", tly },
				{ "width", w },
				{ "length", l},
				{ "componentId", ComponentId }
			};

			return cutting;
		}
		else {

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
}
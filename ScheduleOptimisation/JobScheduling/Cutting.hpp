#pragma once
#define NOMINMAX

#include <iostream>

namespace operations_research {

	class Cutting
	{
	public:
		int ComponentId;
		int TopLeftX;
		int TopLeftY;
		int Width;
		int Length;

		Cutting() {};
		Cutting(int comp_id, int tlx, int tly, int w, int l) : ComponentId(comp_id), TopLeftX(tlx), TopLeftY(tly), Width(w), Length(l) {};
		~Cutting() {};
	};
}
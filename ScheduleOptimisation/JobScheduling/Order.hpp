#pragma once
#define NOMINMAX

#include <iostream>

#include "Utils.hpp"

namespace operations_research {

	class Order
	{
	public:

		std::string Guid;
		std::string Name;
		BRDateTime Deadline;

		Order() {};
		Order(std::string guid, std::string name, std::string date_str) : Guid(guid), Name(name) {
			Deadline = BRDateTime(date_str);
		};
		~Order() {};

		bool operator < (const Order& o) {
			return (Deadline.Day < o.Deadline.Day);
		}
	};
}
class EstimatorProperties:
    def __init__(self, milling_speed, drill_speed, rapid_speed, tool_time, plane_time):
        self._data = {'milling': milling_speed, 'drill': drill_speed, 'rapid': rapid_speed, 'tool': tool_time,
                      'plane': plane_time}

    def __getitem__(self, key):
        return self._data[key]


class MillingProperties:
    def __init__(self, speed, diameter, is_clockwise):
        self._data = {'speed': speed, 'diameter': diameter, 'clw': is_clockwise}

    def __getitem__(self, key):
        return self._data[key]


class DrillProperties:
    def __init__(self, drill_type, speed, is_clockwise):
        self._data = {'type': drill_type, 'speed': speed, 'clw': is_clockwise}

    def __getitem__(self, key):
        return self._data[key]


class Tool:
    def __init__(self):
        self._data = {}

    def add(self, key, value):
        self._data[key] = value

    def __getitem__(self, key):
        return self._data[key]


class Machine:
    def __init__(self, estimator, milling, drill, tool, restriction, coolant, return_height, is_clockwise):
        self._estimator_properties = estimator
        self._milling_properties = milling
        self._drill_properties = drill
        self._tool = tool

        self._data = {'coolant': coolant, 'restriction': restriction, 'clw': is_clockwise, 'return': return_height}

    def get_tool_with_diameter(self, diameter):
        return self._tool[diameter]

    def is_spin_clockwise(self):
        return self._data['clw']

    def get_coolant(self):
        return self._data['coolant']

    def invalid_plane(self, value):
        return value in self._data['restriction']['plane']

    def invalid_tool(self, value):
        return value in self._tool.keys()

    def is_milling_clockwise(self):
        return self._milling_properties['clw']

    def get_milling_tool_diameter(self):
        return self._milling_properties['diameter']

    def get_milling_tool(self):
        return self.get_tool_with_diameter(self.get_milling_tool_diameter())

    def get_milling_speed(self):
        return self._milling_properties['speed']

    def get_drill_return_height(self):
        return self._data['return']

    def get_drill_code(self):
        return self._drill_properties['type']

    def get_drill_speed(self):
        return self._drill_properties['speed']

    def get_time_estimator_property(self):
        return self._estimator_properties


class ISOMachineGenerator:
    @staticmethod
    def generate():
        tool_set = Tool()
        for key, value in zip(range(40, 351), range(311)):
            tool_set.add(key/10, "%s_isoD%s" % (value, key/10))
        estimator = EstimatorProperties(800, 500, 1200, 1, 2)
        milling = MillingProperties(800, 4.0, False)
        drill = DrillProperties(81, 1000, False)
        return Machine(estimator, milling, drill, tool_set, {'plane': [2, 4]}, None, 0.1, True)

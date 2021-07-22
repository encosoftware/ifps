from enum import Enum
import numpy as np


class Plane(Enum):
    NONE = 0
    XY = 1
    Mirror_XY = 2
    YZ = 3
    Mirror_YZ = 4
    XZ = 5
    Mirror_XZ = 6


class RelativeCoordinate:
    def __init__(self, coord, mv):
        self._base = coord >= mv/2
        self._both = mv/4 >= coord if not self._base else coord > 3/4*mv
        self._rel = coord*2-(int(self._base)*mv + int(self._base == self._both)*mv)

    def __eq__(self, other):
        return self.base == other.base and self.both == other.both and self.rel == other.rel

    def __repr__(self):
        return "%s_%s_%s" % (self.rel, self.base, self.base)

    def __str__(self):
        return self.__repr__()

    @property
    def base(self):
        return self._base

    @property
    def both(self):
        return self._both

    @property
    def rel(self):
        return self._rel

    def to_absolute_coordinate(self, mv):
        coord = (int(self._base)*mv + int(self._base == self._both)*mv+self._rel)/2
        return coord

    def get_rel_coord(self):
        return self._rel, self._base, self._both

    def to_dict(self):
        return {'value': self.rel, 'endSide': self.base, 'sameSide': self.both}


class RelativePoint:
    def __init__(self, pt, size):
        self._coord = {}
        self.__calculate('X', pt['X'], size[0])
        self.__calculate('Y', pt['Y'], size[1])
        self.__calculate('Z', pt['Z'], size[2])

    def __getitem__(self, key):
        return self.coord[key]

    def __eq__(self, other):
        return self.coord == other.coord

    @property
    def coord(self):
        return self._coord

    def __calculate(self, key, coord, mv):
        self._coord[key] = RelativeCoordinate(coord, mv)

    def get_absolute_point(self, size):
        pt = {k: self[k].to_absolute_coordinate(size[ind]) for ind, k in enumerate(self._coord.keys())}
        return AbsolutePoint(pt, size).coord

    def to_dict(self):
        return {key: value.to_dict() for key, value in self.coord.items()}


class AbsolutePoint:
    def __init__(self, pt, size):
        self._size = size
        self._coord = {k: pt[k] if k != 'Z' else pt[k] - size[2] for k in 'XYZ'}

    def __getitem__(self, key):
        return self.coord[key]

    def __eq__(self, other):
        return self.coord == other.coord

    @property
    def coord(self):
        return self._coord

    def get_absolute_point(self, size):
        if size != self._size:
            pt = {k: self.coord[k] if k != 'Z' else self.coord[k] + size[2] for k in 'XYZ'}
            return RelativePoint(pt, self._size).get_absolute_point(size)
        return self.coord

    def to_dict(self):
        return {key: value for key, value in self.coord.items()}


class Command:
    def __init__(self, order_num, param):
        self._pt = param
        self._order_number = order_num

    def __repr__(self):
        return "Pt: %s" % self._pt

    def __str__(self):
        return self.__repr__()

    def get_order(self):
        return self._order_number

    def is_arc(self):
        return False

    def get_pt(self):
        return self._pt

    def convert_to_absolute(self, size):
        self._pt = self._pt.get_absolute_point(size)

    def to_dict(self):
        return {'successionNumber': self._order_number, 'relativePoint': self._pt.to_dict()}


class Line(Command):
    def __repr__(self):
        return "Line :: %s" % super().__repr__()


class Arc(Command):
    def __init__(self, order_num, params):
        self._i = params[1]
        self._j = params[2]
        self._clw = params[3]
        super().__init__(order_num, params[0])

    def is_arc(self):
        return True

    def get_clw(self):
        return self._clw

    def get_ij(self):
        return {'I': self._i, 'J': self._j}

    def __repr__(self):
        return "Arc :: %s (I: %s, J: %s; %s)" % (super().__repr__(), self._i, self._j, self._clw)

    def __str__(self):
        return self.__repr__()

    def to_dict(self):
        return {**super().to_dict(), 'CenterDistanceAxisX': self._i, 'CenterDistanceAxisY': self._j,
                'Clockwise': self._clw}


class Hole(Command):
    def __repr__(self):
        return "Hole :: %s" % super().__repr__()

    def rotate_hole_with_180(self, size):
        return Hole(self.get_order(), {k: self._pt[k] if k == 'Z' else s - self._pt[k] for k, s in zip('XYZ', size)})


class Sequence:
    def __init__(self, order):
        self._order = order

    def __repr__(self):
        return "Sequence %s" % self.get_start_pt()

    def __str__(self):
        return self.__repr__()

    def is_circle(self):
        return False

    def is_rectangle(self):
        return False

    def is_polygon(self):
        return False

    def is_drill(self):
        return False

    def set_order_number(self, order):
        self._order = order

    def get_order_number(self):
        return self._order

    def get_start_pt(self):
        return {'X': 0, 'Y': 0, 'Z': 0}

    def get_end_pt(self):
        return self.get_start_pt()

    def get_dist(self, other):
        x = self.get_start_pt()['X']
        y = self.get_start_pt()['Y']
        return np.linalg.norm([other['X']-x, other['Y']-y])

    def to_dict(self):
        return {'successionNumber': self._order}


class Rectangle(Sequence):
    def __init__(self, order, params):
        self._a_pt = params[0]
        self._b_pt = params[1]
        self._c_pt = params[2]
        self._d_pt = params[3]
        super().__init__(order)

    def __repr__(self):
        return "Rectangle :: %s -> %s -> %s -> %s" % (self._a_pt, self._b_pt, self._c_pt, self._d_pt)

    def __str__(self):
        return self.__repr__()

    def get_points(self):
        return [self._b_pt, self._c_pt, self._d_pt, self._a_pt]

    def get_start_pt(self):
        return self.get_points()[-1]

    def get_end_pt(self):
        return self.get_start_pt()

    def is_rectangle(self):
        return True

    def set_pts(self, pts):
        self._a_pt = pts[0]
        self._b_pt = pts[1]
        self._c_pt = pts[2]
        self._d_pt = pts[3]

    def convert_to_absolute(self, size):
        self._a_pt = self._a_pt.get_absolute_point(size)
        self._b_pt = self._b_pt.get_absolute_point(size)
        self._c_pt = self._c_pt.get_absolute_point(size)
        self._d_pt = self._d_pt.get_absolute_point(size)

    def to_dict(self):
        pt_dict = {k: v.to_dict() for k, v in zip(['topLeft', 'bottomLeft', 'bottomRight', 'topRight'],
                                                  [self._a_pt, self._b_pt, self._c_pt, self._d_pt])}
        return {**super().to_dict(), **pt_dict}


class Circle(Sequence):
    def __init__(self, order, params):
        self._start = params[0]
        self._i = params[1]
        self._j = params[2]
        super().__init__(order)

    def __repr__(self):
        return "Circle :: %s  (%s; %s)" % (self._start, self._i, self._j)

    def __str__(self):
        return self.__repr__()

    def get_start_pt(self):
        return self._start

    def is_circle(self):
        return True

    def get_ij(self):
        return {'I': self._i, 'J': self._j}

    def convert_to_absolute(self, size):
        self._start = self._start.get_absolute_point(size)

    def to_dict(self):
        return {**super().to_dict(), 'startPoint': self._start,
                'CenterDistanceAxisX': self._i, 'CenterDistanceAxisY': self._j}


class Polygon(Sequence):
    def __init__(self, order, params):
        self._start_pt = params[0]
        self._end_pt = params[1]
        self._commands = []
        super().__init__(order)

    def __repr__(self):
        return "Polygon :: %s -> %s %s" % (self._start_pt, self._end_pt, self._commands)

    def __str__(self):
        return self.__repr__()

    def get_start_pt(self):
        return self._start_pt

    def is_polygon(self):
        return True

    def get_end_pt(self):
        return self.get_start_pt()

    def add_command(self, command):
        self._commands.append(command)

    def get_commands(self):
        return self._commands

    def get_points(self):
        return [self._start_pt, *[x.get_pt() for x in self._commands]]

    def convert_to_absolute(self, size):
        self._start_pt = self._start_pt.get_absolute_point(size)
        self._end_pt = self._end_pt.get_absolute_point(size)
        for command in self._commands:
            command.convert_to_absolute(size)

    def to_dict(self):
        return {**super().to_dict(), 'startPoint': self._start_pt, 'commands': [c.to_dict() for c in self._commands]}


class Drill2(Sequence):
    def __init__(self, order, params):
        self._diameter = params['Diameter']
        self._plane = params['Plane']
        self._holes = []
        super().__init__(order)

    def __repr__(self):
        return "Drill :: %s; %s :: %s" % (self._plane, self._diameter, self._holes)

    def __str__(self):
        return self.__repr__()

    def is_drill(self):
        return True

    def get_plane_key(self):
        if self._plane in [Plane.mXY, Plane.XY]:
            return 'Z'
        elif self._plane in [Plane.mYZ, Plane.YZ]:
            return 'X'
        elif self._plane in [Plane.mXZ, Plane.XZ]:
            return 'Y'
        return None

    def get_start_pt(self):
        return self._holes[0].get_pt()

    def get_end_pt(self):
        return self._holes[-1].get_pt()

    def get_holes(self):
        return self._holes

    def get_pts(self):
        return [hole.get_pt() for hole in self._holes]

    def get_diameter(self):
        return self._diameter

    def get_plane(self):
        return self._plane

    def add_hole(self, hole):
        self._holes.append(hole)
        self._holes = sorted(self._holes, key=lambda x: x.get_order())

    def add_hole_list(self, holes):
        self._holes.extend(sorted(holes, key=lambda x: x.get_order()))

    def convert_to_absolute(self, size):
        for hole in self._holes:
            hole.convert_to_absolute(size)

    def rotate_drill_with_180(self, size):
        v = self._plane.value
        params = {'Plane': Plane(v - 1 if v % 2 == 0 else v + 1), 'Diameter': self.get_diameter()}
        drill = Drill2(self.get_order_number(), params)
        for hole in self._holes:
            drill.add_hole(hole.rotate_hole_with_180(size))
        return drill

    def to_dict(self):
        return {**super().to_dict(), 'plane': self._plane.name, 'diameter': self._diameter,
                'holes': [h.to_dict() for h in self._holes]}

    def from_dict(self):
        pass


class Component:
    def __init__(self, name, rot, size, pt=None):
        self._name = name
        self._rot = rot
        self._size = {k: v for k, v in zip('XYZ', size)}
        self._sequences = []
        self._pt = pt if type(pt) is dict else {k: self._size[k]/2 for k in 'XYZ'}
        self._debug_size = None

    def shifted_keys(self):
        keys = ['X', 'Y', 'Z']
        rot = [*self._rot]
        tmp = []
        while True in rot:
            ind = rot.index(True)
            swap = list(range(3))
            swap.remove(ind)
            tmp.append((keys[swap[0]], keys[swap[1]]))
            keys[swap[1]], keys[swap[0]] = keys[swap[0]], keys[swap[1]]
            rot[swap[1]], rot[swap[0]] = rot[swap[0]], rot[swap[1]]
            rot[ind] = False
        return keys

    def get_neutral(self):
        return 'XYZ'[self.shifted_keys().index('Z')]

    def get_original_size_unit(self):
        return [self._size[k] for k in self.shifted_keys()]

    def get_original_size_comp(self):
        return [self._size[k] for k in 'XYZ']

    def get_new_size_unit(self, size):
        neu = self.get_neutral()
        return [v + (size[k] if k != neu else 0) for k, v in zip('XYZ', self.get_original_size_unit())]

    def get_new_size_comp(self, size):
        return self.size_unit_to_component(self.get_new_size_unit(size))

    def size_unit_to_component(self, size):
        tmp = {k: v for k, v in zip(self.shifted_keys(), size)}
        return [tmp[k] for k in 'XYZ']

    def get_name(self):
        return self._name

    def get_sequences(self):
        return self._sequences

    def convert_sequences(self, size):
        if size is None:
            new_size = self.get_original_size_comp()
        else:
            new_size = self.get_new_size_comp(size)
        self._convert_seq(new_size)

    def convert_sequences_with_conflict(self, available, original, new, resolver_func):
        if new is None:
            new_size = self.get_original_size_comp()

        else:
            original = {k: v for k, v in zip('XYZ',  original)}
            new = {k: v for k, v in zip('XYZ',  new)}
            comp_size = {k: v for k, v in zip('XYZ', self.get_original_size_unit())}
            tmp_size = resolver_func(available, original, new, comp_size)
            new_size = self.size_unit_to_component([tmp_size[k] for k in 'XYZ'])
        self._convert_seq(new_size)

    def _convert_seq(self, new_size):
        self._debug_size = new_size

        for seq in self._sequences:
            seq.convert_to_absolute(new_size)

    def add_sequence(self, seq):
        self._sequences.append(seq)

    def add_sequence_list(self, seqs):
        self._sequences.extend(seqs)

    def get_exact_plane(self):
        return self.get_plane_to(self.get_neutral())

    def get_plane_to(self, key):
        value = self._pt[key]
        key_size = self._size[self.shifted_keys()['XYZ'.index(key)]]
        return key, value - key_size/2, value + key_size/2

    @staticmethod
    def default_resolver(available, original, new, component_size):
        return component_size

    @staticmethod
    def optional_resolver(available, original, new, component_size):
        ret = {}
        keys = 'XYZ'
        for k, v in available.items():
            if v:
                ret[k] = new[k] - original[k] + component_size[k]
            else:
                ret[k] = component_size[k]
            keys = keys.replace(k, '')
        ret[keys] = component_size[keys]
        return ret


class Unit:
    def __init__(self, name, size):
        self._name = name
        self._original_size = size
        self._components = {}

    def get_name(self):
        return self._name

    def add_component(self, component):
        self._add(component)

    def add_component_list(self, components):
        for component in components:
            self._add(component)

    def _add(self, component):
        key = component.get_exact_plane()[0]
        if key in self._components.keys():
            self._components[key].append(component)
        else:
            self._components[key] = [component]

    def get_component(self):
        return self._components

    def get_component_list(self):
        ret = []
        for _, components in self._components.items():
            ret.extend(components)
        return ret

    def convert_components(self, size=None, resolver_func=Component.default_resolver):
        size_diff = None if size is None else self.get_size_difference(size)
        for key, components in self._components.items():
            for component in components:
                conflict = self._get_conflict_component(component)
                if len(conflict) > 1:
                    available = self._conflict_available_dim(key, conflict)
                    component.convert_sequences_with_conflict(available, self._original_size, size, resolver_func)
                else:
                    component.convert_sequences(size_diff)

    def get_converted_component_list(self, size=None, resolver_func=Component.default_resolver):
        self.convert_components(size, resolver_func)
        return self.get_component_list()

    def get_size_difference(self, size):
        return {k: (new - original) for original, new, k in zip(self._original_size, size, 'XYZ')}

    @staticmethod
    def _conflict_available_dim(key, conflict):
        keys = 'XYZ'.replace(key, '')
        if len(conflict) <= 1:
            return {k: True for k in keys}
        ret = {}
        for k in keys:
            _, act_min, act_max = conflict[0].get_plane_to(k)
            for conf in conflict[1:]:
                _, other_min, other_max = conf.get_plane_to(k)
                if act_min <= other_min <= act_max or act_min <= other_max <= act_max:
                    ret[k] = True
                else:
                    ret[k] = False

        return {k: False for k in keys} if all(list(ret.values())) else ret

    def _get_conflict_component(self, component):
        key, act_min, act_max = component.get_exact_plane()
        ret = [component]
        for comp in self._components[key]:
            if comp is component:
                continue
            other_min, other_max = comp.get_exact_plane()[1:]
            if act_min <= other_min <= act_max or act_min <= other_max <= act_max:
                ret.append(comp)
        return ret


"""
a) difference of the curve:
g00 x-10
g02 x0 y10 i13 j-3
g00 x-10 y00
g02 x0 y10 i10
g01 x10
pl3 file B <- param meaning


g0 x2.02 y1.01
g1 y1.41
g2 x2.22 y1.61 i.23 j-.03
g1 x3.42
g2 x3.62 y1.41 i-.03 j-.23
g1 y1.01
g2 x3.42 y.81 i-.23 j.03
g1 x2.22
g2 x2.02 y1.01 i.03 j.23

g0 x4.02 y1.01
g3 x4.22 y.81 i.23 j.03
g1 x5.42
g3 x5.62 y1.01 i-.03 j.23
g1 y1.41
g3 x5.42 y1.61 i-.23 j-.03
g1 x4.22
g3 x4.02 y1.41 i.03 j-.23
g1 y1.01"""

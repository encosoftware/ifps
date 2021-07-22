from unittest.mock import call
from Sequence.converter import SequenceMaker, PolygonType
from Sequence.sequenceType import *


def make_dummy_rectangle():
    return Rectangle(0, [AbsolutePoint({'X': 0, 'Y': 0, 'Z': 0}, [0, 0, 0]) for _ in range(4)])


def make_dummy_circle():
    return Circle(0, [AbsolutePoint({'X': 0, 'Y': 0, 'Z': 0}, [0, 0, 0]), 0, 0])


def make_dummy_polygon():
    return Polygon(0, [AbsolutePoint({'X': 0, 'Y': 0, 'Z': 0}, [0, 0, 0]) for _ in range(2)])


def make_dummy_drill():
    params = {'Start': AbsolutePoint({'X': 0, 'Y': 0, 'Z': 0}, [0, 0, 0]),
              'End': AbsolutePoint({'X': 0, 'Y': 0, 'Z': 0}, [0, 0, 0]), 'Diameter': 0, 'Depth': 0, 'Plane': 0}
    return Drill2(0, params)


class RelativeCoordinateTest:
    data = {'max': 100,
            'coord': [20, 30, 50, 70, 90]}

    def __init__(self):
        self._exp = self.Expected()
        self._arg = self.Argument()
        self._this = None
        self.__make_this()

    def __make_this(self):
        self._this = self.__make_new()

    def __make_new(self):
        return [RelativeCoordinate(coord, self.data['max']) for coord in self.data['coord']]

    class Expected:
        @property
        def base(self):
            return [False, False, True, True, True]

        @property
        def both(self):
            return [True, False, False, False, True]

        @property
        def relative(self):
            return [40, -40, 0, 40, -20]

        @property
        def coordinate(self):
            return RelativeCoordinateTest.data['coord']

        @property
        def coordinate_for_new_max(self):
            return [20, 80, 100, 120, 190]

        @property
        def relative_coordinate(self):
            return zip(self.relative, self.base, self.both)

    class Argument:
        @property
        def max(self):
            return RelativeCoordinateTest.data['max']

        @property
        def new_max(self):
            return 200

    @property
    def expected(self):
        return self._exp

    @property
    def argument(self):
        return self._arg

    @property
    def this(self):
        return self._this

    @property
    def new_instance(self):
        return self.__make_new()


class RelativePointTest:
    data = {'size': [300, 200, 20],
            'pt0': {'X': 200, 'Y': 50, 'Z': 7},
            'pt1': {'X': 20, 'Y': 50, 'Z': 7},
            'pt2': {'X': 100, 'Y': 170, 'Z': 7}}

    def __init__(self):
        self._exp = self.Expected()
        self._arg = self.Argument()
        self._this = None
        self.__make_this()

    def __make_this(self):
        self._this = self.__make_new()

    def __make_new(self):
        return [RelativePoint(self.data[pt_k], self.data['size'])for pt_k in ['pt0', 'pt1', 'pt2']]

    class Expected:
        @property
        def coordinates(self):
            return [{k: RelativeCoordinate(RelativePointTest.data[pt_k][k], s)
                     for k, s in zip('XYZ', RelativePointTest.data['size'])} for pt_k in ['pt0', 'pt1', 'pt2']]

        @property
        def point(self):
            return [{'X': 200, 'Y': 50, 'Z': -13}, {'X': 20, 'Y': 50, 'Z': -13}, {'X': 100, 'Y': 170, 'Z': -13}]

        @property
        def new_point(self):
            return [{'X': 250, 'Y': 50, 'Z': - 18}, {'X': 20, 'Y': 50, 'Z': - 18}, {'X': 150, 'Y': 270, 'Z': - 18}]

    class Argument:
        @property
        def size(self):
            return RelativePointTest.data['size']

        @property
        def new_size(self):
            return [400, 300, 30]

    @property
    def expected(self):
        return self._exp

    @property
    def argument(self):
        return self._arg

    @property
    def this(self):
        return self._this

    @property
    def new_instance(self):
        return self.__make_new()


class AbsolutePointTest:
    data = {'size': [150, 50, 20],
            'points': [{'X': 60, 'Y': 40, 'Z': 10}, {'X': 100, 'Y': 10, 'Z': 18}, {'X': 140, 'Y': 25, 'Z': 1}]}

    def __init__(self):
        self._exp = self.Expected()
        self._arg = self.Argument()
        self._this = None
        self.__make_this()

    def __make_this(self):
        self._this = self.__make_new()

    def __make_new(self):
        return [AbsolutePoint(pt, self.data['size']) for pt in self.data['points']]

    class Expected:
        @property
        def point(self):
            return [{'X': 60, 'Y': 40, 'Z': -10}, {'X': 100, 'Y': 10, 'Z': -2}, {'X': 140, 'Y': 25, 'Z': -19}]

        @property
        def new_point(self):
            return [{'X': 60, 'Y': 90, 'Z': -10}, {'X': 100, 'Y': 10, 'Z': -2}, {'X': 140, 'Y': 50, 'Z': -19}]

    class Argument:
        @property
        def size(self):
            return AbsolutePointTest.data['size']

        @property
        def new_size(self):
            return [150, 100, 20]

    @property
    def expected(self):
        return self._exp

    @property
    def argument(self):
        return self._arg

    @property
    def this(self):
        return self._this

    @property
    def new_instance(self):
        return self.__make_new()


class ComponentTest:
    data = [
        {'name': "Front",
         'size': [150, 100, 1.8],
         'rotation': [True, False, False],
         'center': {'X': 75, 'Y': 60.9, 'Z': 50}
         },
        {'name': "Bottom",
         'size': [146.4, 60, 1.8],
         'rotation': [False, False, False],
         'center': {'X': 75, 'Y': 30, 'Z': 0.9}
         },
        {'name': "Right Side",
         'size': [60, 100, 1.8],
         'rotation': [False, True, True],
         'center': {'X': 149.1, 'Y': 30, 'Z': 50}
         },
        {'name': "Left Side",
         'size': [60, 100, 1.8],
         'rotation': [False, True, True],
         'center': {'X': 0.9, 'Y': 30, 'Z': 50}
         },
        {'name': "Back",
         'size': [148, 98, 0.4],
         'rotation': [True, False, False],
         'center': {'X': 75, 'Y': 1.2, 'Z': 50}
         },
        {'name': "Osszekoto elolso",
         'size': [146.4, 20, 1.8],
         'rotation': [False, False, False],
         'center': {'X': 75, 'Y': 10, 'Z': 99.1}
         },
        {'name': "Osszekoto hatso",
         'size': [146.4, 20, 1.8],
         'rotation': [False, False, False],
         'center': {'X': 75, 'Y': 50, 'Z': 99.1}
         }
    ]

    def __init__(self):
        self._exp = self.Expected()
        self._arg = self.Argument()
        self._this = None
        self.__make_this()

    def __make_this(self):
        self._this = self.__make_new()
        for comp, seq in zip(self._this, self.argument.add_sequence):
            comp.add_sequence(seq)

    def __make_new(self):
        component_list = []
        for cd in self.data:
            component_list.append(Component(cd['name'], cd['rotation'], cd['size'], cd['center']))
        return component_list

    class Expected:
        @property
        def shifted_key(self):
            return [['X', 'Z', 'Y'], ['X', 'Y', 'Z'], ['Z', 'X', 'Y'], ['Z', 'X', 'Y'], ['X', 'Z', 'Y'],
                    ['X', 'Y', 'Z'], ['X', 'Y', 'Z']]

        @property
        def neutral_key(self):
            return ['Y', 'Z', 'X', 'X', 'Y', 'Z', 'Z']

        @property
        def size_component(self):
            size = []
            for cd in ComponentTest.data:
                size.append(cd['size'])
            return size

        @property
        def size_unit(self):
            return [[150, 1.8, 100], [146.4, 60, 1.8], [1.8, 60, 100], [1.8, 60, 100], [148, 0.4, 98], [146.4, 20, 1.8],
                    [146.4, 20, 1.8]]

        @property
        def size_new_component(self):
            return [[170, 100, 1.8], [146.4, 60, 1.8], [80, 90, 1.8], [40, 110, 1.8], [148, 98, 0.4], [146.4, 15, 1.8],
                    [136.4, 20, 1.8]]

        @property
        def size_new_unit(self):
            return [[170, 1.8, 100], [146.4, 60, 1.8], [1.8, 80, 90], [1.8, 40, 110], [148, 0.4, 98], [146.4, 15, 1.8],
                    [136.4, 20, 1.8]]

        @property
        def size_new_convert(self):
            return [[170, 90, 1.8], [166.4, 65, 1.8], [65, 90, 1.8], [65, 90, 1.8], [168, 88, 0.4], [166.4, 25, 1.8],
                    [166.4, 25, 1.8]]

        @property
        def size_new_conflict_convert_opt(self):
            return [*self.size_new_convert[:5], [166.4, 20, 1.8], [166.4, 20, 1.8]]

        @property
        def exact_plane(self):
            return [['Y', 60, 61.8], ['Z', 0, 1.8], ['X', 148.2, 150], ['X', 0, 1.8], ['Y', 1, 1.4], ['Z', 98.2, 100],
                    ['Z', 98.2, 100]]

        @property
        def plane(self):
            return [['X', 0, 150], ['Y', 0, 60], ['Z', 0, 100], ['Y', 0, 60], ['Z', 1, 99], ['X', 1.8, 148.2],
                    ['Z', 98.2, 100]]

        @property
        def sequence_type(self):
            return [Rectangle, Polygon, Circle, Rectangle, Drill2, Circle, Drill2]

    class Argument:
        @property
        def size_unit_original(self):
            return FurnitureTest.data['size']

        @property
        def size_unit_new(self):
            return [170, 66.8, 90]

        @property
        def size_change(self):
            return [{'X': 20, 'Y': 0, 'Z': 0}, {'X': 0, 'Y': 0, 'Z': 10}, {'X': 10, 'Y': 20, 'Z': -10},
                    {'X': 0, 'Y': -20, 'Z': 10}, {'X': 0, 'Y': 15, 'Z': 0}, {'X': 0, 'Y': -5, 'Z': 0},
                    {'X': -10, 'Y': 0, 'Z': 0}]

        @property
        def available(self):
            return [{'X': True, 'Z': True}, {'X': True, 'Y': True}, {'Y': True, 'Z': True}, {'Y': True, 'Z': True},
                    {'X': True, 'Z': True}, {'X': True, 'Y': False}, {'X': True, 'Y': False}]

        @property
        def plane(self):
            return ['X', 'Y', 'Z', 'Y', 'Z', 'X', 'Z']

        @property
        def add_sequence(self):
            return [make_dummy_rectangle(), make_dummy_polygon(), make_dummy_circle(), make_dummy_rectangle(),
                    make_dummy_drill(), make_dummy_circle(), make_dummy_drill()]

        @property
        def add_sequence_list(self):
            return [[make_dummy_rectangle(), make_dummy_drill()], [make_dummy_circle()],
                    [make_dummy_drill(), make_dummy_polygon(), make_dummy_drill()], [],
                    [make_dummy_polygon()], [make_dummy_rectangle()], []]

    @property
    def expected(self):
        return self._exp

    @property
    def argument(self):
        return self._arg

    @property
    def this(self):
        return self._this

    @property
    def new_instance(self):
        return self.__make_new()


class FurnitureTest:
    data = {'name': "Test furniture",
            'size': [150, 61.8, 100]}
    _component_data = ComponentTest()

    def __init__(self):
        self._exp = self.Expected()
        self._arg = self.Argument()
        self._this = None
        self.__make_this()

    def __make_this(self):
        self._this = self.__make_new()
        self._this.add_component_list(self.argument.components)

    def __make_new(self):
        return Unit(self.data['name'], self.data['size'])

    class Expected:
        @property
        def component_number(self):
            return list(range(1, 8))

        @property
        def conflict_index(self):
            conf = []
            components = FurnitureTest._component_data.this
            for index in [[0], [1], [2], [3], [4], [6, 5], [5, 6]]:
                conf.append(set([components[ind] for ind in index]))
            return conf

        @property
        def component_list(self):
            return set(FurnitureTest._component_data.this)

        @property
        def available(self):
            return FurnitureTest._component_data.argument.available

        @property
        def add_calls(self):
            return [call(comp) for comp in FurnitureTest._component_data.this]

        @property
        def convert_with_conflict_arguments(self):
            return {'X': True, 'Y': False}, [150, 61.8, 100], None, Component.default_resolver

        @property
        def new_size_differences(self):
            return [{'X': 20, 'Y': 0, 'Z': 0}, {'X': 0, 'Y': 10, 'Z': 0}, {'X': -20, 'Y': 5, 'Z': 10},
                    {'X': 0, 'Y': -20, 'Z': -30}]

        @property
        def convert_with_conflict_arguments_new_size(self):
            return [({'X': True, 'Y': False}, [150, 61.8, 100], size, Component.default_resolver)
                    for size in [[170, 61.8, 100], [150, 71.8, 100], [130, 66.8, 110], [150, 41.8, 70]]]

        @property
        def component_converter_arguments(self):
            return [(None, Component.default_resolver), ([150, 80, 100], Component.default_resolver),
                    ([190, 70, 110], Component.optional_resolver)]

        @property
        def add_keys(self):
            tr = [(0, 1), (4, 7)]
            return [set([comp.get_neutral() for comp in FurnitureTest._component_data.this[i0:i1]]) for i0, i1 in tr]

        @property
        def add_key_number(self):
            return [1, 2]

        @property
        def add_numbers(self):
            return [1, 3]

    class Argument:
        @property
        def components(self):
            return FurnitureTest._component_data.this

        @property
        def add(self):
            return [FurnitureTest._component_data.this[0]], FurnitureTest._component_data.this[4:]

        @property
        def new_sizes(self):
            return [[170, 61.8, 100], [150, 71.8, 100], [130, 66.8, 110], [150, 41.8, 70]]

        @property
        def component_params(self):
            return [(), ([150, 80, 100],), ([190, 70, 110], Component.optional_resolver)]

    @property
    def expected(self):
        return self._exp

    @property
    def argument(self):
        return self._arg

    @property
    def this(self):
        return self._this

    @property
    def new_instance(self):
        return self.__make_new()


class SequenceMakerTest:
    data = {'size': [146.4, 60, 1.8],
            'holes': [
                ([3, 10, 1.8], [3, 10, 0.65], 1.5),
                ([3, 50, 1.8], [3, 50, 0.65], 1.5),
                ([143.4, 10, 1.8], [143.4, 10, 0.65], 1.5),
                ([143.4, 50, 1.8], [143.4, 50, 0.65], 1.5),
                ([0, 10, 0.9], [3, 10, 0.9], 0.5),
                ([0, 50, 0.9], [3, 50, 0.9], 0.5),
                ([146.4, 10, 0.9], [143.4, 10, 0.9], 0.5),
                ([146.4, 50, 0.9], [143.4, 50, 0.9], 0.5),
                ([10, 1, 1.8], [10, 1, 0.9], 0.5),
                ([13, 1, 1.8], [13, 1, 0.9], 0.5),
                ([10, 3, 1.8], [10, 3, 0.9], 0.5),
                ([13, 3, 1.8], [13, 3, 0.9], 0.5),
                ([133.4, 1, 1.8], [133.4, 1, 0.9], 0.5),
                ([136.4, 1, 1.8], [136.4, 1, 0.9], 0.5),
                ([133.4, 3, 1.8], [133.4, 3, 0.9], 0.5),
                ([136.4, 3, 1.8], [136.4, 3, 0.9], 0.5)],
            'rectangle':
                {'Height': 0.8,
                 'Polygon': [
                     (0, 58.6, {'B': 0}),
                     (0, 59, {'B': 0}),
                     (146.4, 59, {'B': 0}),
                     (146.4, 58.6, {'B': 0}),
                     (0, 58.6, {'B': 0})]},
            'polygon':
                {'Height': 0.1,
                 'Polygon': [
                     (78, 60, {'B': 0}),
                     (68, 60, {'B': 0}),
                     (68, 52, {'B': -0.5}),
                     (70, 50, {'B': 0}),
                     (76, 50, {'B': -0.5}),
                     (78, 52, {'B': 0}),
                     (78, 60, {'B': 0})]},
            'circle':
                {'Height': 0.1,
                 'Polygon': [
                     (1.4, 40, {'B': 1}),
                     (11.4, 40, {'B': 1}),
                     (1.4, 40, {'B': 0})]
                 }}

    def __init__(self):
        self._exp = self.Expected()
        self._arg = self.Argument()
        self._this = None
        self.__make_this()

    def __make_this(self):
        self._this = self.__make_new()

    def __make_new(self):
        holes = self.data['holes']
        polygons = [self.data[key] for key in ['rectangle', 'polygon', 'circle']]
        size = self.data['size']
        return SequenceMaker(holes, polygons, size)

    class Expected:
        @property
        def hole_param(self):
            expected = []
            for ind, depth, plane in zip([0, 5, 6, 9], [1.15, 3, 3, 0.9], [Plane(1), Plane(4), Plane(3), Plane(1)]):
                _, pt, d = SequenceMakerTest.data['holes'][ind]
                rel_pt = RelativePoint({k: v for k, v in zip('XYZ', pt)}, SequenceMakerTest.data['size'])
                expected.append({'Pt': rel_pt, 'Diameter': d, 'Plane': plane})
            return expected

        @property
        def key_func(self):
            return [57.6, 58, 59, 58.6, 57.6, 78, 68, np.sqrt(68**2 + 52**2), np.sqrt(70**2 + 50**2),
                    np.sqrt(76**2 + 50**2), np.sqrt(78**2 + 52**2), 78]

        @property
        def find_center(self):
            return [np.array([100, 20]), np.array([110, 10]), np.array([90, 30])]

        @property
        def min_poly_pt(self):
            return [(SequenceMakerTest.data[key]['Polygon'][index], index)
                    for key, index in [('rectangle', 0), ('polygon', 1), ('circle', 0)]]

        @property
        def convert_poly_params_pts(self):
            rect = SequenceMakerTest.data['rectangle']
            z = rect['Height']
            size = SequenceMakerTest.data['size']
            pts0 = [{'pt': RelativePoint({'X': x, 'Y': y, 'Z': z}, size)} for x, y, _ in rect['Polygon']]
            pts1 = [{'pt': RelativePoint({'X': p[0], 'Y': p[1], 'Z': 0.1}, size)} if len(p) == 2 else
                    {'pt': RelativePoint({'X': p[0], 'Y': p[1], 'Z': 0.1}, size), 'I': p[2], 'J': p[3], 'CLW': False}
                    for p in [(78, 60), (68, 60), (68, 52), (70, 50, 2, 0), (76, 50), (78, 52, 0, 2), (78, 60)]]
            pts2 = [{'pt': RelativePoint({'X': p[0], 'Y': p[1], 'Z': 0.1}, size)} if len(p) == 2 else
                    {'pt': RelativePoint({'X': p[0], 'Y': p[1], 'Z': 0.1}, size), 'I': p[2], 'J': p[3], 'CLW': True}
                    for p in [(1.4, 40), (11.4, 40, 5, 0), (1.4, 40, -5, 0)]]
            return [pts0, pts1, pts2]

        @property
        def convert_poly_params_cts(self):
            ct1 = set()
            ct1.add((70, 52))
            ct1.add((76, 52))
            ct2 = set()
            ct2.add((6.4, 40))
            ct2.add((6.4, 40))
            return [set(), ct1, ct2]

        @property
        def polygon_type(self):
            return [PolygonType.RECTANGLE, PolygonType.POLYGON, PolygonType.CIRCLE]

        @property
        def polygon_param_pt_list(self):
            rect = SequenceMakerTest.data['rectangle']
            z = rect['Height']
            size = SequenceMakerTest.data['size']
            pts0 = [{'pt': RelativePoint({'X': x, 'Y': y, 'Z': z}, size)} for x, y, _ in rect['Polygon']]
            pts1 = [{'pt': RelativePoint({'X': p[0], 'Y': p[1], 'Z': 0.1}, size)} if len(p) == 2 else
                    {'pt': RelativePoint({'X': p[0], 'Y': p[1], 'Z': 0.1}, size), 'I': p[2], 'J': p[3], 'CLW': False}
                    for p in [(68, 60), (68, 52), (70, 50, 2, 0), (76, 50), (78, 52, 0, 2), (78, 60), (68, 60)]]
            pts2 = [{'pt': RelativePoint({'X': p[0], 'Y': p[1], 'Z': 0.1}, size)} if len(p) == 2 else
                    {'pt': RelativePoint({'X': p[0], 'Y': p[1], 'Z': 0.1}, size), 'I': p[2], 'J': p[3], 'CLW': True}
                    for p in [(1.4, 40), (11.4, 40, 5, 0), (1.4, 40, -5, 0)]]
            return [pts0, pts1, pts2]

        @property
        def polygon_param_min_pt(self):
            return [{'X': 0, 'Y': 58.6, 'Z': 0}, {'X': 68, 'Y': 60, 'Z': 0}, {'X': 1.4, 'Y': 40, 'Z': 0}]

        @property
        def spec_poly(self):
            return [Rectangle, Polygon, Circle]

        @property
        def poly_command(self):
            return [Line, Arc, Line, Arc, Line, Line]

    class Argument:
        @property
        def hole_param(self):
            return [SequenceMakerTest.data['holes'][ind] for ind in [0, 5, 6, 9]]

        @property
        def key_func(self):
            ret = []
            for key in ['rectangle', 'polygon', 'circle']:
                ret.extend(SequenceMakerTest.data[key]['Polygon'])
            return ret

        @property
        def find_center_pts(self):
            return np.array([30, 30]), np.array([90, 90])

        @property
        def find_center_angle(self):
            return [73.739/180*np.pi, 61.927/180*np.pi, 90/180*np.pi]

        @property
        def min_poly_pt(self):
            return [SequenceMakerTest.data[key] for key in ['rectangle', 'polygon', 'circle']]

        @property
        def convert_poly_params_pts(self):
            return self.min_poly_pt

        @property
        def convert_poly_params_cts(self):
            return self.min_poly_pt

        @property
        def polygon_type(self):
            return self.min_poly_pt

        @property
        def polygon_param_pt_list(self):
            return self.min_poly_pt

        @property
        def polygon_param_min_pt(self):
            return self.min_poly_pt

        @property
        def spec_poly(self):
            return self.min_poly_pt

        @property
        def poly_command(self):
            sm = SequenceMaker([], [SequenceMakerTest.data['polygon']], SequenceMakerTest.data['size'])
            _, pts, _ = sm.calculate_polygon_params(SequenceMakerTest.data['polygon'])
            return sm.make_specialised_polygon(PolygonType.POLYGON, 0, pts).get_commands()

    @property
    def expected(self):
        return self._exp

    @property
    def argument(self):
        return self._arg

    @property
    def this(self):
        return self._this

    @property
    def new_instance(self):
        return self.__make_new()

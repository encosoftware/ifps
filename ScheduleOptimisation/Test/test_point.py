from unittest import TestCase
from parameterized import parameterized
from Test import relative_coordinate_test_data as data0
from Test import relative_point_test_data as data1
from Test import absolute_point_test_data as data2


class TestRelativeCoordinate(TestCase):
    @parameterized.expand(zip(data0.this, data0.expected.base))
    def test_base(self, relative, expected):
        self.assertEqual(expected, relative.base)

    @parameterized.expand(zip(data0.this, data0.expected.both))
    def test_both(self, relative, expected):
        self.assertEqual(expected, relative.both)

    @parameterized.expand(zip(data0.this, data0.expected.relative))
    def test_rel(self, relative, expected):
        self.assertEqual(expected, relative.rel)

    @parameterized.expand(zip(data0.this, data0.expected.coordinate))
    def test_to_absolute_coordinate_old(self, relative, expected):
        self.assertEqual(expected, relative.to_absolute_coordinate(data0.argument.max))

    @parameterized.expand(zip(data0.this, data0.expected.coordinate_for_new_max))
    def test_to_absolute_coordinate_old(self, relative, expected):
        self.assertEqual(expected, relative.to_absolute_coordinate(data0.argument.new_max))

    @parameterized.expand(zip(data0.this, data0.expected.relative_coordinate))
    def test_get_rel_coord(self, relative, expected):
        self.assertEqual(expected, relative.get_rel_coord())


class TestRelativePoint(TestCase):
    @parameterized.expand(zip(data1.this, data1.expected.coordinates))
    def test_coord(self, point, expected):
        self.assertEqual(expected, point.coord)

    @parameterized.expand(zip(data1.this, data1.expected.point))
    def test_get_absolute_point(self, point, expected):
        self.assertEqual(expected, point.get_absolute_point(data1.argument.size))

    @parameterized.expand(zip(data1.this, data1.expected.new_point))
    def test_get_absolute_point_new(self, point, expected):
        self.assertEqual(expected, point.get_absolute_point(data1.argument.new_size))


class TestAbsolutePoint(TestCase):
    @parameterized.expand(zip(data2.this, data2.expected.point))
    def test_get_absolute_point(self, point, expected):
        self.assertEqual(expected, point.get_absolute_point(data2.argument.size))

    @parameterized.expand(zip(data2.this, data2.expected.new_point))
    def test_get_absolute_point_new(self, point, expected):
        self.assertEqual(expected, point.get_absolute_point(data2.argument.new_size))

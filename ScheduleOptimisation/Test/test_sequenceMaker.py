from unittest import TestCase, skip
from parameterized import parameterized
from Test import sequence_maker_test_data as data
from numpy.testing import assert_array_almost_equal


class TestSequenceMaker(TestCase):
    @parameterized.expand(zip(data.argument.hole_param, data.expected.hole_param))
    def test_calculate_hole_params(self, param, expected):
        self.assertEqual(expected, data.this.calculate_hole_params(param))

    @parameterized.expand(zip(data.argument.key_func, data.expected.key_func))
    def test_key_func(self, param, expected):
        self.assertAlmostEqual(expected, data.this.key_func(param), delta=0.0000001)

    @parameterized.expand(zip(data.argument.find_center_angle, data.expected.find_center))
    def test_find_center(self, param, expected):
        value = data.this.find_center(*data.argument.find_center_pts, param)
        assert_array_almost_equal(expected, value, decimal=3)

    @parameterized.expand(zip(data.argument.polygon_type, data.expected.polygon_type))
    def test_polygon_type(self, param, expected):
        pts, cts = data.this.convert_polygon_params(param)
        self.assertEqual(expected, data.this.polygon_type(cts, [pts[0]['pt'], pts[-1]['pt']], len(pts)))

    @parameterized.expand(zip(data.argument.min_poly_pt, data.expected.min_poly_pt))
    def test_find_minimum_polygon_point_and_index(self, param, expected):
        self.assertEqual(expected, data.this.find_minimum_polygon_point_and_index(param))

    @parameterized.expand(zip(data.argument.convert_poly_params_pts, data.expected.convert_poly_params_pts))
    def test_convert_polygon_params_pts(self, param, expected):
        self.assertEqual(expected, data.this.convert_polygon_params(param)[0])

    @parameterized.expand(zip(data.argument.convert_poly_params_cts, data.expected.convert_poly_params_cts))
    def test_convert_polygon_params_cts(self, param, expected):
        self.assertEqual(expected, set(data.this.convert_polygon_params(param)[1]))

    @parameterized.expand(zip(data.argument.polygon_param_pt_list, data.expected.polygon_param_pt_list))
    def test_calculate_polygon_params_pt_list(self, param, expected):
        self.assertEqual(expected, data.this.calculate_polygon_params(param)[1])

    @parameterized.expand(zip(data.argument.polygon_param_min_pt, data.expected.polygon_param_min_pt))
    def test_calculate_polygon_params_min_pt(self, param, expected):
        self.assertEqual(expected, data.this.calculate_polygon_params(param)[2])

    @parameterized.expand(zip(data.argument.spec_poly, data.expected.spec_poly))
    def test_make_specialised_polygon(self, param, expected):
        poly_type, pts, _ = data.this.calculate_polygon_params(param)
        self.assertEqual(expected, type(data.this.make_specialised_polygon(poly_type, 0, pts)))

    @parameterized.expand(zip(data.argument.poly_command, data.expected.poly_command))
    def test_make_specialised_polygon(self, value, expected):
        self.assertEqual(expected, type(value))

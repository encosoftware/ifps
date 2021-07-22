from DataHandler.DataProjection import data_for_customer_analysis, data_for_trend_analysis, data_for_cart_analysis

from unittest import TestCase
from parameterized import parameterized
from pandas.util.testing import assert_frame_equal

from Test import PROJECTION_DATA as DATA


class TestDataProjection(TestCase):
    @parameterized.expand(zip(DATA.argument.raw_people, DATA.expected.customer))
    def test_data_for_customer_analysis(self, argument, expected):
        df = data_for_customer_analysis(DATA.argument.raw_data, argument)
        assert_frame_equal(expected, df)

    @parameterized.expand(zip(DATA.argument.access_key, DATA.expected.trend))
    def test_data_for_trend_analysis(self, argument, expected):
        df = data_for_trend_analysis(DATA.argument.raw_data, argument)
        assert_frame_equal(expected, df)

    def test_data_for_cart_analysis(self):
        df = data_for_cart_analysis(DATA.argument.multi_row_orders)
        self.assertEqual(DATA.expected.cart, df)

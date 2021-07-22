from Analysis.ProbabilityCalculator.TrendClass import TrendClass

from unittest import TestCase
from parameterized import parameterized
from pandas.util.testing import assert_frame_equal

from Test import TREND_DATA as DATA


class TestTrendClass(TestCase):
    @parameterized.expand(zip(DATA.argument.filter, DATA.expected.filter))
    def test_filter_data(self, argument, expected):
        df = TrendClass.filter_data(DATA.argument.sale_history, *argument)
        assert_frame_equal(expected, df)

    def test_filter_data_empty(self):
        df = TrendClass.filter_data(DATA.argument.sale_history, *DATA.argument.filter_empty)
        self.assertTrue(df.empty)

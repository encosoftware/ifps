from Recommendation.Portfolio import Portfolio
from DataHandler.DataInterface import DataTransformInterface

from unittest import TestCase
from unittest.mock import patch
from parameterized import parameterized

from Test import PORTFOLIO_DATA as DATA


class TestPortfolio(TestCase):
    @parameterized.expand(zip(DATA.argument.product, DATA.expected.increase))
    @patch("DataHandler.DataInterface.DataTransformInterface.order_transform")
    @patch("Recommendation.Portfolio.TrendClass", side_effect=DATA.argument.trends)
    def test_is_increasing(self, argument, expected, *_):
        portfolio = Portfolio(DataTransformInterface, None)
        self.assertEqual(expected, portfolio.is_increasing(argument)[0])

    @parameterized.expand(zip(DATA.argument.product, DATA.expected.increase_value))
    @patch("DataHandler.DataInterface.DataTransformInterface.order_transform")
    @patch("Recommendation.Portfolio.TrendClass", side_effect=DATA.argument.trends)
    def test_is_increasing_value(self, argument, expected, *_):
        portfolio = Portfolio(DataTransformInterface, None)
        self.assertEqual(expected, portfolio.is_increasing(argument)[1])

    @parameterized.expand(zip(DATA.argument.product, DATA.expected.decrease_value))
    @patch("DataHandler.DataInterface.DataTransformInterface.order_transform")
    @patch("Recommendation.Portfolio.TrendClass", side_effect=DATA.argument.trends)
    def test_is_decreasing_value(self, argument, expected, *_):
        portfolio = Portfolio(DataTransformInterface, None)
        self.assertEqual(expected, portfolio.is_decreasing(argument)[1])

    @parameterized.expand(zip(DATA.argument.product, DATA.expected.stagnating))
    @patch("DataHandler.DataInterface.DataTransformInterface.order_transform")
    @patch("Recommendation.Portfolio.TrendClass", side_effect=DATA.argument.trends)
    def test_is_stagnating(self, argument, expected, *_):
        portfolio = Portfolio(DataTransformInterface, None)
        self.assertEqual(expected, portfolio.is_stagnating(argument))

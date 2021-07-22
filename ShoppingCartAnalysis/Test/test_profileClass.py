from Analysis.ProbabilityCalculator.ProfileClass import ProfileClass

from unittest import TestCase
from parameterized import parameterized

from Test import PROFILE_DATA as DATA


class TestProfileClass(TestCase):
    @parameterized.expand(zip(DATA.argument.is_in, DATA.expected.is_in))
    def test_customer_is_in(self, argument, expected):
        pc = ProfileClass(*DATA.argument.init_params)
        self.assertEqual(expected, pc.customer_is_in(argument))

    def test_probability_list(self):
        pc = ProfileClass(*DATA.argument.init_params)
        self.assertEqual(DATA.expected.probability_list, pc.probability_list())

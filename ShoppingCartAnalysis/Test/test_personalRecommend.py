from Recommendation.Recommend import PersonalRecommend

from unittest import TestCase
from parameterized import parameterized
from numpy.testing import assert_almost_equal

from Test import RECOMMEND_DATA as DATA


class TestPersonalRecommend(TestCase):
    @parameterized.expand(zip(DATA.argument.recommend_score, DATA.expected.recommend_score))
    def test_calculate_recommendation_score_keys(self, argument, expected):
        recommend = PersonalRecommend(None, 2)
        value = recommend.calculate_recommendation_score(**argument)
        self.assertEqual(expected.keys(), value.keys())

    @parameterized.expand(zip(DATA.argument.recommend_score, DATA.expected.recommend_score))
    def test_calculate_recommendation_score_values(self, argument, expected):
        recommend = PersonalRecommend(None, 2)
        value = recommend.calculate_recommendation_score(**argument)
        assert_almost_equal([*expected.values()], [*value.values()], decimal=6)

    def test_flat_rules(self):
        rule_list = PersonalRecommend.flat_rules(DATA.argument.flat_rules)
        self.assertEqual(DATA.expected.flat_rules, rule_list)

    @parameterized.expand(zip(DATA.argument.flat_profile, DATA.expected.flat_profile))
    def test_flat_profiles(self, argument, expected):
        prof_list = PersonalRecommend.flat_profiles(argument)
        self.assertEqual(expected, prof_list)

    @parameterized.expand(zip(DATA.argument.call, DATA.expected.call))
    def test_call(self, argument, expected):
        recommend = DATA.argument.this()
        value, _ = recommend(argument)
        self.assertEqual(expected, value)

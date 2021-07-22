from Analysis.ProbabilityCalculator.AnalysisClass import BaseAnalysis

from unittest import TestCase
from unittest.mock import patch

from Test import BASE_DATA as DATA


class TestBaseAnalysis(TestCase):
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.customer_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.trend_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.apriori", return_value=(None, []))
    @patch("DataHandler.DataProjection.data_for_cart_analysis")
    def test_rule_analysis_data_for_cart(self, data_for_cart, *_):
        BaseAnalysis(*DATA.argument.init_data)
        data_for_cart.assert_called_with(DATA.expected.data)

    @patch("DataHandler.DataProjection.data_for_cart_analysis", side_effect=lambda x: x + ' <- mock')
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.customer_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.trend_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.apriori", return_value=(None, []))
    def test_rule_analysis_apriori(self, apriori, *_):
        BaseAnalysis(*DATA.argument.init_data, **DATA.argument.cart_kwargs)
        apriori.assert_called_with(DATA.expected.cart_data, **DATA.expected.cart_kwargs)

    @patch("DataHandler.DataProjection.data_for_cart_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.customer_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.trend_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.apriori", return_value=(None, 'Mock rule'))
    @patch("Analysis.ProbabilityCalculator.RuleClass.RuleClass")
    def test_rule_analysis_rule_class(self, rule_class, *_):
        BaseAnalysis(*DATA.argument.init_data)
        rule_class.assert_called_with(DATA.expected.rule_class)

    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.rule_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.trend_analysis")
    @patch("ModelClasses.Classificatory.AbstractCustomerClassification.make_classes")
    @patch("DataHandler.DataProjection.data_for_customer_analysis")
    def test_customer_analysis_data_for_customer(self, data_for_customer, *_):
        BaseAnalysis(*DATA.argument.init_data)
        data_for_customer.assert_called_with(DATA.expected.data, DATA.expected.people)

    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.rule_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.trend_analysis")
    @patch("DataHandler.DataProjection.data_for_customer_analysis", return_value="Profile_data")
    @patch("ModelClasses.Classificatory.AbstractCustomerClassification.make_classes")
    def test_customer_analysis_classificatory(self, make_classes, *_):
        BaseAnalysis(*DATA.argument.init_data)
        make_classes.assert_called_with("Profile_data")

    @patch("DataHandler.DataProjection.data_for_customer_analysis")
    @patch("ModelClasses.Classificatory.AbstractCustomerClassification.make_classes", return_value=[(1, 2)])
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.rule_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.trend_analysis")
    @patch("Analysis.ProbabilityCalculator.ProfileClass.ProfileClass")
    def test_customer_analysis_profile_class(self, profile_class, *_):
        BaseAnalysis(*DATA.argument.init_data)
        profile_class.assert_called_with(1, 2)

    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.rule_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.customer_analysis")
    @patch("Analysis.ProbabilityCalculator.TrendClass.TrendClass")
    @patch("DataHandler.DataProjection.data_for_trend_analysis", return_value=[])
    def test_trend_analysis_data_for_trend(self, data_for_trend_analysis, *_):
        BaseAnalysis(*DATA.argument.init_data, **DATA.argument.trend_kwargs)
        data_for_trend_analysis.assert_called_with(DATA.expected.data)

    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.rule_analysis")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.BaseAnalysis.customer_analysis")
    @patch("DataHandler.DataProjection.data_for_trend_analysis", return_value='Mock data')
    @patch("Analysis.ProbabilityCalculator.TrendClass.TrendClass")
    def test_trend_analysis_trend_class(self, trend_class, *_):
        BaseAnalysis(*DATA.argument.init_data, **DATA.argument.trend_kwargs)
        trend_class.assert_called_with('Mock data', **DATA.expected.trend_kwargs)

    @patch("DataHandler.DataProjection.data_for_cart_analysis")
    @patch("DataHandler.DataProjection.data_for_trend_analysis")
    @patch("DataHandler.DataProjection.data_for_customer_analysis")
    @patch("ModelClasses.Classificatory.AbstractCustomerClassification.make_classes")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.Analysis")
    @patch("Analysis.ProbabilityCalculator.TrendClass.TrendClass", return_value="TrendClass mock")
    @patch("Analysis.ProbabilityCalculator.RuleClass.RuleClass", return_value="RuleClass mock")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.apriori", return_value=(None, None))
    @patch("Analysis.ProbabilityCalculator.RuleClass.CartRules")
    def test_call_cart_rule(self, cart_rule, *_):
        base_analysis = BaseAnalysis(*DATA.argument.init_data)
        base_analysis(None)
        cart_rule.assert_called_with('RuleClass mock')

    @patch("DataHandler.DataProjection.data_for_cart_analysis")
    @patch("DataHandler.DataProjection.data_for_trend_analysis")
    @patch("DataHandler.DataProjection.data_for_customer_analysis")
    @patch("ModelClasses.Classificatory.AbstractCustomerClassification.make_classes", return_value=[("Class", "mock")])
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.Analysis")
    @patch("Analysis.ProbabilityCalculator.TrendClass.TrendClass", return_value="TrendClass mock")
    @patch("Analysis.ProbabilityCalculator.RuleClass.RuleClass", return_value="RuleClass mock")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.apriori", return_value=(None, None))
    @patch("Analysis.ProbabilityCalculator.RuleClass.CartRules")
    @patch("Analysis.ProbabilityCalculator.ProfileClass.ProfileClass")
    def test_call_profile(self, profile, *_):
        base_analysis = BaseAnalysis(*DATA.argument.init_data)
        base_analysis(None)
        profile.assert_called_with("Class", "mock")

    @patch("DataHandler.DataProjection.data_for_cart_analysis")
    @patch("DataHandler.DataProjection.data_for_trend_analysis")
    @patch("DataHandler.DataProjection.data_for_customer_analysis")
    @patch("ModelClasses.Classificatory.AbstractCustomerClassification.make_classes")
    @patch("Analysis.ProbabilityCalculator.TrendClass.TrendClass", return_value="TrendClass mock")
    @patch("Analysis.ProbabilityCalculator.RuleClass.RuleClass")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.apriori", return_value=(None, None))
    @patch("Analysis.ProbabilityCalculator.RuleClass.CartRules", return_value="RuleClass mock")
    @patch("Analysis.ProbabilityCalculator.AnalysisClass.Analysis")
    def test_call_analysis(self, analysis, *_):
        base_analysis = BaseAnalysis(*DATA.argument.init_data)
        base_analysis(None)
        analysis.assert_called_with("RuleClass mock", "TrendClass mock", [])

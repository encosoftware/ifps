from DataHandler import DataProjection
from Analysis.ProbabilityCalculator import RuleClass, TrendClass, ProfileClass
from efficient_apriori import apriori
from datetime import datetime


class BaseAnalysis:
    def __init__(self, classificatory, *args, **kwargs):
        self.__rules, self.__profiles, self.__trends = None, None, None

        ap_args = {
            'min_support': 0.5,
            'min_confidence': 0.5,
            'max_length': 8,
            'verbosity': 0
        }
        trend_args = {
            'from_date': datetime(1990, 1, 1),
            'to_date': datetime.now(),
        }
        for key, item in kwargs.items():
            if key in ap_args.keys():
                ap_args[key] = item
            elif key in trend_args.keys():
                trend_args[key] = item
            else:
                raise KeyError("Parameter key '%s' is not a valid parameter for Analysis!")

        self.rule_analysis(args[0], ap_args)
        self.customer_analysis(classificatory, *args)
        self.trend_analysis(args[0], trend_args)

    def __call__(self, customer):
        cart = RuleClass.CartRules(self.__rules)
        profiles = [prof for prof in self.__profiles if prof.customer_is_in(customer)]
        return Analysis(cart, self.__trends, profiles)

    def get_analysis_dict(self, customer):
        return self(customer).to_dict()

    def rule_analysis(self, order, args):
        data = DataProjection.data_for_cart_analysis(order)
        _, ass_rules = apriori(data, **args)
        self.__rules = RuleClass.RuleClass(ass_rules)

    def customer_analysis(self, classificatory, order, people):
        data = DataProjection.data_for_customer_analysis(order, people)
        self.__profiles = [ProfileClass.ProfileClass(*arg) for arg in classificatory.make_classes(data)]

    def trend_analysis(self, order, args):
        data = DataProjection.data_for_trend_analysis(order)
        self.__trends = TrendClass.TrendClass(data, **args)


class Analysis:
    def __init__(self, *args):
        self.__rules, self.__trends, self.__profiles = args

    def __call__(self, product=None):
        if product is not None:
            self.__rules(product)
        return {'Trend': self.__trends.probability,
                'Customer': [prof.probability for prof in self.__profiles],
                'Rule': self.__rules.probability}

    def to_dict(self):
        return {'rules': self.__rules.to_dict(), 'trends': self.__trends.to_dict(),
                'profiles': [p.to_dict() for p in self.__profiles]}

    @classmethod
    def from_dict(cls, d):
        return cls(RuleClass.CartRules.from_dict(d['rules']),
                   TrendClass.TrendClass.from_dict(d['trends']),
                   [ProfileClass.ProfileClass.from_dict(pr) for pr in d['profiles']])

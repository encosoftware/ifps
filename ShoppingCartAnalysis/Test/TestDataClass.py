from DataHandler.AccessKey import AccessKey as Ak
from ModelClasses.Classificatory import AbstractCustomerClassification
from ModelClasses.Customer import Gender, Age, Married, Attribute, Customer
from pandas import DataFrame
from datetime import datetime

from unittest.mock import MagicMock
from Recommendation.Recommend import PersonalRecommend


class TestData:
    def __init__(self, argument=None, expected=None):
        self.__arg = argument
        self.__exp = expected

    @property
    def argument(self):
        return self.__arg

    @property
    def expected(self):
        return self.__exp


class ArgumentInterface:
    order_cols = [Ak.Order_ID, Ak.Date, Ak.Customer_ID, Ak.Category, Ak.Product, Ak.Quantity, Ak.Price]
    people_cols = [Ak.Customer_ID, Ak.Age, Ak.Gender]
    @property
    def order_true(self):
        dummy_data = list(range(len(self.order_cols)))
        return [DataFrame([dummy_data], columns=self.order_cols),
                DataFrame([[*dummy_data, 30]], columns=[*self.order_cols, Ak.Age]),
                DataFrame([dummy_data], columns=[*self.order_cols[4:], *self.order_cols[:4]])]

    @property
    def order_false(self):
        dummy_data = [list(range(len(self.order_cols)-1))]
        return [DataFrame(dummy_data, columns=[*self.order_cols[:index], *self.order_cols[index+1:]])
                for index, _ in enumerate(self.order_cols)]

    @property
    def order_people_transform(self):
        return ['It', '\'', 's', ' ', 'ok', '!']

    @property
    def validator_exception(self):
        return [['T', 'E', 'S', 'T'], [' ',  'O', 'K']]

    @property
    def people_true(self):
        dummy_data = list(range(len(self.people_cols)))
        return [DataFrame([dummy_data], columns=self.people_cols),
                DataFrame([[*dummy_data, 19, 89]], columns=[*self.people_cols, Ak.Workaholic, Ak.Has_pet]),
                DataFrame([dummy_data], columns=list(reversed(self.people_cols)))]

    @property
    def people_false(self):
        dummy_data = [list(range(len(self.people_cols)-1))]
        return [DataFrame(dummy_data, columns=[*self.people_cols[:index], *self.people_cols[index+1:]])
                for index, _ in enumerate(self.people_cols)]


class ArgumentProjection:
    @property
    def raw_data(self):
        cols = [Ak.Order_ID, Ak.Date, Ak.Customer_ID, Ak.Category, Ak.Product, Ak.Quantity, Ak.Price]
        data = [
            [id('O1'), datetime(2010, 10, 23), id('C1'), 'Test', 'Dummy', 2, 8],
            [id('O2'), datetime(2010, 11, 12), id('C2'), 'Test', 'Case', 3, 9],
            [id('O3'), datetime(2010, 12, 5), id('C3'), 'Test', 'Test', 5, 5],
            [id('O4'), datetime(2010, 12, 10), id('C1'), 'Test', 'Case', 5, 10],
            [id('O5'), datetime(2010, 12, 23), id('C4'), 'Case', 'Ok', 6, 3],
            [id('O6'), datetime(2010, 12, 23), id('C2'), 'Test', 'Case', 5, 12]
        ]
        return DataFrame(data, columns=cols)

    @property
    def raw_people(self):
        cols0 = [Ak.Customer_ID, Ak.Age, Ak.Gender]
        cols1 = [Ak.Customer_ID, Ak.Age, Ak.Gender, Ak.Married]
        data0 = [
            [id('C1'), 24, Gender.Female],
            [id('C2'), 38, Gender.Female],
            [id('C3'), 18, Gender.Male],
            [id('C4'), 50, Gender.Female]
        ]
        data1 = [
            [id('C1'), 24, Gender.Female, Married.Married],
            [id('C2'), 38, Gender.Female, Married.Single],
            [id('C3'), 18, Gender.Male, Married.Single],
            [id('C4'), 50, Gender.Female, Married.Married]
        ]
        return [DataFrame(data0, columns=cols0), DataFrame(data1, columns=cols1)]

    @property
    def access_key(self):
        return [Ak.Product, Ak.Category]

    @property
    def multi_row_orders(self):
        cols = [Ak.Order_ID, Ak.Date, Ak.Customer_ID, Ak.Category, Ak.Product, Ak.Quantity, Ak.Price]
        data = [
            [id('O1'), datetime(2010, 10, 23), id('C1'), 'Test', 'I', 1, 4],
            [id('O1'), datetime(2010, 10, 23), id('C1'), 'Test', 'love', 3, 9],
            [id('O2'), datetime(2010, 11, 10), id('C2'), 'Test', 'the', 4, 2],
            [id('O2'), datetime(2010, 11, 10), id('C2'), 'Test', 'smell', 4, 8],
            [id('O2'), datetime(2010, 11, 10), id('C2'), 'Test', 'of', 5, 5],
            [id('O3'), datetime(2010, 12, 24), id('C3'), 'Test', 'the', 1, 10],
            [id('O3'), datetime(2010, 12, 24), id('C3'), 'Test', 'passing', 10, 1],
            [id('O4'), datetime(2011, 1, 23), id('C4'), 'Test', 'tests.', 30, 30],
        ]
        return DataFrame(data, columns=cols)


class ExpectedProjection:
    @property
    def customer(self):
        cols0 = [Ak.Age, Ak.Gender, Ak.Product, Ak.Quantity, Ak.Price]
        cols1 = [Ak.Age, Ak.Gender, Ak.Married, Ak.Product, Ak.Quantity, Ak.Price]
        data0 = [
            [Age.Young, Gender.Female, 'Case', 5, 2.0],
            [Age.Young, Gender.Female, 'Dummy', 2, 4.0],
            [Age.Young, Gender.Male, 'Test', 5, 1.0],
            [Age.Middle, Gender.Female, 'Case', 8, 2.7],
            [Age.Adult, Gender.Female, 'Ok', 6, 0.5],
        ]
        data1 = [
            [Age.Young, Gender.Female, Married.Married, 'Case', 5, 2.0],
            [Age.Young, Gender.Female, Married.Married, 'Dummy', 2, 4.0],
            [Age.Young, Gender.Male, Married.Single, 'Test', 5, 1.0],
            [Age.Middle, Gender.Female, Married.Single, 'Case', 8, 2.7],
            [Age.Adult, Gender.Female, Married.Married, 'Ok', 6, 0.5],
        ]
        return [DataFrame(data0, columns=cols0), DataFrame(data1, columns=cols1)]

    @property
    def trend(self):
        cols = [Ak.Date, Ak.Product, Ak.Quantity]
        data0 = [
            [datetime(2010, 10, 1), 'Dummy', 2],
            [datetime(2010, 11, 1), 'Case', 3],
            [datetime(2010, 12, 1), 'Case', 10],
            [datetime(2010, 12, 1), 'Ok', 6],
            [datetime(2010, 12, 1), 'Test', 5]
        ]
        data1 = [
            [datetime(2010, 10, 1), 'Test', 2],
            [datetime(2010, 11, 1), 'Test', 3],
            [datetime(2010, 12, 1), 'Case', 6],
            [datetime(2010, 12, 1), 'Test', 15]
        ]
        return [DataFrame(data0, columns=cols), DataFrame(data1, columns=cols)]

    @property
    def cart(self):
        data = [
            ('I', 'love'),
            ('the', 'smell', 'of'),
            ('the', 'passing'),
            ('tests.',)
        ]
        return data


class ArgumentProfile:
    @property
    def init_params(self):
        data = [['To', 9, 3], ['be', 2, 9], ['or', 10, 10], ['not', 7, 2], ['to', 4, 2], ['be?', 8, 1]]
        return Customer({Age.Middle, Gender.Male}), DataFrame(data, columns=[Ak.Product, Ak.Quantity, Ak.Price])

    @property
    def is_in(self):
        return [Customer({Age.Middle, Gender.Male, Married.Single}), Customer({Age.Middle, Gender.Female}),
                Customer({Age.Middle}), Customer({Age.Middle, Gender.Male, Married.Married, Attribute.Workaholic})]


class ExpectedProfile:
    @property
    def is_in(self):
        return [True, False, False, True]

    @property
    def probability_list(self):
        return {'or': (0.25, 10), 'To': (0.225, 3), 'be?': (0.2, 1), 'not': (0.175, 2), 'to': (0.1, 2), 'be': (0.05, 9)}


class ArgumentTrend:
    @property
    def sale_history(self):
        cols = [Ak.Date, Ak.Product, Ak.Quantity]
        data = [
            [datetime(2000, 1, 1), 'I\'m', 1],
            [datetime(2000, 2, 1), 'a', 1],
            [datetime(2000, 6, 1), 'lumberjack', 1],
            [datetime(2000, 7, 1), 'and', 1],
            [datetime(2000, 8, 1), 'I\'m', 1],
            [datetime(2000, 11, 1), 'okay.', 1],
            [datetime(2000, 12, 1), 'I', 1],
            [datetime(2001, 2, 1), 'sleep', 1],
            [datetime(2001, 3, 1), 'all', 1],
            [datetime(2001, 7, 1), 'night', 1],
            [datetime(2001, 8, 1), 'and', 1],
            [datetime(2001, 11, 1), 'work', 1],
            [datetime(2002, 3, 1), 'all', 1],
            [datetime(2002, 4, 1), 'day.', 1]
        ]
        return DataFrame(data, columns=cols)

    @property
    def filter(self):
        return [(datetime(2000, 1, 1), datetime(2001, 1, 1)), (datetime(2001, 1, 1), datetime(2001, 11, 2)),
                (datetime(1999, 1, 1), datetime(2001, 7, 31)), (datetime(2002, 1, 1), datetime(2003, 1, 1))]

    @property
    def filter_empty(self):
        return datetime(2018, 12, 1), datetime(2020, 12, 31)


class ExpectedTrend:
    @classmethod
    def __sorter(cls, data):
        return sorted(data, key=lambda x: x[0])

    @property
    def filter(self):
        cols = [Ak.Product, Ak.Date, Ak.Quantity]
        data = [
            ['I\'m', datetime(2000, 1, 1), 1],
            ['a', datetime(2000, 2, 1), 1],
            ['lumberjack', datetime(2000, 6, 1), 1],
            ['and', datetime(2000, 7, 1), 1],
            ['I\'m', datetime(2000, 8, 1), 1],
            ['okay.', datetime(2000, 11, 1), 1],
            ['I', datetime(2000, 12, 1), 1],
            ['sleep', datetime(2001, 2, 1), 1],
            ['all', datetime(2001, 3, 1), 1],
            ['night', datetime(2001, 7, 1), 1],
            ['and', datetime(2001, 8, 1), 1],
            ['work', datetime(2001, 11, 1), 1],
            ['all', datetime(2002, 3, 1), 1],
            ['day.', datetime(2002, 4, 1), 1]
        ]
        return [DataFrame(self.__sorter(data[:7]), columns=cols), DataFrame(self.__sorter(data[7:12]), columns=cols),
                DataFrame(self.__sorter(data[:10]), columns=cols), DataFrame(self.__sorter(data[12:]), columns=cols)]


class ArgumentBase:
    @property
    def init_data(self):
        return AbstractCustomerClassification, 'Data', 'People'

    @property
    def cart_kwargs(self):
        return {"min_support": 0, "min_confidence": 1, "max_length": 2, "verbosity": 0}

    @property
    def trend_kwargs(self):
        return {'from_date': datetime(1970, 5, 7), 'to_date': datetime(2020, 12, 30), "prd_num": 2}


class ExpectedBase:
    @property
    def data(self):
        return 'Data'

    @property
    def people(self):
        return 'People'

    @property
    def cart_data(self):
        return "Data <- mock"

    @property
    def profile_data(self):
        return

    @property
    def cart_kwargs(self):
        return {"min_support": 0, "min_confidence": 1, "max_length": 2, "verbosity": 0}

    @property
    def rule_class(self):
        return "Mock rule"

    @property
    def trend_kwargs(self):
        return {'from_date': datetime(1970, 5, 7), 'to_date': datetime(2020, 12, 30), "prd_num": 2}


class ArgumentRecommendation:
    class TestRule:
        def __init__(self, keys, conf):
            self.__rhs = keys
            self.__conf = conf

        @property
        def rhs(self):
            return self.__rhs

        @property
        def confidence(self):
            return self.__conf

    recommend = None

    @property
    def flat_rules(self):
        return [self.TestRule(['C'], 2), self.TestRule(['A', 'B'], 1)]

    @property
    def flat_profile(self):
        return [{'A': (1, 0.1), 'B': (1, 0.5), 'C': (2, 2.5)}], \
               [{'A': (0.6, 1), 'F': (0.3, 2)}, {'B': (0.4, 0.5), 'A': (0.3, 1), 'D': (0.1, 3)}]

    @property
    def recommend_score(self):
        args0 = {
            'flatten_part': [('A', 0.4), ('B', 0.3), ('C', 0.28)],
            'selected': {},
            'trend': {'E': 0.9, 'D': 0.6, 'B': 0.1, 'A': 0.05},
            'profiles': [{'A': (0.6, 1), 'F': (0.3, 2)}, {'B': (0.4, 0.5), 'A': (0.3, 1), 'D': (0.1, 3)}]
        }
        args1 = {
            'flatten_part': [('A', 0.6), ('B', 0.4), ('F', 0.3), ('A', 0.3), ('D', 0.1)],
            'selected': {'A': 1.05, 'B': 0.8},
            'trend': {'E': 0.9, 'D': 0.6, 'B': 0.1, 'A': 0.05}
        }
        args2 = {
            'flatten_part': [('E', 0.9), ('D', 0.6), ('B', 0.1), ('A', 0.05)],
            'selected': {'A': 1.05, 'B': 0.8, 'F': 0.3, 'D': 0.7}
        }
        return [args0, args1, args2]

    @staticmethod
    def probabilities(product):
        rule_data = {
            None: [],
            'C':  [ArgumentRecommendation.TestRule(['A'], 0.8), ArgumentRecommendation.TestRule(['B'], 0.75),
                   ArgumentRecommendation.TestRule(['A', 'B'], 0.6)],
            'B': [ArgumentRecommendation.TestRule(['C'], 0.78), ArgumentRecommendation.TestRule(['A'], 0.6)]
        }
        return {'Rule': rule_data[product],
                'Customer': [{'C': (0.6, 1), 'F': (0.3, 2)}, {'B': (0.4, 0.5), 'A': (0.3, 1), 'D': (0.1, 3)}],
                'Trend': {'E': 0.9, 'C': 0.6, 'B': 0.1, 'A': 0.05}}

    @property
    def call(self):
        return [None, 'C', 'B']

    @classmethod
    def this(cls):
        if cls.recommend is None:
            cls.recommend = PersonalRecommend(MagicMock(side_effect=ArgumentRecommendation.probabilities), 1)
        return cls.recommend


class ExpectedRecommendation:
    @property
    def flat_rules(self):
        return [('C', 2), ('A', 1), ('B', 1)]

    @property
    def flat_profile(self):
        return [('C', 2), ('A', 1), ('B', 1)], [('A', 0.6), ('B', 0.4), ('F', 0.3), ('A', 0.3), ('D', 0.1)]

    @property
    def recommend_score(self):
        return [{'A': 1.05, 'B': 0.8}, {'D': 0.7, 'F': 0.3}, {'E': 0.9}]

    @property
    def call(self):
        return ['C'], ['B'], ['A']


class ArgumentPortfolio:
    class MockTrend:
        trend_data = {
            'data': {
                "Increasing product0": [0.3],
                "Decreasing product0": [0.7],
                "Increasing product1": [0.4],
                "Decreasing product1": [0.6],
                "Non_creasing product": [0.5]
            },
            'prediction': {
                "Increasing product0": [0.4, 0.25, 0.5],
                "Decreasing product0": [0.6, 0.8, 0.68],
                "Increasing product1": [0.41, 0.42, 0.45],
                "Decreasing product1": [0.59, 0.55, 0.50],
                "Non_creasing product": [0.45, 0.5, 0.3]
            }
        }
        @property
        def data(self):
            return self.trend_data['data']

        @property
        def probability(self):
            return self.trend_data['prediction']

    @property
    def trends(self):
        return [self.MockTrend()]

    @property
    def product(self):
        return self.MockTrend.trend_data['data'].keys()


class ExpectedPortfolio:
    @property
    def increase(self):
        return [True, False, True, False, False]

    @property
    def increase_value(self):
        v = []
        data = ArgumentPortfolio.MockTrend.trend_data['data']
        predictions = ArgumentPortfolio.MockTrend.trend_data['prediction']
        for key in ['Increasing product0', 'Increasing product1']:
            v.append(round(sum([prd - data[key][0] for prd in predictions[key]]) / len(predictions[key]), 3))
        return [v[0], None, v[1], None, None]

    @property
    def decrease(self):
        return [False, True, False, True, False]

    @property
    def decrease_value(self):
        v = []
        data = ArgumentPortfolio.MockTrend.trend_data['data']
        predictions = ArgumentPortfolio.MockTrend.trend_data['prediction']
        for key in ['Decreasing product0', 'Decreasing product1']:
            v.append(round(sum([prd - data[key][0] for prd in predictions[key]]) / len(predictions[key]), 3))
        return [None, v[0], None, v[1], None]

    @property
    def stagnating(self):
        return [False, False, False, False, True]

from DataHandler.DataProjection import data_for_trend_analysis
from Analysis.ProbabilityCalculator.TrendClass import TrendClass
from collections import Counter
from datetime import datetime


class Portfolio:
    def __init__(self, data_transform_class, data, **kwargs):
        order = data_transform_class.order_transform(data)
        access_key = []
        trend_args = {
            'from_date': datetime(1990, 1, 1),
            'to_date': datetime.now(),
            'prd_num': 1
        }
        for k, v in kwargs.items():
            if k == 'access_key':
                access_key.append(v)
            elif k in trend_args.keys():
                trend_args[k] = v
        self.__trend = TrendClass(data_for_trend_analysis(order, *access_key), **trend_args)

    def __get_data_for_category(self, category):
        data = self.__trend.data[category]
        return data[-1], self.__trend.probability[category]

    def is_increasing(self, category):
        last_data, prediction = self.__get_data_for_category(category)
        diff = [x - last_data for x in prediction]
        mean = sum(diff)/len(diff)
        if all([x > 0 for x in diff]):
            return True, round(mean, 3)
        count = Counter([x > 0 and abs(x) > abs(mean) for x in diff])
        return count.most_common(1)[0][0], round(mean, 3) if count.most_common(1)[0][0] else None

    def is_decreasing(self, category):
        last_data, prediction = self.__get_data_for_category(category)
        diff = [x - last_data for x in prediction]
        mean = sum(diff)/len(diff)
        if all([x < 0 for x in diff]):
            return True, round(mean, 3)
        count = Counter([x < 0 and abs(x) > abs(mean) for x in diff])
        return count.most_common(1)[0][0], round(mean, 3) if count.most_common(1)[0][0] else None

    def is_stagnating(self, category):
        return not self.is_increasing(category)[0] and not self.is_decreasing(category)[0]

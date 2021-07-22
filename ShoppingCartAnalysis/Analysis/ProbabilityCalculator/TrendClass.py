import warnings
from datetime import datetime

from DataHandler.AccessKey import AccessKey as Ak
from statsmodels.tsa.holtwinters import ExponentialSmoothing

warnings.filterwarnings("ignore")


class TrendClass:
    __slots__ = {'_trends', '_prediction'}

    def __init__(self, sale_history, **kwargs):
        self._trends = {}
        self._prediction = {}
        if sale_history is not None:
            self._all_trend_from_to(sale_history, **kwargs)

    @staticmethod
    def filter_data(sale_history, from_date, to_date):
        if sale_history.empty:
            return sale_history[[Ak.Product, Ak.Date, Ak.Quantity]]
        filtered = sale_history[(sale_history[Ak.Date] >= from_date) & (sale_history[Ak.Date] < to_date)]
        return filtered.groupby([Ak.Product, Ak.Date]).sum().reset_index()

    def _all_trend_from_to(self, sale_history, from_date, to_date=datetime.now(), data_max=None, period=12):
        f_data = self.filter_data(sale_history, from_date, to_date)
        for category in f_data[Ak.Product].unique():
            dm = f_data[f_data[Ak.Product] == category][Ak.Quantity].max() if data_max is None else data_max
            data = (f_data[f_data[Ak.Product] == category][Ak.Quantity] / dm).tolist()
            if len(data) < period*2:
                continue
            model = ExponentialSmoothing(data, trend='add', seasonal='add', seasonal_periods=period)
            predicted = model.fit().predict(len(data), len(data))

            self._prediction[category] = predicted[0]
            self._trends[category] = data

    @property
    def data(self):
        return self._trends

    @property
    def probability(self):
        return {k: v for k, v in sorted(self._prediction.items(), key=lambda x: x[1], reverse=True)}

    def to_dict(self):
        return {key: self.__getattribute__(key) for key in self.__slots__}

    @classmethod
    def from_dict(cls, d):
        ret = cls(None)
        for key in cls.__slots__:
            ret.__setattr__(key, d[key])
        return ret

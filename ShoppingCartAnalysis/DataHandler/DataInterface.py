from DataHandler.AccessKey import AccessKey as Ak

from pandas import DataFrame
from abc import ABC, abstractmethod


class DataTransformInterface(ABC):
    __order_columns = {Ak.Order_ID, Ak.Date, Ak.Customer_ID, Ak.Category, Ak.Product, Ak.Quantity, Ak.Price}
    __ppl_columns = {Ak.Customer_ID, Ak.Age, Ak.Gender}

    @classmethod
    @abstractmethod
    def _order_transform(cls, records):
        raise NotImplementedError("This is an abstract class, and this method is not implemented.")

    @classmethod
    @abstractmethod
    def _people_transform(cls, records):
        raise NotImplementedError("This is an abstract class, and this method is not implemented.")

    @classmethod
    def order_transform(cls, records):
        orders = cls._order_transform(records)
        if not cls.order_validator(orders):
            raise AttributeError("Order DataFrame must have these columns tags from AccessKey enum class: %s" %
                                 ([*cls.__order_columns]))
        return orders

    @classmethod
    def people_transform(cls, records):
        ppl = cls._people_transform(records)
        if not cls.people_validator(ppl):
            raise AttributeError("People DataFrame must have these columns tags from AccessKey enum class: %s" %
                                 ([*cls.__ppl_columns]))
        return ppl

    @classmethod
    def order_validator(cls, df):
        if type(df) is not DataFrame:
            raise ValueError("Order data type must be DataFrame!")
        return all([key in df.columns for key in cls.__order_columns])

    @classmethod
    def people_validator(cls, df):
        if type(df) is not DataFrame:
            raise ValueError("People data type must be DataFrame!")
        return all([key in df.columns for key in cls.__ppl_columns])

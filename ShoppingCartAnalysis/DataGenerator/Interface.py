from pandas import DataFrame

from ModelClasses.Customer import Attribute as At
from DataHandler.AccessKey import AccessKey as Ak
from DataHandler.DataInterface import DataTransformInterface


class GeneratorDataTransform(DataTransformInterface):
    @classmethod
    def _order_transform(cls, records):
        orders = DataFrame(
            [[id(r), r.date, id(r.customer), prod.category, prod, 1, 0] for r in records for prod in r.product],
            columns=[Ak.Order_ID, Ak.Date, Ak.Customer_ID, Ak.Category, Ak.Product, Ak.Quantity, Ak.Price])
        return orders

    @classmethod
    def _people_transform(cls, records):
        people = DataFrame(
            [[id(order.customer),
              order.customer.age,
              order.customer.sex,
              order.customer.married,
              At.Has_child if order.customer.has_attribute(At.Has_child) else None,
              At.Has_pet if order.customer.has_attribute(At.Has_pet) else None,
              At.Workaholic if order.customer.has_attribute(At.Workaholic) else None]
             for order in records],
            columns=[Ak.Customer_ID, Ak.Age, Ak.Gender, Ak.Married, Ak.Has_child, Ak.Has_pet, Ak.Workaholic])
        return people

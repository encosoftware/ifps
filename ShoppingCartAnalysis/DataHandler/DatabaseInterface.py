from pandas import DataFrame
from datetime import datetime

from DataHandler.AccessKey import AccessKey as Ak
from DataHandler.DataInterface import DataTransformInterface
from ModelClasses.Customer import Gender, Attribute as At, Married


class DatabaseDataTransform(DataTransformInterface):
    @classmethod
    def _order_transform(cls, records):
        orders = DataFrame(
            [[id(r), datetime.strptime(r['deadline'], '%Y-%m-%dT%H:%M:%S'),
              id(r['customer']), 'kitchen', prod['code'], 1, prod['currentPrice']['value']]
             for r in records for prod in [*r['furnitureUnits'], *r['orderedApplianceMaterials']]],
            columns=[Ak.Order_ID, Ak.Date, Ak.Customer_ID, Ak.Category, Ak.Product, Ak.Quantity, Ak.Price]
        )
        return orders

    @classmethod
    def _people_transform(cls, records):
        people = DataFrame(
            [[id(r['customer']),
              r['customer']['age'],
              Gender.Female if r['customer']['gender']['gender'] == 'Female' else Gender.Male,
              Married.Married if r['customer']['isMarried'] == 1 else Married.Single,
              At.Has_child if r['customer']['hasChildren'] == 1 else None,
              None,
              None]
             for r in records],
            columns=[Ak.Customer_ID, Ak.Age, Ak.Gender, Ak.Married, Ak.Has_child, Ak.Has_pet, Ak.Workaholic]
        )
        return people

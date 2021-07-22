from random import Random

import pandas as pd
from pandas import DataFrame

from ModelClasses.Customer import Gender
from DataHandler.ColumnKeys import Key
from DataHandler.DataInterface import DataTransformInterface


class GlobalSalesDataTransform(DataTransformInterface):
    @classmethod
    def _order_transform(cls, records):
        order = get_order_data(records)
        return order

    @classmethod
    def _people_transform(cls, records):
        ppl = fake_personal_data(records)
        return ppl


def get_order_data(path):
    df = pd.read_excel(path, 'Orders')
    return translate_column_key(df)


def fake_personal_data(path):
    df = generate_fake_persons_from_id(pd.read_excel(path, 'Orders')[Key.Customer_ID.value].unique())
    return translate_column_key(df)


def translate_column_key(df):
    df.columns = [Key(column)() for column in df.columns]
    return df


def generate_fake_persons_from_id(ids):
    rnd = Random()
    age = [rnd.randint(18, 39) + rnd.randint(0, 40) for _ in range(len(ids))]
    gender = [Gender(rnd.randint(0, 1)) for _ in range(len(ids))]
    return DataFrame({Key.Customer_ID: ids, Key.Age: age, Key.Gender: gender})

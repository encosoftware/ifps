from DataHandler.AccessKey import AccessKey as Ak
from ModelClasses.Customer import Age
import pandas as pd


def data_for_customer_analysis(raw_order, raw_people):
    df = pd.merge(raw_order, raw_people, left_on=Ak.Customer_ID, right_on=Ak.Customer_ID, how='left')
    df[Ak.Price] = df[Ak.Price] / df[Ak.Quantity]
    df[Ak.Age] = df[Ak.Age].apply(lambda x: Age.get(x))

    cols = [*[key for key in raw_people.columns if key is not Ak.Customer_ID], Ak.Product]
    return df.groupby(cols, as_index=False).agg({Ak.Quantity: 'sum', Ak.Price: 'mean'})


def data_for_trend_analysis(raw_data, access_key=Ak.Product):
    projection = raw_data[[access_key, Ak.Date, Ak.Quantity]]
    projection.columns = [Ak.Product if column == access_key else column for column in projection.columns]
    projection.index = projection[Ak.Date].apply(lambda x: x.replace(day=1))
    return projection.groupby(by=[projection.index, Ak.Product]).sum().reset_index()


def data_for_cart_analysis(raw_data):
    data = []
    for order_id in raw_data[Ak.Order_ID].unique():
        data.append(list(raw_data[raw_data[Ak.Order_ID] == order_id][Ak.Product]))
    record = []
    for row in pd.DataFrame(data).values:
        record.append(tuple([columns for columns in row if columns is not None]))
    return record

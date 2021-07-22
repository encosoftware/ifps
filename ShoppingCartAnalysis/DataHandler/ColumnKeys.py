from enum import Enum
from DataHandler.AccessKey import AccessKey


class Key(Enum):
    Row_ID = 'Row ID'
    Order_ID = 'Order ID'
    Order_Date = 'Order Date'
    Ship_Date = 'Ship Date'
    Ship_Mode = 'Ship Mode'
    Customer_ID = 'Customer ID'
    Customer_Name = 'Customer Name'
    Segment = 'Segment'
    Postal_Code = 'Postal Code'
    City = 'City'
    State = 'State'
    Country = 'Country'
    Region = 'Region'
    Market = 'Market'
    Product_ID = 'Product ID'
    Category = 'Category'
    Sub_Category = 'Sub-Category'
    Product_Name = 'Product Name'
    Sales = 'Sales'
    Quantity = 'Quantity'
    Discount = 'Discount'
    Profit = 'Profit'
    Shipping_Cost = 'Shipping Cost'
    Order_Priority = 'Order Priority'
    Age = 'Age'
    Gender = 'Gender'

    def __call__(self):
        switch = {
            Key.Category: AccessKey.Category,
            Key.Order_ID: AccessKey.Order_ID,
            Key.Order_Date: AccessKey.Date,
            Key.Customer_ID: AccessKey.Customer_ID,
            Key.Sub_Category: AccessKey.Product,
            Key.Quantity: AccessKey.Quantity,
            Key.Sales: AccessKey.Price,
            Key.Age: AccessKey.Age,
            Key.Gender: AccessKey.Gender
        }
        return switch[self] if self in switch.keys() else self

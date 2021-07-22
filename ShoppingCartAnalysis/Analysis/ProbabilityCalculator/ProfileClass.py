from DataHandler.AccessKey import AccessKey as Ak
import pandas as pd

from ModelClasses.Customer import Customer


class ProfileClass:
    def __init__(self, customer, habit: pd.DataFrame):
        self.__customer = customer
        self.__habits = habit  # Sub-Category | Sales | Quantity
        self.__sum_quantity = 0 if habit is None else habit[Ak.Quantity].sum()

    def customer_is_in(self, customer):
        if not ((customer.age == self.__customer.age) and (customer.married == self.__customer.married)
                and (customer.sex == self.__customer.sex)):
            return False
        return all([attr in customer.attributes for attr in self.__customer.attributes]) or all(
            [attr in self.__customer.attributes for attr in customer.attributes])

    @property
    def customer(self):
        return self.__customer

    def probability_list(self):
        rows = self.__habits.sort_values(Ak.Quantity, ascending=False)
        probability = (rows[Ak.Quantity] / self.__sum_quantity).tolist()
        avg_spend = rows[Ak.Price].tolist()
        return {k: (p, s) for k, p, s in zip(rows[Ak.Product].tolist(), probability, avg_spend)}

    @property
    def probability(self):
        return self.probability_list()

    def to_dict(self):
        ret = pd.DataFrame(self.__habits.values, columns=[c.value for c in self.__habits.columns])
        print(self.__habits)
        return {'customer': self.__customer.to_dict(), 'habits': ret.to_dict()}

    @classmethod
    def from_dict(cls, d):
        return cls(Customer.from_dict(d['customer']),
                   pd.DataFrame(d['habits'], columns=[Ak(k) for k in d['habits'].keys()]))

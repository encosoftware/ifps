class Order:
    def __init__(self, time, customer, product):
        self.__date = time
        self.__customer = customer
        self.__product = product

    def __repr__(self):
        return "%s;;%s;;%s" % (self.__date.date(), self.__customer, self.__product)

    def __str__(self):
        return self.__repr__()

    @property
    def product(self):
        return self.__product

    @property
    def date(self):
        return self.__date

    @property
    def customer(self):
        return self.__customer

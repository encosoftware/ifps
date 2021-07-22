from enum import Enum


class ProductPropertyType(Enum):
    Material = 0
    Size = 1
    Color = 2


class Product:
    def __init__(self, name, category):
        self.__name = name
        self.__category = category

    def __eq__(self, other):
        return self.name == other.name and self.category == other.category

    def __gt__(self, other):
        return self.name > other.name

    def __lt__(self, other):
        return self.name < other.name

    def __ge__(self, other):
        return self.name >= other.name

    def __le__(self, other):
        return self.name <= other.name

    def __hash__(self):
        return hash(self.name + self.category)

    @property
    def name(self):
        return self.__name

    @property
    def category(self):
        return self.__category


class ProductDescriptor(Product):
    def __init__(self, name, category, property_type):
        super().__init__(name, category)
        self.__property_type = property_type

    def __repr__(self):
        ret = "%s :: %s --> " % (self.category, self.__name)
        for ind, prop in enumerate(self.__property_type):
            ret += "%s, " % prop.name
        return ret[:-2]

    def __str__(self):
        return self.__repr__()

    @property
    def property_types(self):
        return self.__property_type


class ActualProduct(Product):
    def __init__(self, name, category, properties):
        super().__init__(name, category)
        self.__all_property = properties

    def __repr__(self):
        ret = ">"
        for ind, prop in enumerate([self.name, self.category, *self.__all_property]):
            ret += "%s " % prop
        return ret[:-1] + '<'

    def __str__(self):
        return self.__repr__()

    @property
    def attributes(self):
        return self.__all_property

    def to_dict(self):
        return id(self)  # self.name, self.category, tuple(self.__all_property)

    @classmethod
    def from_dict(cls, d):
        return cls(d[0], d[1], d[2])


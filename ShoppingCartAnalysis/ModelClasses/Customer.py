from enum import Enum


class AnonymousCustomer:
    __slots__ = {'__age', '__sex', '__married', '__attribute'}

    def __init__(self):
        self.__age = 0
        self.__married = Married.Unknown
        self.__sex = Gender.Unknown
        self.__attribute = set()

    def __eq__(self, other):
        return self.all_property == other.all_property

    def __repr__(self):
        other = [o.name for o in self.attributes]
        return "Age: %s; Sex: %s; Status: %s; Other: %s" % (self.age, self.sex.name, self.married.name, other)

    def __str__(self):
        return self.__repr__()

    def has_attribute(self, attribute):
        return attribute in self.all_property

    def _set_all(self, attr):
        switch = {Gender: 'sex', Married: 'married',  Age: 'age', int: 'age', float: 'age'}
        for at in attr:
            key = type(at)
            if key is Attribute:
                self._add_to_attributes(at)
            else:
                self.__setattr__(switch[key], at)

    def _add_to_attributes(self, val):
        self.__attribute.add(val)

    @property
    def age(self):
        return self.__age

    @property
    def sex(self):
        return self.__sex

    @property
    def married(self):
        return self.__married

    @property
    def attributes(self):
        return self.__attribute

    @age.setter
    def age(self, val):
        self.__age = val

    @sex.setter
    def sex(self, val):
        self.__sex = val

    @married.setter
    def married(self, val):
        self.__married = val

    @attributes.setter
    def attributes(self, val):
        self.__attribute = val

    @property
    def all_property(self):
        return {self.age, self.sex, self.married, *self.attributes}


class Customer(AnonymousCustomer):
    def __init__(self, attr):
        super().__init__()
        self._set_all(attr)

    def to_dict_deprecated(self):
        return {"age": self.age, "genderId": 2 if self.sex is Gender.Female else 1,
                "isMarried": self.married is Married.Married, "hasChildren": Attribute.Has_child in self.attributes}

    def to_dict(self):
        return {"age": self.age, "gender": {'gender': 'Female'}, "isMarried": int(self.married is Married.Married),
                "hasChildren": int(Attribute.Has_child in self.attributes)}

    @classmethod
    def from_dict_deprecated(cls, d):
        attr = [Attribute.Has_child] if d['hasChildren'] else []
        return cls([d['age'], Gender(0 if d['genderId'] == 2 else 1),
                    Married.Married if d['isMarried'] else Married.Single, *attr])

    @classmethod
    def from_dict(cls, d):
        attr = [Attribute.Has_child] if d['hasChildren'] else []
        return cls([d['age'], Gender.Female if d['gender']['gender'] == 'Female' else Gender.Male,
                    Married.Married if d['isMarried'] == 1 else Married.Single, *attr])


class ComparableEnum(Enum):
    def __lt__(self, other):
        return self.value < other.value

    def __le__(self, other):
        return self.value <= other.value

    def __ge__(self, other):
        return self.value > other.value

    def __gt__(self, other):
        return self.value >= other.value


class Age(ComparableEnum):
    Young = 0
    Middle = 1
    Adult = 2
    Elder = 3

    @staticmethod
    def get(v):
        return Age.Young if v <= 30 else Age.Middle if v < 50 else Age.Adult if v <= 65 else Age.Elder


class Gender(ComparableEnum):
    Unknown = -1
    Female = 0
    Male = 1


class Married(ComparableEnum):
    Unknown = -1
    Married = 0
    Single = 1


class Attribute(ComparableEnum):
    Has_child = 0
    Has_pet = 1
    Workaholic = 2

from DataHandler.AccessKey import AccessKey as Ak
from ModelClasses.Customer import Customer, Age, Gender, Married, Attribute

from abc import ABC, abstractmethod


class AbstractCustomerClassification(ABC):
    @classmethod
    @abstractmethod
    def make_classes(cls, orders):
        raise NotImplementedError("This method is not implemented, you need to implement it.")


class AgeGenderClass(AbstractCustomerClassification):
    @classmethod
    def make_classes(cls, orders):
        profile_data = []
        for age in Age:
            for gender in Gender:
                cmr = Customer({age, gender})
                habit = orders.where((orders[Ak.Age] == age) & (orders[Ak.Gender] == gender)).dropna()
                profile_data.append((cmr, habit[[Ak.Product, Ak.Price, Ak.Quantity]]))
        return profile_data


class ComplexCustomerClass(AbstractCustomerClassification):
    @classmethod
    def make_classes(cls, orders):
        profile_data = []
        for age in Age:
            for gender in Gender:
                if gender is Gender.Unknown:
                    continue
                for married in Married:
                    if married is Married.Unknown:
                        continue
                    attr_list = []
                    for attr in Attribute:
                        cmr = Customer({age, gender, married, attr})
                        key = {Attribute.Has_child: Ak.Has_child, Attribute.Has_pet: Ak.Has_pet,
                               Attribute.Workaholic: Ak.Workaholic}[attr]
                        habit = orders.where((orders[Ak.Age] == age) & (orders[Ak.Gender] == gender) &
                                             (orders[Ak.Married] == married) & (orders[key] == attr)).dropna()
                        attr_list.append((cmr, habit[[Ak.Product, Ak.Price, Ak.Quantity]]))
                    if any([h.empty for _, h in attr_list]):
                        cmr = Customer({age, gender, married})
                        habit = orders.where((orders[Ak.Age] == age) & (orders[Ak.Gender] == gender) &
                                             (orders[Ak.Married] == married)).dropna()
                        profile_data.append((cmr, habit[[Ak.Product, Ak.Price, Ak.Quantity]]))
                    else:
                        profile_data.extend(attr_list)
        return profile_data


class UniqueCustomerClass(AbstractCustomerClassification):
    __cluster_set = None
    @classmethod
    def make_classes(cls, orders):
        profile_data = []
        for cluster in cls.__cluster_set:
            cmr = Customer(*cluster)
            fil = None
            for attr in cluster:
                key = cls.get_key(attr)
                fil = (orders[key] == attr) & (True if fil is None else fil)
            habit = orders.where(fil)
            profile_data.append((cmr, habit[[Ak.Product, Ak.Price, Ak.Quantity]]))
        return profile_data

    @classmethod
    def get_key(cls, attr):
        if attr in Attribute:
            return {Attribute.Workaholic: Ak.Workaholic,
                    Attribute.Has_pet: Ak.Has_pet,
                    Attribute.Has_child: Ak.Has_child}[attr]
        elif attr in Gender:
            return Ak.Gender
        elif attr in Age:
            return Ak.Age
        return None

    @classmethod
    def set_classes(cls, classes):
        if cls.__cluster_set is None:
            cls.__cluster_set = set(classes)

    @classmethod
    def clear_classes(cls):
        cls.__cluster_set = None

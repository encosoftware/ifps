import json

from DataHandler.ApiClient import ApiClientClass
from DataGenerator.Distributions import days_random_distribution
from DataGenerator.Interface import GeneratorDataTransform
from DataGenerator.Generator import data_generation
from DataHandler.DatabaseInterface import DatabaseDataTransform
from ModelClasses.Product import ProductPropertyType, ProductDescriptor
from DataHandler.GetFromExcel import GlobalSalesDataTransform
from Recommendation.Recommend import Recommend
from ModelClasses.Customer import Customer, Age, Gender, Attribute, Married
from ModelClasses.Classificatory import AgeGenderClass, ComplexCustomerClass
from datetime import datetime
from pathlib import Path
from random import Random


def generate_data():
    property_all = {
        ProductPropertyType.Material: ['Cseresznye', 'Tölgy', 'Sima Fehér'],
        ProductPropertyType.Color: ['Fekete', 'Fehér', 'Vaj', 'Piros', 'Kék'],
        ProductPropertyType.Size: ['Kicsi', 'Mini', 'Közepes', 'Nagy']
    }

    products_groups = {
        "Konyha": [
            ProductDescriptor('Sütő', "Konyha", {ProductPropertyType.Size, ProductPropertyType.Color}),
            ProductDescriptor('Hűtő', "Konyha", {ProductPropertyType.Size, ProductPropertyType.Color}),
            ProductDescriptor('Főzőlap', "Konyha", {ProductPropertyType.Size}),
            ProductDescriptor('Mosogatógép', "Konyha", {ProductPropertyType.Size, ProductPropertyType.Color}),
            ProductDescriptor('Szekrény', "Konyha", {ProductPropertyType.Size, ProductPropertyType.Material})
        ],
        "Háló": [
            ProductDescriptor('Ágy', 'Háló', {ProductPropertyType.Size, ProductPropertyType.Material}),
            ProductDescriptor('Éjeli_szekrény', 'Háló', {ProductPropertyType.Size, ProductPropertyType.Material}),
            ProductDescriptor('Lámpa', 'Háló', {ProductPropertyType.Color})
        ]
    }

    years = {2015: 0.2, 2016: 0.25, 2017: 0.15, 2018: 0.1, 2019: 0.3}
    month = {1: 0.05, 2: 0.1, 3: 0.1, 4: 0.3, 5: 0.1, 6: 0.05, 7: 0.02, 8: 0.02, 9: 0.02, 10: 0.1, 11: 0.1, 12: 0.04}
    day = days_random_distribution([1.5, 0.2, 0.1], [4, 3, 2], 0.25, 1)

    number_of_records = 1000
    return data_generation(property_all, products_groups, years, month, day, number_of_records)


def type_1():
    apr_arg = {'min_support': 0.01,
               'min_confidence': 0.06}
    trend_arg = {'to_date': datetime(2015, 5, 1)}
    # path = Path("../data/Sample - Superstore.xls")
    path = Path("../data/Global Superstore Orders 2016.xlsx")
    prod = 'Table', 'Paper', 'Supplies', 'Art'
    rec = Recommend(GlobalSalesDataTransform, path, path, AgeGenderClass, **{**apr_arg, **trend_arg})
    return rec, prod, Customer({Age.Young, Gender.Female})


def type_2():
    records = generate_data()

    apr_arg = {'min_support': 0.2,
               'min_confidence': 0.6}
    trend_arg = {'from_date': datetime(2015, 1, 1)}

    rcd = Recommend(GeneratorDataTransform, records, records, ComplexCustomerClass, **{**apr_arg, **trend_arg})
    prod = []
    return rcd, prod, Customer({Age.Young, Gender.Female, Married.Married, Attribute.Has_pet})


def type_3(login_data):
    api_client = ApiClientClass(Path("../data/api_config.json"))
    api_client.login(login_data)
    records = api_client.get_order()
    api_client.logout()

    apr_arg = {'min_support': 0.2,
               'min_confidence': 0.6}
    trend_arg = {'from_date': datetime(2015, 1, 1)}
    rcd = Recommend(DatabaseDataTransform, records, records, ComplexCustomerClass, **{**apr_arg, **trend_arg})
    prod = []
    return rcd, prod, Customer({Age.Young, Gender.Female, Married.Married, Attribute.Has_pet})


if __name__ == "__main__":
    login_data = json.loads(json.dumps({"email": "enco@enco.hu", "password": "password", "rememberMe": True}))

    recommend, products, customer = type_3(login_data)
    c2 = Customer({Age.Adult, Gender.Male, Married.Single, Attribute.Workaholic})
    c3 = Customer({Age.Middle, Gender.Male, Married.Married, Attribute.Workaholic, Attribute.Has_pet})
    for c in [customer, c2, c3]:
        personal_recommend = recommend.new_customer(c)

        prd = None
        rnd = Random()
        result = []
        print(c)
        print("%"*100)
        for _ in range(3):
            a, b = personal_recommend() if prd is None else personal_recommend(prd)
            select = rnd.random() * sum(b)
            act = 0
            for p, v in zip(a, b):
                act += v
                if act >= select:
                    prd = p
                    break
            # prd = a[0]
            print(a)
            print(b)
            print(prd)
            print("."*100)
            result.append(prd)
        print("="*100)
        print()
        print(result)
        print()
        print("%"*100)
        print()

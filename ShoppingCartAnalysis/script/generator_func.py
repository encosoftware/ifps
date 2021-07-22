import numpy

from DataGenerator.Distributions import days_random_distribution
from ModelClasses.Classificatory import ComplexCustomerClass
from DataGenerator.Interface import GeneratorDataTransform as Gt, GeneratorDataTransform
from DataGenerator.Generator import data_generation
from Analysis.CalculetedProbability import Calculate

from matplotlib import pyplot as plt
from datetime import datetime

from ModelClasses.Product import ProductPropertyType, ProductDescriptor
from DataHandler import DataProjection, AccessKey


def plot_date_func(years_, month_, day_):
    plt.figure('years')
    plt.plot(list(years_.keys()), list(years_.values()))
    plt.figure('months')
    plt.plot(list(month_.keys()), list(month_.values()))
    plt.figure('days')
    plt.plot(list(day_.keys()), list(day_.values()))
    plt.show()


if __name__ == "__main__":

    # Parameters of the data generation >>>
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
            ProductDescriptor('Éjeli szekrény', 'Háló', {ProductPropertyType.Size, ProductPropertyType.Material}),
            ProductDescriptor('Lámpa', 'Háló', {ProductPropertyType.Color})
        ]
    }
    years = {2015: 0.2, 2016: 0.25, 2017: 0.15, 2018: 0.1, 2019: 0.3}
    month = {1: 0.05, 2: 0.1, 3: 0.11, 4: 0.17, 5: 0.1, 6: 0.07, 7: 0.04, 8: 0.02, 9: 0.08, 10: 0.1, 11: 0.12, 12: 0.04}
    peak_max = [4, 3, 2]
    peak_min = [1.5, 0.3, 0.1]
    day = days_random_distribution(peak_min, peak_max)
    print(sum(years.values()))
    print(sum(month.values()))
    print(sum(day.values()))
    plot_date_func(years, month, day)

    number_of_records = 10000

    # Parameters of the data generation <<<

    records = data_generation(property_all, products_groups, years, month, day, number_of_records)
    size = {}
    color = {}
    material = {}
    for r in records:
        for p in r.product:
            if p.name not in size.keys():
                size[p.name] = {k: 0 for k in ['Kicsi', 'Mini', 'Közepes', 'Nagy']}
                color[p.name] = {k: 0 for k in ['Fekete', 'Fehér', 'Vaj', 'Piros', 'Kék']}
                material[p.name] = {k: 0 for k in ['Cseresznye', 'Tölgy', 'Sima Fehér']}
            for at in p.attributes:
                if at in ['Kicsi', 'Mini', 'Közepes', 'Nagy']:
                    size[p.name][at] += 1
                elif at in ['Fekete', 'Fehér', 'Vaj', 'Piros', 'Kék']:
                    color[p.name][at] += 1
                elif at in ['Cseresznye', 'Tölgy', 'Sima Fehér']:
                    material[p.name][at] += 1

    for key in size.keys():
        print(key)
        print(size[key])
        print(color[key])
        print(material[key])
        print("/"*100)
    dates = {}
    for rec in records:
        if rec.date not in dates.keys():
            dates[rec.date] = 1
        else:
            dates[rec.date] += 1
    dates = {k: dates[k] for k in sorted(dates.keys())}
    plt.plot(list(dates.keys()), list(dates.values()))
    plt.show()
    """order = GeneratorDataTransform.order_transform(records)
    people = GeneratorDataTransform.people_transform(records)
    data = DataProjection.data_for_customer_analysis(order, people)
    print(data)
    a = ComplexCustomerClass.make_classes(data)
    for i, _ in a:
        print(i,_.empty)
    count = {'year': {k: 0 for k in range(2015, 2020)},
             'month': {k: 0 for k in range(1, 13)},
             'day': {k: 0 for k in range(1, 32)}
             }
    for rec in records:
        date = rec.date
        count['year'][date.year] += 1
        count['month'][date.month] += 1
        count['day'][date.day] += 1

    for key, val in count.items():
        print(key)
        d = sum(val.values())
        for k, v in val.items():
            print("%s: %s%s" % (k, round(v/d*100, 1) if d != 0 else 0, '%'))
        print("~"*100)

    count_name = {}
    count_size = {'Kicsi': 0, 'Mini': 0, 'Közepes': 0, 'Nagy': 0}
    count_color = {'Fekete': 0, 'Fehér': 0, 'Vaj': 0, 'Piros': 0, 'Kék': 0}
    count_material = {'Cseresznye': 0, 'Tölgy': 0, 'Sima Fehér': 0}
    for record in records:
        for product in record.product:
            if product.name not in count_name.keys():
                count_name[product.name] = 0
            count_name[product.name] += 1
            for a in product.attributes:
                if a in count_size.keys():
                    count_size[a] += 1
                elif a in count_color.keys():
                    count_color[a] += 1
                elif a in count_material.keys():
                    count_material[a] += 1

    dn = sum(count_name.values())
    dm = sum(count_material.values())
    ds = sum(count_size.values())
    dc = sum(count_color.values())
    for dict_, d in zip([count_name, count_material, count_size, count_color], [dn, dm, ds, dc]):
        for n, v in dict_.items():
            print("%s: %s%s" % (n, round(v/d*100, 1) if d != 0 else 0, '%'))
        print("%"*100)"""

    data = Gt.order_transform(records)
    people = Gt.people_transform(records)

    # print(people.head())
    """print(" '_' "*30)
    a_product = [product for rec in records for product in rec.product if product.name == 'Szekrény'][0]
    Calculate.calculate_profile(data=[data, people], product=a_product)
    print(" '_' "*30)
    Calculate.calculate_trends_year(a_product, 'r',
                                    data=data, limit=datetime(2019, 1, 1), valid_limit=datetime(2020, 1, 1))
    print(" '_' "*30)"""
    rules = Calculate.calculate_rules(data=data)
    for r in rules:
        print(r)

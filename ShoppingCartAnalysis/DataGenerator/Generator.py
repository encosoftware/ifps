from datetime import datetime
from random import Random
import numpy as np

from DataGenerator.ProductionHelper import ProductGroup, ProductSelector
from ModelClasses.Order import Order
from DataGenerator.Distributions import age_distribution
from ModelClasses.Customer import Customer, Gender, Married, Attribute


class AgeLessCluster:
    def __init__(self, attr):
        self.__attr = attr

    def is_in(self, ppl):
        for a in self.__attr:
            if not ppl.has_attribute(a):
                return False
        return True

    def __repr__(self):
        return "%s" % ([a.name for a in self.__attr])


class DateGenerator:
    def __init__(self, *args):
        self.__years, self.__months, self.__days = args

    def generate_date(self):
        rnd = Random()
        ymd = []
        for col, r in zip([self.__years, self.__months, self.__days], [rnd.random() for _ in range(3)]):
            act = 0
            for key, val in col.items():
                act += val
                if round(act, 2) >= r:
                    ymd.append(key)
                    break
        if len(ymd) != 3:
            print(ymd)
        while True:
            try:
                return datetime(*ymd)
            except ValueError:
                ymd[2] -= 1


class OrderGenerator:
    def __init__(self, customer_generator, date_generator, product_selector):
        self.__customer_gen = customer_generator
        self.__product_selector = product_selector
        self.__date_gen = date_generator

    def generate(self):
        date = self.__date_gen.generate_date()
        customer = gen_customer(1)[0]
        product = self.__product_selector.select(customer)
        return Order(date, customer, product)


def gen_customer(n):
    rnd = Random()
    ret = []
    age_dist = age_distribution(18, 80)
    for _ in range(n):
        score = rnd.random()
        pr = 0
        age = 0
        while score > pr:
            age = rnd.randint(18, 80)
            pr = age_dist[age]
            score -= 0.1
        sex = Gender(rnd.randint(0, 1))
        married = Married(rnd.randint(0, 1))
        attr = {Attribute(rnd.randint(0, 2)) for _ in range(rnd.randint(1, 3))}

        ret.append(Customer({age, sex, married, *attr}))
    return ret


def __generate_sample(group, people, clusters, number_of_sell, max_sell_unit):
    rnd = Random()
    statistic = {}
    sells_sum = {}
    for p in group:
        index = 0
        limit = number_of_sell
        sells = []
        while limit != 0:
            ppl = people[index]
            index += 1 if index < (len(people) - 1) else -index
            number = rnd.randint(0, max_sell_unit)
            if number > limit:
                number = limit
            limit -= number
            sells.append((number, ppl))

        vsm = {key: 0 for key in clusters}
        for number, ppl in sells:
            for cl in clusters:
                vsm[cl] += number if cl.is_in(ppl) else 0

        statistic[p] = vsm
        sells_sum[p] = sum(vsm.values())
    return statistic, sells_sum


def gen_attribute_probability(attributes, people, clusters, number_of_sell=100, max_sell_unit=10):
    statistic, _ = __generate_sample(attributes, people, clusters, number_of_sell, max_sell_unit)
    sells_for_clusters = {cl: {attr: value[cl] for attr, value in statistic.items()} for cl in clusters}
    ret = {}
    for index, i in enumerate(sells_for_clusters.values()):
        div = sum([v for v in i.values()])
        ret[clusters[index]] = {k: round((v / div if div != 0 else 1 / len(i.values())), 2) for k, v in i.items()}
    return ret


def gen_product_group_probability(group, people, clusters, number_of_sell=100, max_sell_unit=10):
    statistic, max_sells = __generate_sample(group, people, clusters, number_of_sell, max_sell_unit)
    group_prob = {index: 0 for index in clusters}
    prod_dist = {}
    div = len(group) * number_of_sell
    item_list = list(statistic.items())
    for ind, item in enumerate(item_list[:-1]):
        product, value = item
        for index in clusters:
            group_prob[index] += value[index] / div
        prod_dist[product] = {}
        for other_prod, other_value in item_list[ind+1:]:
            array = np.power(np.array([*value.values()]) - np.array([*other_value.values()]), 2)
            weight = np.mean(array)/number_of_sell
            prod_dist[product][other_prod] = weight

    return statistic, group_prob, prod_dist, max_sells


def make_orders_from_customer_samples(customer_sample, clusters, all_property, products_groups,
                                      year_dtr, month_dtr, day_dtr):
    product_selector = ProductSelector(clusters)

    for p_type, prop in all_property.items():
        act_prop = gen_attribute_probability(prop, customer_sample, clusters, 100, 25)
        product_selector.add_property(act_prop, p_type)

    for group_name, products in products_groups.items():
        _, pr, dist, s_sum = gen_product_group_probability(products, customer_sample, clusters, 100)
        group = ProductGroup(s_sum)
        group.add_edge_from_dict(dist)
        product_selector.add_group(pr, group)
    return OrderGenerator(None, DateGenerator(year_dtr, month_dtr, day_dtr), product_selector)


def data_generation(properties, product_groups, years, months, days, record_number=10, population=4000, sample_size=20):
    # The people sample maker path >>>

    rnd = Random()

    index = rnd.randint(0, population-sample_size-1)
    people_sample = gen_customer(population)[index:index + sample_size]

    # The order generation part >>>

    clusters = []
    for sex in Gender:
        for status in Married:
            for other in [Attribute.Workaholic, Attribute.Has_pet, Attribute.Has_child]:
                clusters.append(AgeLessCluster({sex, status, other}))

    order = make_orders_from_customer_samples(people_sample, clusters, properties, product_groups, years, months, days)

    return [order.generate() for _ in range(record_number)]

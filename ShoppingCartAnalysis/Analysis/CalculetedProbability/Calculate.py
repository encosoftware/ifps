from DataHandler import DataProjection, GetFromExcel
from DataHandler.AccessKey import AccessKey as Ak
from Analysis.ProbabilityCalculator import ProfileClass, RuleClass, TrendClass
from ModelClasses.Customer import Customer, Age, Gender

from efficient_apriori import apriori

from random import Random
from datetime import datetime
from matplotlib import pyplot as plt, cm
from numpy import linspace


def random_shopping(basket_, prod_ls, n):
    rnd = Random()
    while True:
        prod = prod_ls[rnd.randint(0, n-1)]
        if prod not in basket_:
            basket_.append(prod)
            return prod


def calculate_trends(**kwargs):
    import warnings
    warnings.simplefilter(action='ignore', category=FutureWarning)
    if 'path' in kwargs.keys():
        pd_data = DataProjection.data_for_trend_analysis(GetFromExcel.get_order_data(kwargs['path']))
    elif 'data' in kwargs.keys():
        pd_data = DataProjection.data_for_trend_analysis(kwargs['data'])
    else:
        raise AttributeError('The data or the path is required.')
    st = datetime(2012, 1, 1)
    full = []
    for month in range(1, 13):
        ed = datetime(2016, 1, 1) if month == 12 else datetime(2015, month+1, 1)
        trend = TrendClass.TrendClass(pd_data, from_date=st, to_date=datetime(2015, month, 1), data_max=600)
        predicted = TrendClass.TrendClass(pd_data, from_date=st, to_date=ed, data_max=600)

        pd_next = []
        dot = []
        trends = trend.probability
        all_data = trend.data
        all_p_data = predicted.data

        plt.figure("trend_2015_%s" % month)
        plt.xlim(datetime(2012, 1, 1), datetime(2015, 12, 1))
        plt.ylim(0, 1.2)
        labels = [datetime(y, m, 1) for y in range(2012, 2015) for m in range(1, 13)]
        labels.extend([datetime(y, m, 1) for y in range(2015, 2016) for m in range(1, month)])

        color = cm.rainbow(linspace(0, 1, len(trends.keys())))
        for category, c in zip(trends.keys(), color):
            data = all_data[category]
            plt.plot(labels, data, color=c)

            p_data = all_p_data[category][-1:]

            hat = trends[category]
            pd_next.append(abs(hat[0]-p_data[0]))
            dot.append(([data[-1], hat[0]], p_data[0], c))

            # print(category, [round(x, 3) for x in hat], [round(abs(x - p), 3) for x, p in zip(hat, p_data)])

        full.append((round(sum(pd_next)/len(pd_next), 3), max(pd_next)))
        print(full[-1])
        print("." * 100)

        plt.legend(trends.keys())
        prev_date = datetime(2014, 12, 1) if month == 1 else datetime(2015, month - 1, 1)
        act_date = datetime(2015, month, 1)
        for d, v, c in dot:
            plt.plot([prev_date, act_date], d, color=c, ls='--')
            plt.plot(act_date, v, color=c, marker='o')
    plt.show()
    a = [f for f, _ in full]
    b = [f for _, f in full]
    print(max(a), sum(b)/len(b), max(b))


def calculate_trends_year(category, color, **kwargs):
    import warnings
    warnings.simplefilter(action='ignore', category=FutureWarning)
    if 'path' in kwargs.keys():
        pd_data = DataProjection.data_for_trend_analysis(GetFromExcel.get_order_data(kwargs['path']))
    elif 'data' in kwargs.keys():
        pd_data = DataProjection.data_for_trend_analysis(kwargs['data'])
    else:
        raise AttributeError('The data or the path is required.')
    valid_limit = kwargs['valid_limit'] if 'valid_limit' in kwargs.keys() else datetime.now()
    limit = kwargs['limit'] if 'limit' in kwargs.keys() \
        else datetime(valid_limit.year-1, valid_limit.month, valid_limit.day)
    st = datetime(2000, 1, 1)
    trend = TrendClass.TrendClass(pd_data, from_date=st, to_date=limit, prd_num=12)
    predicted = TrendClass.TrendClass(pd_data, from_date=st, to_date=valid_limit)

    trends = trend.probability
    all_data = trend.data
    all_p_data = predicted.data
    plt.figure("trend_2019")
    plt.xlim(datetime(2015, 1, 1), datetime(limit.year, 12, 1))
    plt.ylim(0, 1.2)
    labels = [datetime(y, m, 1) for y in range(2015, limit.year) for m in range(1, 13)]
    dot = []
    data = all_data[category]
    plt.plot(labels, data, color)

    p_data = all_p_data[category][-13:]
    hat = trends[category]
    dot.append(([data[-1], *hat], p_data, color))
    print(category, [round(x, 3) for x in hat])
    plt.legend('ABC')
    prd_labels = [datetime(limit.year-1, 12, 1),
                  *[datetime(y, m, 1) for y in range(limit.year, valid_limit.year) for m in range(1, 13)]]
    for d, v, c in dot:
        plt.plot(prd_labels, v, c, marker='o', linewidth=0.3)
        plt.plot(prd_labels, d, c + '--')
    plt.legend(['Data', 'Real', 'Predicted'])
    plt.show()


def calculate_profile(**kwargs):
    if 'path' in kwargs.keys():
        data0 = DataProjection.data_for_customer_analysis(GetFromExcel.get_order_data(kwargs['path']),
                                                          GetFromExcel.fake_personal_data(kwargs['path']))
    elif 'data' in kwargs.keys():
        data0 = DataProjection.data_for_customer_analysis(*kwargs['data'])
    else:
        raise AttributeError('The data or the path is required.')
    product = kwargs['product'] if 'product' in kwargs.keys() else 'Tables'

    print(data0.head(-1))
    tmp = data0.reset_index()
    tmp = tmp.where((tmp[Ak.Age] == Age.Adult) & (tmp[Ak.Gender] == Gender.Female)).dropna()
    print(tmp)
    print(tmp[Ak.Quantity].sum())
    habits = tmp
    print(habits[habits[Ak.Product] == product][Ak.Quantity])
    customer_profiles = []

    tmp = data0.reset_index()
    for age in Age:
        for gender in Gender:
            cmr = Customer({Ak.Age: age, Ak.Gender: gender})
            habit = tmp.where((tmp[Ak.Age] == age) & (tmp[Ak.Gender] == gender)).dropna()
            customer_profiles.append(ProfileClass.ProfileClass(cmr, habit[[Ak.Product, Ak.Price, Ak.Quantity]]))

    print("=" * 100)
    print(product)
    for cp in customer_profiles:
        a = cp.probability_list[product]
        print("%s -> %s; %s$" % (cp.customer.all_property, round(a[0], 3), round(a[1], 2)))

    print("=" * 100)
    print(customer_profiles[0].customer.all_property)
    for k, value in customer_profiles[0].probability_list().items():
        p, v = value
        print("%s: %s <- %s$" % (k, round(p, 3), round(v, 2)))


def calculate_rules(**kwargs):
    if 'path' in kwargs.keys():
        data = DataProjection.data_for_cart_analysis(GetFromExcel.get_order_data(kwargs['path']))
    elif 'data' in kwargs.keys():
        data = DataProjection.data_for_cart_analysis(kwargs['data'])
    else:
        raise AttributeError('The data or the path is required.')

    r_d, ass_rules = apriori(data, min_support=0.2, min_confidence=0.6)

    product_list = [key[0] for key in r_d[1].keys()]
    product_n = len(product_list)
    rule_class = RuleClass.CartRules(RuleClass.RuleClass(ass_rules))
    basket = []
    for _ in range(5):
        product = random_shopping(basket, product_list, product_n)
        rule_class(product)
        for r in rule_class.probability:
            print("%s :: %s -> %s" % (basket, r.lhs, r.rhs))
        print("=" * 100)
    print("END")
    return ass_rules

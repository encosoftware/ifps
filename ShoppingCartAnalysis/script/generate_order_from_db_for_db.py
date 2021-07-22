import json
import logging
import sys
from os import path
from random import Random
from pathlib import Path

sys.path.append(path.abspath('..'))

from DataHandler.ApiClient import ApiClientClass
from DataGenerator.Distributions import days_random_distribution
from DataGenerator.Generator import data_generation
from ModelClasses.Product import ProductPropertyType, ProductDescriptor


logger = logging.getLogger(__name__)
logger.setLevel(logging.INFO)

handler = logging.StreamHandler(sys.stdout)
handler.setFormatter(logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s'))
logger.addHandler(handler)


def product_data(json_path, group_number, number_per_group=8):
    with open(json_path, 'r') as json_file:
        group_data = json.load(json_file)
    property_types = {
        ProductPropertyType.Material: ['A', 'B', 'C', 'D'],
        ProductPropertyType.Size: ['Mini', 'Small', 'Medium', 'Large'],
        ProductPropertyType.Color: ['Black', 'White', 'Silver', 'Material']
    }
    rnd = Random()
    base = group_data['base']
    ovens = group_data['other']['Oven']
    sinks = group_data['other']['Water']
    finish = group_data['other']['Finish']
    corner = group_data['other']['45']

    oven_ma = group_data['machine']['Oven']
    frig = group_data['machine']['Refrigerator']
    micro = group_data['machine']['Microwave']
    dish = group_data['machine']['Dishwasher']

    products_groups = {}
    for ind in range(group_number):
        key = "Kitchen_%s" % ind
        b = [base[rnd.randint(0, len(base)-1)] for _ in range(number_per_group)]
        o = ovens[rnd.randint(0, len(ovens)-1)]
        s = sinks[rnd.randint(0, len(sinks)-1)]
        f = finish[rnd.randint(0, len(finish)-1)]
        if type(s) is list:
            prod_list = [*b, o, *s, *f]
        else:
            c = corner[rnd.randint(0, len(corner)-1)]
            prod_list = [*b, o, s, *f, *c]
        products_groups[key] = [ProductDescriptor(name, key, {ProductPropertyType.Material}) for name in prod_list]
        m_o = oven_ma[rnd.randint(0, len(oven_ma)-1)]
        m_r = frig[rnd.randint(0, len(frig)-1)]
        tmp = [ProductDescriptor(m_o, key, {ProductPropertyType.Size, ProductPropertyType.Color}),
               ProductDescriptor(m_r, key, {ProductPropertyType.Size, ProductPropertyType.Color})]
        if rnd.random() > 0.7:
            m_m = micro[rnd.randint(0, len(micro)-1)]
            tmp.append(ProductDescriptor(m_m, key, {ProductPropertyType.Size}))
        if rnd.random() > 0.9:
            m_d = dish[rnd.randint(0, len(dish)-1)]
            tmp.append(ProductDescriptor(m_d, key, {ProductPropertyType.Color}))
        products_groups[key].extend(tmp)
    return property_types, products_groups


def order_generate(json_path, number_of_records=10):
    properties, groups = product_data(json_path, 5)
    years = {2015: 0.2, 2016: 0.25, 2017: 0.15, 2018: 0.1, 2019: 0.3}
    month = {1: 0.05, 2: 0.1, 3: 0.11, 4: 0.17, 5: 0.1, 6: 0.07, 7: 0.04, 8: 0.02, 9: 0.08, 10: 0.1, 11: 0.12, 12: 0.04}

    peak_max = [4, 3, 2]
    peak_min = [1.5, 0.3, 0.1]

    day = days_random_distribution(peak_min, peak_max)

    return data_generation(properties, groups, years, month, day, number_of_records)


def make_lut(api):
    un_lut = {}
    for unit in api.get_furniture_unit():
        if unit['name'] in un_lut.keys():
            continue
        un_lut[unit['name']] = unit['id']

    ap_lut = {}
    for apl in api.get_appliance():
        if apl['name'] in ap_lut.keys():
            continue
        ap_lut[apl['name']] = apl['id']
    return un_lut, ap_lut


def upload_customer_and_records(api, un_lut, ap_lut):
    logger.info(f'Uploading customers and orders!')
    for index, order in enumerate(order_records):
        customer = order.customer
        val = api.post_customer(json.loads(json.dumps(customer.to_dict())))
        logger.info(f'{index}. customers is uploaded!')

        date = order.date
        product_id_list = []
        appliance_id_list = []
        for pr in order.product:
            if pr.name in un_lut.keys():
                product_id_list.append(un_lut[pr.name])
            elif pr.name in ap_lut.keys():
                appliance_id_list.append(ap_lut[pr.name])

        dto = {
            "orderName": "Generated %s" % index,
            "workingNumber": "string",
            "deadline": str(date),
            "customerId": val,
            "furnitureUnitIds": product_id_list,
            "applianceIds": appliance_id_list
        }
        api.post_order(json.loads(json.dumps(dto)))
        logger.info(f'{index}. order is uploaded!')


if __name__ == '__main__':
    group_json_path = Path("C:/Users/vamos.peter/Projects/Butor/Documentation/Documents/seed/groups.json")
    order_records = order_generate(group_json_path, 1000)

    api_client = ApiClientClass(Path("../data/api_config.json"))

    login_json = json.loads(json.dumps({"email": "enco@enco.hu", "password": "password", "rememberMe": True}))
    api_client.login(login_json)

    unit_lut, apl_lut = make_lut(api_client)

    upload_customer_and_records(api_client, unit_lut, apl_lut)

    api_client.logout()

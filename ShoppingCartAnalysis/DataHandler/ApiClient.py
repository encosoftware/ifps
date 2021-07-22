import json
import requests
import re
import pandas as pd

import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)


class ApiClientClass:
    def __init__(self, config_path):
        self.__verify = False
        with open(config_path, 'r') as json_file:
            self.__api_url = json.load(json_file)
        self.__access_token = None
        self.__tokens = None

    def login(self, login_json):
        login_api = self.__api_url['root'] + self.__api_url['login']
        head = {'Content-type': 'application/json-patch+json', 'Accept': 'application/json-patch+json'}

        response = self.__post(login_api, json=login_json, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Login: %s" % response.status_code)
        self.__tokens = json.loads(response.content)
        self.__access_token = self.__tokens['accessToken']

    def refresh(self):
        refresh_api = self.__api_url['root'] + self.__api_url['refresh']
        head = {'Content-type': 'application/json-patch+json', 'Accept': 'application/json-patch+json'}
        response = requests.post(refresh_api, json=self.__tokens, headers=head, verify=False)

        if response.status_code != 200:
            raise ConnectionError("Refresh: %s" % response.status_code)
        self.__tokens = json.loads(response.content)
        self.__access_token = self.__tokens['accessToken']

    def logout(self):
        logout_api = self.__api_url['root'] + self.__api_url['logout']
        logout_json = json.loads(json.dumps({"accessToken": self.__access_token}))
        head = {'Content-type': 'application/json', 'Accept': 'application/json',
                'Authorization':  f'Bearer {self.__access_token}'}

        response = requests.post(logout_api, json=logout_json, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Login: %s" % response.status_code)

    def __post(self, api_url, **kwargs):
        for tries in range(2):
            response = requests.post(api_url, **kwargs)
            if response.status_code == 401 and tries == 0:
                self.refresh()
            else:
                return response

    def __get(self, api_url, **kwargs):
        for tries in range(2):
            response = requests.get(api_url, **kwargs)
            if response.status_code == 401 and tries == 0:
                self.refresh()
                kwargs['headers']['Authorization'] = f"Bearer {self.__access_token}"
            else:
                return response

    def get_customers(self):
        customer_api = self.__api_url['root'] + self.__api_url['customer']
        head = {'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__get(customer_api, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Customer get: %s" % response.status_code)

        return json.loads(response.content)

    def post_customer(self, customer_json):
        customer_api = self.__api_url['root'] + self.__api_url['customer']
        head = {'Content-type': 'application/json', 'Accept': 'application/json',
                'Authorization': f"Bearer {self.__access_token}"}

        response = self.__post(customer_api, headers=head, json=customer_json, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Customer post: %s" % response.status_code)
        return json.loads(response.content)

    def get_furniture_unit(self):
        furniture_api = self.__api_url['root'] + self.__api_url['furniture_unit']['get']
        head = {'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__get(furniture_api, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Furniture get: %s" % response.status_code)
        return json.loads(response.content)

    def post_furniture_unit(self, furniture_json, csv_data):
        furniture_api = self.__api_url['root'] + self.__api_url['furniture_unit']['post']
        head = {'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__post(furniture_api, json=furniture_json, params=csv_data, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Furniture post: %s" % response.status_code)

    def get_furniture_component_by_unit_id(self, unit_id):
        furniture_api = self.__api_url['root'] + self.__api_url['furniture_component_by_unit_id']\
            .replace("{furnitureUnitId}", "%s" % unit_id)
        head = {'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__get(furniture_api, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Component get by unit id: %s" % response.status_code)
        return json.loads(response.content)

    def get_furniture_component(self, c_id):
        furniture_api = self.__api_url['root'] + self.__api_url['furniture_component'].replace("{id}", "%s" % c_id)
        head = {'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__get(furniture_api, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Component get: %s" % response.status_code)
        return json.loads(response.content)

    def get_material(self):
        material_api = self.__api_url['root'] + self.__api_url['material']['get']
        head = {'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__get(material_api, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Material get: %s" % response.status_code)
        return json.loads(response.content)

    def post_material(self, csv_dir_and_file_name):
        material_api = self.__api_url['root'] + self.__api_url['material']['post']
        head = {'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__post(material_api, params=csv_dir_and_file_name, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Material post: %s" % response.status_code)

    def get_image(self, image_data):
        not_implemented = True
        if not_implemented:
            return {}
        image_api = self.__api_url['root'] + self.__api_url['image']
        head = {'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__get(image_api, params=image_data, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Image get: %s" % response.status_code)
        return json.loads(response.content)

    def post_image(self, image_param, image_file):
        image_api = self.__api_url['root'] + self.__api_url['image']
        head = {'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__post(image_api, params=image_param,  files=image_file, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Image post: %s" % response.status_code)
        return json.loads(response.content)

    def get_appliance(self):
        appliance_api = self.__api_url['root'] + self.__api_url['appliance']['get']
        head = {'Content-type': 'application/json', 'Accept': 'application/json',
                'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__get(appliance_api, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Appliance get: %s" % response.status_code)
        return json.loads(response.content)

    def post_appliance(self, appliance_json):
        appliance_api = self.__api_url['root'] + self.__api_url['appliance']['post']
        head = {'Content-type': 'application/json', 'Accept': 'application/json',
                'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__post(appliance_api, json=appliance_json, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Appliance post: %s" % response.status_code)

    def get_order(self):
        order_api = self.__api_url['root'] + self.__api_url['order']
        head = {'Content-type': 'application/json', 'Accept': 'application/json',
                'Authorization': f'Bearer {self.__access_token}'}

        response = self.__get(order_api, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Order get: %s" % response.status_code)
        return json.loads(response.content)

    def post_order(self, order_json):
        order_api = self.__api_url['root'] + self.__api_url['order']
        head = {'Content-type': 'application/json', 'Accept': 'application/json',
                'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__post(order_api, json=order_json, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Order post: %s" % response.status_code)

    def post_drill(self, component_id, drill_json):
        drill_api = self.__api_url['root'] + self.__api_url['drill']
        head = {'Content-type': 'application/json', 'Accept': 'application/json',
                'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__post(drill_api, params=component_id, json=drill_json, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Drill post: %s" % response.status_code)

    def post_rectangle(self, component_id, rect_json):
        rect_api = self.__api_url['root'] + self.__api_url['rectangle']
        head = {'Content-type': 'application/json', 'Accept': 'application/json',
                'Authorization':  f'Bearer {self.__access_token}'}

        response = self.__post(rect_api, params=component_id, json=rect_json, headers=head, verify=self.__verify)
        if response.status_code != 200:
            raise ConnectionError("Rectangle post: %s" % response.status_code)


def furniture_unite_size(path):
    with open(path) as f:
        data = f.readlines()

    return {k.lower(): v for k, v in parse_size(data).items()}


def furniture_unite_price(path):
    data = pd.read_csv(path, sep=';')
    return (data.Qty * data.Price).sum()


def parse_size(data):
    ret_d = {}
    p = re.compile(r"""(?P<name>\w+):.?\((?P<height>\d+),.?(?P<width>\d+),.?(?P<depth>\d+)\).?""")
    for d in data:
        parsed = p.search(d)
        ret_d[parsed['name']] = {'height': parsed['height'], 'width': parsed['width'], 'depth': parsed['depth']}
    return ret_d


def appliance_data(path):
    with open(path / "apl_sizes.txt") as sizes:
        data = sizes.readlines()

    sizes = list(parse_size(data).items())

    with open(path / "apl_prices.txt") as prices:
        data = prices.readlines()

    ret = []
    p = re.compile(r"""[^_]+_(?P<brand>[^_]+)_?(?P<type_code>\w+)?:.?(?P<price>\d+)""")
    for index, line in enumerate(data):
        parsed = p.search(line)
        code, desc = sizes[index]
        ret.append({
            "description": str(desc),
            "brand": {"brandName": parsed['brand']},
            "code": code,
            "price": {"value": int(parsed['price']), 'currencyId': 2}
        })
    return ret


def parse_xxl(paths):
    if type(paths) is not list:
        return _parse_xxl(paths)
    ret_d = {'size': [], 'hole': [], 'rect': []}
    for ind, path in enumerate(paths):
        tmp = _parse_xxl(path)
        if ind == 0:
            ret_d = tmp
        else:
            for h in tmp['hole']:
                h['plane'] = 6
            ret_d['hole'].extend(tmp['hole'])
    return ret_d


def _parse_xxl(path):
    ret_d = {'size': [], 'hole': [], 'rect': []}
    head = re.compile(r"H DX=(?P<length>\d+)\.?(?P<length_>\d+)? DY=(?P<width>\d+)\.?(?P<width_>\d+)? "
                      r"DZ=(?P<thickness>\d+)\.?(?P<thickness_>\d+)?.*")

    hole = re.compile(r"XBO X=(?P<x>\d+)\.?(?P<x_>\d+)? Y=(?P<y>\d+)\.?(?P<y_>\d+)? Z=(?P<z>\d+)\.?(?P<z_>\d+)? "
                      r"F(?P<plane>\d).+D=(?P<diameter>\d+)\.?(?P<diameter_>\d+)?.?")

    nut = re.compile(r"LONG X=(?P<x0>\d+)\.?(?P<x0_>\d+)? Y=(?P<y0>\d+)\.?(?P<y0_>\d+)?.+x=(?P<x1>\d+)\.?(?P<x1_>\d+)? "
                     r"y=(?P<y1>\d+)\.?(?P<y1_>\d+)? Z=(?P<z>\d+)\.?(?P<z_>\d+)?.?")
    ret_keys = {0: 'hole', 1: 'rect', 2: 'size'}
    with open(path, 'r') as xxl_file:
        data = xxl_file.readlines()
    for d in data:
        for ind, p in enumerate([hole, nut, head]):
            match = re.search(p, d)
            if match is None:
                continue
            val = {}
            for k, v in match.groupdict().items():
                if k[-1] == '_':
                    continue
                if k in ['x', 'y', 'x0', 'y0', 'x1', 'y1', 'z', 'diameter', "length", "width", "thickness"]:
                    val[k] = round(float(v + '.' + (match.groupdict()[k+'_'] if match.groupdict()[k+'_'] else '')), 2)
                else:
                    val[k] = int(v)
            ret_d[ret_keys[ind]].append(val)
            break
    return ret_d


def convert_xxl(data):
    size = list(data['size'][0].values())
    holes = []
    rectangle = []
    hole_transform = {
        1: lambda h, s: ((h['x'], h['y'], s[2]), (h['x'], h['y'], s[2]-h['z']), h['diameter']),
        2: lambda h, s: ((0, h['y'], h['x']), (h['z'], h['y'], h['x']), h['diameter']),
        3: lambda h, s: ((s[0], h['y'], h['x']), (s[0] - h['z'], h['y'], h['x']), h['diameter']),
        4: lambda h, s: ((h['x'], 0, h['y']), (h['x'], h['z'], h['y']), h['diameter']),
        5: lambda h, s: ((h['x'], s[1], h['y']), (h['x'], s[1] - h['z'], h['y']), h['diameter']),
        6: lambda h, s: ((h['x'], h['y'], 0), (h['x'], h['y'], h['z']), h['diameter'])
    }
    for hole in data['hole']:
        holes.append(hole_transform[hole['plane']](hole, size))

    for rect in data['rect']:
        start = rect['x0'], rect['y0']
        end = rect['x1'], rect['y1']
        tl = []
        tr = []
        br = []
        bl = []
        for a, b in zip(start, end):
            tl.append((a if a < b else b) if a != b else (a-2))
            tr.append((a if a < b else b) if a != b else (a+2))
            br.append((a if a > b else b) if a != b else (b+2))
            bl.append((a if a > b else b) if a != b else (b-2))
        for pt in [tl, tr, br, bl]:
            pt.append({'B': 0})
        rectangle.append({'Height': rect['z'], 'Polygon': [tl, tr, br, bl, tl]})
    return holes, rectangle, size

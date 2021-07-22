import json
import os
import sys
from pathlib import Path
from flask import Flask, Response, request
from collections import namedtuple


sys.path.append(os.path.join(os.path.dirname(__file__), '..'))

from DataHandler.ApiClient import ApiClientClass
from DataHandler.DatabaseInterface import DatabaseDataTransform
from ModelClasses.Customer import Customer, AnonymousCustomer
from ModelClasses.Classificatory import ComplexCustomerClass
from Recommendation.Recommend import Recommend


APIConfig = namedtuple('APIConfig', ['api_config_path', 'login_data'])
RCDParam = namedtuple('RCDParam', ['param'])
api_cf = None
rcd_param = None
rcd = None


def init():
    config = json.loads((Path(app.root_path) / 'service_config.json').absolute().read_text())
    cf = config['api_config']
    global api_cf
    api_cf = APIConfig(api_config_path=cf['api_config_path'], login_data=cf['login_data'])

    cf = config['recommend_param']
    global rcd_param
    rcd_param = RCDParam(param={**cf['apriori_args'], **cf['trend_args']})
    __update_cache()


app = Flask(__name__)
app.before_first_request(init)


def __update_cache():
    global api_cf
    api_client = ApiClientClass(api_cf.api_config_path)
    api_client.login(api_cf.login_data)
    records = api_client.get_order()
    api_client.logout()
    global rcd, rcd_param

    rcd = Recommend(DatabaseDataTransform, records, records, ComplexCustomerClass, **rcd_param.param)


@app.route('/get_data/', methods=['GET'])
def get_data():
    global rcd
    customer_dto = request.get_json()
    customer = AnonymousCustomer() if len(customer_dto) == 0 else Customer.from_dict(customer_dto)
    content = rcd.get_dict(customer)
    # TODO rcd to json ;; pass customer???
    return Response(response=json.dumps(content), status=200, mimetype='application/json')


@app.route('/update_cache', methods=['POST'])
def update_cache():
    __update_cache()
    return Response(status=200)


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001, debug=True)

import json
from collections import namedtuple

import requests
from flask import Flask, request, Response
from pathlib import Path

import sys
import os

from Recommendation.Recommend import PersonalRecommend

sys.path.append(os.path.join(os.path.dirname(__file__), '..'))


ServiceConfig = namedtuple('ServiceConfig', ['host', 'port'])
service_cf = None


def init():
    global service_cf
    config = json.loads((Path(app.root_path) / 'service_config.json').absolute().read_text())
    cf = config['recommend_service']
    service_cf = ServiceConfig(host=cf['host'], port=int(cf['port']))


app = Flask(__name__)
app.before_first_request(init)


@app.route('/recommend/<guid>')
def recommend(guid):
    dto = json.loads(request.get_json())
    if dto:
        cart = dto.get('cart')
        customer = dto.get('customer')
    else:
        cart = []
        customer = {}
    global service_cf
    res = requests.get(f"http://{service_cf.host}:{service_cf.port}/get_data/", json=customer)
    personal_rcd = PersonalRecommend.from_dict(res.json())
    products, values = personal_rcd.add_cart(cart)
    return Response(status=200, response=json.dumps({'products': products, 'probability': values}))


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)

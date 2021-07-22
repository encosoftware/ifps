import flask
from flask import request, jsonify

import predict

app = flask.Flask(__name__)
app.config['DEBUG'] = True

@app.route('/stock-prediction', methods=['GET'])
def call_stock_prediction():
    t1 = request.args['t1']
    t2 = request.args['t2']
    length = request.args['length']

    results = predict.stock_prediction(float(t1), float(t2), int(length))

    return jsonify(results)

app.run()
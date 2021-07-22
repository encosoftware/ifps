import numpy as np
import keras
from keras.models import load_model

import matplotlib.pyplot as plt

def stock_prediction(q_prev, q_now, prediction_length):
    model = load_model('stock_prediction_model.h5')
    predicted_result = [q_prev, q_now]

    current = q_now
    previous = q_prev
    while len(predicted_result) < (prediction_length + 2):
        data = np.reshape(np.array([[previous, current]]), (1, 2, 1))
        pred = model.predict(data)
        previous = current
        current = pred.flat[0]
        predicted_result.append(current)

    return predicted_result

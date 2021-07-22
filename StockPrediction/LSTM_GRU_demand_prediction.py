import datetime
import numpy as np
import keras
import keras.backend as be
import sklearn.preprocessing as pp
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.metrics import mean_squared_error as mse
import csv


LEARNING_RATE = 0.01
LEARNING_DECAY = 0.01
ERROR_ASYMMETRY = 0  # -1 -> underestimate, +1 -> overestimate
NORMALIZER = pp.MinMaxScaler(feature_range=(0, 1))

FILE_NAME = "Global Superstore Orders 2016.xlsx"
SHEET_NAME = "Orders"


def lstm_model(input_shape, layers, output_size):
    model = keras.Sequential()
    if len(layers) > 1:
        model.add(keras.layers.LSTM(units=layers[0], return_sequences=True, input_shape=input_shape))
        model.add(keras.layers.Dropout(0.2))
        for i in range(1, len(layers) - 1):
            model.add(keras.layers.LSTM(units=layers[i], return_sequences=True))
            model.add(keras.layers.Dropout(0.2))
        model.add(keras.layers.LSTM(units=layers[-1]))
        model.add(keras.layers.Dropout(0.2))
    else:
        model.add(keras.layers.LSTM(units=layers[0], input_shape=input_shape))
        model.add(keras.layers.Dropout(0.2))
    model.add(keras.layers.Dense(units=output_size))
    # replaced asymmetric square error to 
    model.compile(optimizer=keras.optimizers.Adam(lr=LEARNING_RATE, decay=LEARNING_DECAY), loss=keras.losses.mean_squared_error)
    return model


def gru_model(input_shape, layers, output_size):
    model = keras.Sequential()
    if len(layers) > 1:
        model.add(keras.layers.GRU(units=layers[0], return_sequences=True, input_shape=input_shape))
        model.add(keras.layers.Dropout(0.2))
        for i in range(1, len(layers) - 1):
            model.add(keras.layers.GRU(units=layers[i], return_sequences=True))
            model.add(keras.layers.Dropout(0.2))
        model.add(keras.layers.GRU(units=layers[-1]))
        model.add(keras.layers.Dropout(0.2))
    else:
        model.add(keras.layers.GRU(units=layers[0], input_shape=input_shape))
        model.add(keras.layers.Dropout(0.2))
    model.add(keras.layers.Dense(units=output_size))
    model.compile(optimizer=keras.optimizers.Adam(lr=LEARNING_RATE, decay=LEARNING_DECAY), loss=asymmetric_square_error)
    return model


def load_excel_sheet(file_name, sheet_name):
    file = pd.ExcelFile(file_name)
    sheet = file.parse(sheet_name)
    return sheet


def prepare_data(data, input_length, test_fraction=0.1, valid_fraction=0.1):
    # reshape and normalize
    scaled_data = np.array(data)
    scaled_data = np.reshape(scaled_data, (-1, 1))
    scaled_data = NORMALIZER.fit_transform(scaled_data)
    # create training data
    training_length = int(input_length + (len(scaled_data) - input_length) * (1 - test_fraction - valid_fraction))
    train_x, train_y = [], []
    for i in range(input_length, training_length):
        train_x.append(scaled_data[i - input_length:i, 0])
        train_y.append(scaled_data[i, 0])
    train_x, train_y = np.array(train_x), np.array(train_y)
    train_x = np.reshape(train_x, (train_x.shape[0], train_x.shape[1], 1))
    # create testing data
    testing_length = int(input_length + (len(scaled_data) - input_length) * (1 - valid_fraction))
    test_x, test_y = [], []
    for i in range(training_length, testing_length):
        test_x.append(scaled_data[i - input_length:i, 0])
        test_y.append(scaled_data[i, 0])
    test_x, test_y = np.array(test_x), np.array(test_y)
    test_x = np.reshape(test_x, (test_x.shape[0], test_x.shape[1], 1))
    # create validation data
    val_x, val_y = [], []
    for i in range(testing_length, len(scaled_data)):
        val_x.append(scaled_data[i - input_length:i, 0])
        val_y.append(scaled_data[i, 0])
    val_x, val_y = np.array(val_x), np.array(val_y)
    val_x = np.reshape(val_x, (val_x.shape[0], val_x.shape[1], 1))
    return train_x, train_y, test_x, test_y, val_x, val_y


def prepare_data_with_week_month(data, weeks_months, input_length, test_fraction=0.1, valid_fraction=0.1):
    # reshape and normalize
    scaled_data = np.array(data)
    scaled_data = np.reshape(scaled_data, (-1, 1))
    scaled_data = NORMALIZER.fit_transform(scaled_data)
    scaled_weeks_months = np.array(weeks_months)
    scaled_weeks_months = np.reshape(scaled_weeks_months, (-1, 1))
    scaled_weeks_months = NORMALIZER.fit_transform(scaled_weeks_months)
    # create training data
    training_length = int(input_length + (len(scaled_data) - input_length) * (1 - test_fraction - valid_fraction))
    train_x, train_y = [], []
    for i in range(input_length, training_length):
        sales_segment = np.reshape(scaled_data[i - input_length:i, 0], (-1, 1))
        train_x.append(sales_segment)
        train_y.append(scaled_data[i, 0])
        week_month_segment = np.reshape(scaled_weeks_months[i - input_length + 1:i + 1, 0], (-1, 1))
        train_x[-1] = np.concatenate((train_x[-1], week_month_segment), axis=1)
    train_x, train_y = np.array(train_x), np.array(train_y)
    # create testing data
    testing_length = int(input_length + (len(scaled_data) - input_length) * (1 - valid_fraction))
    test_x, test_y = [], []
    for i in range(training_length, testing_length):
        sales_segment = np.reshape(scaled_data[i - input_length:i, 0], (-1, 1))
        test_x.append(sales_segment)
        test_y.append(scaled_data[i, 0])
        week_month_segment = np.reshape(scaled_weeks_months[i - input_length + 1:i + 1, 0], (-1, 1))
        test_x[-1] = np.concatenate((test_x[-1], week_month_segment), axis=1)
    test_x, test_y = np.array(test_x), np.array(test_y)
    # create validation data
    val_x, val_y = [], []
    for i in range(testing_length, len(scaled_data)):
        sales_segment = np.reshape(scaled_data[i - input_length:i, 0], (-1, 1))
        val_x.append(sales_segment)
        val_y.append(scaled_data[i, 0])
        week_month_segment = np.reshape(scaled_weeks_months[i - input_length + 1:i + 1, 0], (-1, 1))
        val_x[-1] = np.concatenate((val_x[-1], week_month_segment), axis=1)
    val_x, val_y = np.array(val_x), np.array(val_y)
    return train_x, train_y, test_x, test_y, val_x, val_y


def get_daily_sales_from_sheet(sheet, sub_category):
    order_date = sheet['Order Date'].values
    sub_cat = sheet['Sub-Category'].values
    quantity = sheet['Quantity'].values
    # find first and last order_date
    index_min = order_date[0]
    index_max = order_date[0]
    for i in range(len(order_date)):
        if order_date[i] < index_min:
            index_min = order_date[i]
        if order_date[i] > index_max:
            index_max = order_date[i]
    data_length = (index_max - index_min).astype('timedelta64[D]').astype(int) + 1
    sales = np.zeros((data_length,), dtype=np.int32)
    # fill sales
    for i in range(len(order_date)):
        if sub_cat[i] == sub_category:
            j = (order_date[i] - index_min).astype('timedelta64[D]').astype(int)
            sales[j] += quantity[i]
    return sales


def get_weekly_sales_from_sheet(sheet, sub_category):
    order_date = sheet['Order Date'].values
    sub_cat = sheet['Sub-Category'].values
    quantity = sheet['Quantity'].values
    # find first and last order_date
    index_min = order_date[0]
    index_max = order_date[0]
    for i in range(len(order_date)):
        if order_date[i] < index_min:
            index_min = order_date[i]
        if order_date[i] > index_max:
            index_max = order_date[i]
    data_length = ((index_max - index_min).astype('timedelta64[D]').astype(int) + 1) // 7
    sales = np.zeros((data_length,), dtype=np.int32)
    # fill sales
    for i in range(len(order_date)):
        if sub_cat[i] == sub_category:
            j = (order_date[i] - index_min).astype('timedelta64[D]').astype(int) // 7
            if j < data_length:
                sales[j] += quantity[i]
    return sales


def get_weekly_sales_and_week_from_sheet(sheet, sub_category):
    order_date = sheet['Order Date'].values
    sub_cat = sheet['Sub-Category'].values
    quantity = sheet['Quantity'].values
    # find first and last order_date
    index_min = order_date[0]
    index_max = order_date[0]
    for i in range(len(order_date)):
        if order_date[i] < index_min:
            index_min = order_date[i]
        if order_date[i] > index_max:
            index_max = order_date[i]
    wd = datetime.datetime.utcfromtimestamp(index_min.astype('O')/1e9).weekday()
    # if the first order is not Monday, start with the next week
    index_min = index_min + np.timedelta64((7 - wd) % 7, 'D')
    data_length = ((index_max - index_min).astype('timedelta64[D]').astype(int) + 1) // 7
    sales = np.zeros((data_length,), dtype=np.int32)
    weeks = np.zeros((data_length,), dtype=np.int32)
    for i in range(len(weeks)):
        weeks[i] = 100
    # fill sales
    for i in range(len(order_date)):
        if sub_cat[i] == sub_category:
            j = (order_date[i] - index_min).astype('timedelta64[D]').astype(int) // 7
            if 0 <= j < data_length:
                sales[j] += quantity[i]
                weeks[j] = datetime.datetime.utcfromtimestamp(order_date[i].astype('O')/1e9).strftime("%W")
    return sales, weeks


def get_monthly_sales_and_month_from_sheet(sheet, sub_category):
    order_date = sheet['Order Date'].values
    sub_cat = sheet['Sub-Category'].values
    quantity = sheet['Quantity'].values
    # find first and last order_date
    index_min = order_date[0]
    index_max = order_date[0]
    for i in range(len(order_date)):
        if order_date[i] < index_min:
            index_min = order_date[i]
        if order_date[i] > index_max:
            index_max = order_date[i]
    year_min = datetime.datetime.utcfromtimestamp(index_min.astype('O')/1e9).year
    month_min = datetime.datetime.utcfromtimestamp(index_min.astype('O')/1e9).month
    year_max = datetime.datetime.utcfromtimestamp(index_max.astype('O')/1e9).year
    month_max = datetime.datetime.utcfromtimestamp(index_max.astype('O')/1e9).month
    data_length = (year_max - year_min) * 12 + (month_max - month_min + 1)
    sales = np.zeros((data_length,), dtype=np.int32)
    months = np.zeros((data_length,), dtype=np.int32)
    # fill sales
    for i in range(len(order_date)):
        if sub_cat[i] == sub_category:
            order_year = datetime.datetime.utcfromtimestamp(order_date[i].astype('O') / 1e9).year
            order_month = datetime.datetime.utcfromtimestamp(order_date[i].astype('O') / 1e9).month
            j = (order_year - year_min) * 12 + (order_month - month_min)
            sales[j] += quantity[i]
            months[j] = order_month
    return sales, months


def asymmetric_square_error(y_true, y_prediction):
    return be.mean(be.square(y_true - y_prediction) * be.square(be.sign(y_true - y_prediction) + ERROR_ASYMMETRY))


def parallel_prediction(timestamp_length, categories, add_cycle=False):
    sheet = load_excel_sheet(FILE_NAME, SHEET_NAME)
    sales = []
    train_x, test_x, val_x = [], [], []
    train_y, test_y, val_y = [], [], []
    for category in categories:
        sales.append(get_weekly_sales_and_week_from_sheet(sheet, category)[0])
        tn_x, tn_y, tt_x, tt_y, vl_x, vl_y = prepare_data(sales[-1], timestamp_length)
        train_x.append(tn_x)
        test_x.append(tt_x)
        val_x.append(vl_x)
        train_y.append(tn_y.reshape((-1, 1)))
        test_y.append(tt_y.reshape((-1, 1)))
        val_y.append(vl_y.reshape((-1, 1)))
    if add_cycle:
        cycle = get_weekly_sales_and_week_from_sheet(sheet, categories[0])[1]
        tn_x, tn_y, tt_x, tt_y, vl_x, vl_y = prepare_data(cycle, timestamp_length)
        train_x.append(tn_x)
        test_x.append(tt_x)
        val_x.append(vl_x)
    train_x = np.concatenate(train_x, axis=2)
    test_x = np.concatenate(test_x, axis=2)
    val_x = np.concatenate(val_x, axis=2)
    train_y = np.concatenate(train_y, axis=1)
    test_y = np.concatenate(test_y, axis=1)
    val_y = np.concatenate(val_y, axis=1)
    model = lstm_model(train_x.shape[1:], [20, 20], len(categories))
    model.fit(train_x, train_y, epochs=300, verbose=1, batch_size=None, validation_data=(test_x, test_y))
    model.save('stock_prediction_model.h5')
    # predictions
    errors = []
    naive_errors = []
    predictions = model.predict(val_x, batch_size=None)
    for i in range(len(categories)):
        previous = np.concatenate([test_y[-1:, i], val_y[:-1, i]], axis=0)
        errors.append(mse(val_y[:, i], predictions[:, i]))
        naive_errors.append(mse(val_y[:, i], previous))
        print(categories[i], "prediction mean squared error:", errors[-1])
        print(categories[i], "naive pred mean squared error:", naive_errors[-1])
        plt.plot(predictions[:, i])
        plt.plot(val_y[:, i])
        plt.show()
    return predictions, val_y, errors, naive_errors


def write_weekly_sum_to_file():
    sheet = load_excel_sheet(FILE_NAME, "Orders")
    sales = get_weekly_sales_and_week_from_sheet(sheet, "Furnishings")[0]
    with open('quantities.csv', 'w', newline='') as csv_file:
        writer = csv.DictWriter(csv_file, fieldnames=['timestamp', 'quantity'])
        writer.writeheader()
        for i in range(len(sales)):
            writer.writerow({'timestamp': i, 'quantity': sales[i]})


if __name__ == "__main__":
    parallel_prediction(2, ["Furnishings"])
    # parallel_prediction(2, ["Furnishings", "Paper", "Supplies", "Art"], True)

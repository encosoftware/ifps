from DataHandler.GetFromExcel import GlobalSalesDataTransform
from Recommendation.Portfolio import Portfolio
from DataGenerator.Interface import GeneratorDataTransform
from pathlib import Path
from datetime import datetime
from script.recommendation_main import generate_data
from ModelClasses.Product import ActualProduct


def type_1():
    # path = Path("../data/Sample - Superstore.xls")
    path = Path("../data/Global Superstore Orders 2016.xlsx")
    return Portfolio(GlobalSalesDataTransform, path, from_date=datetime(2012, 1, 1),
                     to_date=datetime(2015, 1, 1), prd_num=3), 'Tables'


def type_2():
    data = generate_data()
    return Portfolio(GeneratorDataTransform, data, from_date=datetime(2015, 1, 1),
                     to_date=datetime(2018, 3, 1), prd_num=3), ActualProduct('Sütő', 'Konyha', [])


if __name__ == "__main__":
    ptf, product = type_2()

    print('%s :: Increasing %s with %s rate' % (product, *ptf.is_increasing(product)))
    print('%s :: Decreasing %s with %s rate' % (product, *ptf.is_decreasing(product)))
    print('%s :: Stagnating %s' % (product, ptf.is_stagnating(product)))

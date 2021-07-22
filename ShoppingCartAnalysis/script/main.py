from Analysis.CalculetedProbability import Calculate
from DataHandler.GetFromExcel import GlobalSalesDataTransform as Gst
from pathlib import Path


if __name__ == "__main__":
    path = Path("../data/Sample - Superstore.xls")
    data = Gst.order_transform(path)
    ppl = Gst.people_transform(path)
    # path = Path("../data/Global Superstore Orders 2016.xlsx")
    # Calculate.calculate_trends(path)
    # for cat, col in [('Paper', 'r'), ('Art', 'b'), ('Chairs', 'g')]:
    #    Calculate.calculate_trends_year(path, cat, col)

    # Calculate.calculate_profile(path)
    Calculate.calculate_rules(data=data)

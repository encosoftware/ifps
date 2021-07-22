from Test.TestDataClass import TestData, ArgumentProjection, ArgumentInterface, ArgumentProfile, ExpectedProjection, \
    ExpectedProfile, ArgumentTrend, ExpectedTrend, ArgumentBase, ExpectedBase, ArgumentRecommendation,\
    ExpectedRecommendation, ArgumentPortfolio, ExpectedPortfolio

INTERFACE_DATA = TestData(ArgumentInterface())
PROJECTION_DATA = TestData(ArgumentProjection(), ExpectedProjection())
PROFILE_DATA = TestData(ArgumentProfile(), ExpectedProfile())
TREND_DATA = TestData(ArgumentTrend(), ExpectedTrend())
BASE_DATA = TestData(ArgumentBase(), ExpectedBase())
RECOMMEND_DATA = TestData(ArgumentRecommendation(), ExpectedRecommendation())
PORTFOLIO_DATA = TestData(ArgumentPortfolio(), ExpectedPortfolio())

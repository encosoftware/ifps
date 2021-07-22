from Analysis.ProbabilityCalculator.AnalysisClass import BaseAnalysis, Analysis


class Recommend:
    def __init__(self, *args, **kwargs):
        data_transform_class, o_data, ppl_data, classificatory = args
        order = data_transform_class.order_transform(o_data)
        people = data_transform_class.people_transform(ppl_data)
        self.__analysis = BaseAnalysis(classificatory, order, people, **kwargs)

    def new_customer(self, customer):
        return PersonalRecommend(self.__analysis(customer))

    def get_dict(self, customer):
        return self.__analysis.get_analysis_dict(customer)


class PersonalRecommend:
    def __init__(self, analysis, rec_len=10):
        self.__analysis = analysis
        self.__len = rec_len
        self.__cart = []

    def __call__(self, product=None):
        rec_dict = {}
        if product is not None:
            self.__cart.append(product)
        probabilities = self.__analysis(product)
        cart = probabilities['Rule']
        profiles = probabilities['Customer']
        trend = probabilities['Trend']

        rec_dict.update(
            self.calculate_recommendation_score(self.flat_rules(cart), rec_dict, trend=trend, profiles=profiles))

        rec_dict.update(self.calculate_recommendation_score(self.flat_profiles(profiles), rec_dict, trend=trend))

        rec_dict.update(self.calculate_recommendation_score(trend.items(), rec_dict))

        return [prod for prod, _ in sorted(rec_dict.items(), key=lambda x: x[1], reverse=True)[:self.__len]],\
               [value for _, value in sorted(rec_dict.items(), key=lambda x: x[1], reverse=True)[:self.__len]]

    def add_cart(self, cart):
        self.__cart.extend(cart)
        for pr in cart[:-1]:
            self.__analysis(pr)
        return self(cart[-1] if len(cart) != 0 else None)

    def calculate_recommendation_score(self, flatten_part, selected, trend=None, profiles=None):
        ret_dict = {}
        for product, value in flatten_part:
            if product in [*selected.keys(), *self.__cart]:
                continue
            sum_value = value

            if profiles:
                profile_list = [profile[product] for profile in profiles if product in profile.keys()]
                sum_value += max(profile_list, key=lambda x: x[0])[0] if len(profile_list) > 0 else 0

            if trend and product in trend.keys():
                sum_value += trend[product]

            ret_dict[product] = sum_value
        return {k: v for k, v in sorted(ret_dict.items(), key=lambda x: x[1], reverse=True)[:self.__len]}

    @classmethod
    def flat_rules(cls, rules):
        return [(prod, rule.confidence) for rule in rules for prod in rule.rhs]

    @classmethod
    def flat_profiles(cls, profiles):
        return sorted([(prod, value[0]) for profile in profiles for prod, value in profile.items()],
                      key=lambda x: x[1], reverse=True)

    @classmethod
    def from_dict(cls, d, rec_len=10):
        return cls(Analysis.from_dict(d), rec_len)

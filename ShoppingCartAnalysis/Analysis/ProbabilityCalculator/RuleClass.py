from efficient_apriori import Rule


class SerializableRule(Rule):
    __slots__ = {'lhs', 'rhs', 'count_full', 'count_lhs', 'count_lhs', 'count_rhs', 'num_transactions'}

    def __init__(self, rule):
        super().__init__(rule.lhs, rule.rhs, rule.count_full, rule.count_lhs, rule.count_rhs, rule.num_transactions)

    def to_dict(self):
        return {key: self.__getattribute__(key) for key in self.__slots__}


class RuleClass:
    def __init__(self, rule_list):
        self.__all_rule = {}
        for rule in rule_list:
            if rule.lhs not in self.__all_rule.keys():
                self.__all_rule[rule.lhs] = []
            self.__all_rule[rule.lhs].append(SerializableRule(rule))

    def __call__(self, **kwargs):
        if 'key' in kwargs.keys():
            return self.__all_rule[kwargs['key']]
        return self.__all_rule.keys()

    def to_dict(self):
        ret = []
        for value in self.__all_rule.values():
            ret.extend([val.to_dict() for val in value])
        return ret

    @classmethod
    def from_dict(cls, rule_dict_list):
        rules = []
        for rule_dict in rule_dict_list:
            rules.append(Rule(**{k: tuple(v) if type(v) is list else v for k, v in rule_dict.items()}))
        return cls(rules)


class CartRules:
    def __init__(self, base_rule, **kwargs):
        self.__all_rule = base_rule

        self.__act_rule_keys = kwargs['act rule keys'] if 'act rule keys' in kwargs.keys() else []
        self.__act_rules = kwargs['act rules'] if 'act rules' in kwargs.keys() else []

    def __call__(self, product):
        new_rules_keys = [(product,), *[(*rb, product) for rb in self.__act_rule_keys]]
        for key in new_rules_keys:
            k = tuple(sorted(key))
            if k not in self.__all_rule():
                continue
            self.__act_rule_keys.append(k)
            self.__act_rules.extend(self.__all_rule(key=k))

    def to_dict(self):
        return {'rules': self.__all_rule.to_dict(),
                'act rule keys': [x for x in self.__act_rule_keys],
                'act rules': [x.to_dict() for x in self.__act_rules]}

    @classmethod
    def from_dict(cls, d):
        return cls(RuleClass.from_dict(d['rules']),
                   **{'act rule keys': d['act rule keys'],
                      'act rules': [SerializableRule(Rule(**rule)) for rule in d['act rules']]})

    @property
    def probability(self):
        return sorted(self.__act_rules, key=lambda x: x.confidence*x.support*(x.lift > 1), reverse=True)

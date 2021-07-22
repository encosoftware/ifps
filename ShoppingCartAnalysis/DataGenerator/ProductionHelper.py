from random import Random
import networkx as nx

from ModelClasses.Product import ActualProduct


class ProductGroup:
    def __init__(self, products, edges=None):
        self.__nodes = products
        self.__graph = nx.Graph()
        for product, _ in products.items():
            self.__graph.add_node(product)
        if edges:
            self.add_edge_from_dict(edges)

    def add_edge_from_dict(self, edges):
        for st, value in edges.items():
            for ed, w in value.items():
                self.add_edge(st, ed, w)

    def add_edge(self, start, end, weight):
        self.__graph.add_edge(start, end, weight=round(1-weight, 3))

    def products(self, product=None):
        if product:
            return {n: self.__graph.get_edge_data(product, n)['weight'] for n in self.__graph.neighbors(product)}
        return self.__nodes


class ProductSelector:
    def __init__(self, clusters):
        self.__group_data = {k: {} for k in clusters}
        self.__property_data = {k: {} for k in clusters}

    def add_group(self, group, gid):
        for cluster, pr in group.items():
            self.__group_data[cluster][gid] = pr

    def add_property(self, prop, pid):
        for cluster, pr in prop.items():
            self.__property_data[cluster][pid] = pr

    def select(self, customer):
        clusters = [k for k in self.__group_data.keys() if k.is_in(customer)]
        selected_group = self.__select_group(clusters)
        actual_product_list = []
        cart = []
        while True:
            product = self.__select_product(selected_group, cart)
            if product is None:
                break
            cart.append(product)
            actual_product_list.append(self.__create_actual_products(clusters, product))
        return actual_product_list

    def __select_group(self, clusters):
        rnd = Random()
        all_ = []
        for cl in clusters:
            all_.extend(self.__group_data[cl].items())
        rnd_value = sum([i for _, i in all_]) * rnd.random()
        act = 0
        for gr, p in all_:
            act += p
            if act >= rnd_value:
                return gr
        return max(all_, key=lambda x: x[1])[0]

    @staticmethod
    def __select_product(group, products=None):
        rnd = Random()
        potential = [] if products else list(group.products().items())
        in_cart = products if products else []
        for pr in in_cart:
            potential.extend(group.products(pr).items())
        rnd_value = sum([i for _, i in potential]) * rnd.random()
        act = 0
        for pr, p in potential:
            act += p
            if act >= rnd_value:
                return pr if pr not in in_cart else None
        return None

    def __create_actual_products(self, clusters, product):
        rnd = Random()
        all_ = {key: {k: 0 for k in value.keys()} for key, value in self.__property_data[clusters[0]].items()}
        for cl in clusters:
            for k, v in self.__property_data[cl].items():
                for name in v.keys():
                    all_[k][name] += v[name]
        product_properties = set()
        for pt in product.property_types:
            probabilities = all_[pt]
            rnd_val = sum([i for _, i in probabilities.items()]) * rnd.random()
            act = 0
            for pr, p in probabilities.items():
                act += p
                if act >= rnd_val:
                    product_properties.add(pr)
                    break
        return ActualProduct(product.name, product.category, product_properties)

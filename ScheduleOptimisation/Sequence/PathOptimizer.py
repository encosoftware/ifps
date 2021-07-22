from numpy.linalg import norm
from enum import Enum
import networkx as nx
import matplotlib.pyplot as plt


class Type(Enum):
    FULL = -1
    TREE = 0
    MIN_SPAN_TREE_1 = 1
    MIN_SPAN_TREE_2 = 2
    HAMILTON = 3
    RECURSIVE = 4


class PathOptimizer:
    @staticmethod
    def __make_full_graph_for_order(pts):
        graph = nx.Graph()
        indexes = []
        ind, pt = pts[0]
        graph.add_node(ind, pos=pt)
        for index, ind0_pt0 in enumerate(pts):
            ind0, pt0 = ind0_pt0
            indexes.append(ind0)
            for ind1, pt1 in pts[index + 1:]:
                graph.add_node(ind1, pos=pt1)
                dist = norm([pt0['X'] - pt1['X'], pt0['Y'] - pt1['Y'], pt0['Z'] - pt1['Z']])
                graph.add_edge(ind0, ind1, weight=dist)
        return graph, indexes

    @staticmethod
    def __make_path_from_tree_for_order(tree, ids):
        edges = nx.edge_dfs(tree, -1)
        edges = list(edges)
        prev_pt1 = -1
        path = [prev_pt1]
        branch_flag = False

        for pt0, pt1 in edges:
            if pt0 != prev_pt1:
                branch_flag = True
                break
            path.append(pt1)
            ids.remove(pt1)
            prev_pt1 = pt1
        return path[1:], branch_flag

    @staticmethod
    def __get_not_cowered_for_order(pts, ids, last_id):
        rest = []
        last = None
        for value in pts:
            if value[0] in ids:
                rest.append(value)
            if value[0] == last_id:
                last = value[1]
        return rest, last

    @staticmethod
    def __search_optimal_min_span_tree(pts, start):
        graph, ids = PathOptimizer.__make_full_graph_for_order([(-1, start), *pts])
        tree = nx.minimum_spanning_tree(graph)

        order, branch_flag = PathOptimizer.__make_path_from_tree_for_order(tree, ids)
        if branch_flag:
            rest, last = PathOptimizer.__get_not_cowered_for_order(pts, ids, order[-1])
            that = PathOptimizer.__search_optimal_min_span_tree(rest, last)
            order.extend(that)
        return order

    @staticmethod
    def __get_leave_nodes(tree, start_pt):
        leave_list = []
        for node_, degree in sorted(list(tree.degree()), key=lambda x: x[1]):
            if degree > 1:
                break
            if node_ != start_pt:
                leave_list.append(node_)
        return leave_list

    @staticmethod
    def __search_min_path_to_leaves(tree, start_pt, leave_list):
        min_path = [start_pt]
        min_weight = None
        for leave in leave_list:
            if not nx.has_path(tree, start_pt, leave):
                continue
            path = nx.shortest_path(tree, start_pt, leave, weight="weight")
            weight = sum([tree.get_edge_data(n0, n1)['weight'] for n0, n1 in zip(path[:-1], path[1:])])
            if (min_weight is None) or (weight < min_weight):
                min_weight = weight
                min_path = path
        return min_path

    @staticmethod
    def __search_path(graph, tree, start_pt):
        leave_list = PathOptimizer.__get_leave_nodes(tree, start_pt)
        min_path = PathOptimizer.__search_min_path_to_leaves(tree, start_pt, leave_list)
        for n in min_path:
            tree.remove_node(n)
        if nx.is_empty(tree):
            return [*min_path, *nx.nodes(tree)]
        leave_list = PathOptimizer.__get_leave_nodes(tree, start_pt)
        leave_dist = [(n, graph.get_edge_data(min_path[-1], n)['weight']) for n in leave_list]
        start_pt = min(leave_dist, key=lambda x: x[1])[0]
        return [*min_path, *PathOptimizer.__search_path(graph, tree, start_pt)]

    @staticmethod
    def __search_optimal_min_span_tree_2(pts, start):
        graph, _ = PathOptimizer.__make_full_graph_for_order([(-1, start), *pts])
        tree = nx.minimum_spanning_tree(graph)
        start_pt = -1
        return PathOptimizer.__search_path(graph, tree, start_pt)[1:]

    @staticmethod
    def __hamilton_path(graph, start=None):
        if start is None:
            start = list(graph.nodes())[0]
        search = [(graph, [start])]
        search_weight = [0]
        n = graph.number_of_nodes()
        while search:
            mm = search_weight.index(min(search_weight, key=lambda x: x))
            temp_graph, path = search.pop(mm)
            if len(path) == n:
                return path
            weight = search_weight.pop(mm)
            search_conf = []
            for node in temp_graph.neighbors(path[-1]):
                w = temp_graph.get_edge_data(path[-1], node)['weight']
                conf_path = path[:]
                conf_path.append(node)
                conf_graph = nx.Graph(temp_graph)
                conf_graph.remove_node(path[-1])
                search_conf.append((conf_graph, conf_path, weight + w))
            for g, p, s_w in search_conf:
                search.append((g, p))
                search_weight.append(s_w)
        return None

    @staticmethod
    def __search_optimal_hamilton(pts, start):
        graph, _ = PathOptimizer.__make_full_graph_for_order([(-1, start), *pts])
        return PathOptimizer.__hamilton_path(graph, -1)[1:]

    @staticmethod
    def __alpha_omega(path):
        return (path[0][0], path[-1][1]) if type(path[0]) is tuple else (path[0], path[0])

    @staticmethod
    def __make_compressed_graph(graph, paths):
        compressed_graph = nx.Graph()
        for index0, path0 in enumerate(paths):
            st0, ed0 = PathOptimizer.__alpha_omega(path0)
            for index1, path1 in enumerate(paths[index0+1:]):
                st1, ed1 = PathOptimizer.__alpha_omega(path1)
                for n0, n1 in [(st0, st1), (st0, ed1), (ed0, st1), (ed0, ed1)]:
                    compressed_graph.add_edge(n0, n1, weight=graph.get_edge_data(n0, n1)['weight'])
            for n in path0:
                if type(n) is not tuple:
                    break
                compressed_graph.add_edge(n[0], n[1], weight=0)
        return compressed_graph

    @staticmethod
    def __recursive_path(graph, start=-1):
        weight_list = [(node, graph.get_edge_data(start, node)['weight']) for node in nx.neighbors(graph, start)]
        if len(weight_list) == 0:
            return [start]
        graph.remove_node(start)
        nn = min(weight_list, key=lambda x: x[1])[0]
        return [start, *PathOptimizer.__recursive_path(graph, nn)]

    @staticmethod
    def __search_optimal_recursive(pts, start):
        graph, _ = PathOptimizer.__make_full_graph_for_order([(-1, start), *pts])
        return PathOptimizer.__recursive_path(graph)[1:]

    @staticmethod
    def __get_tree(pts):
        graph, _ = PathOptimizer.__make_full_graph_for_order(pts)
        tree = nx.minimum_spanning_tree(graph)
        return tree.edges

    @staticmethod
    def search_optimal_path(pts, start, optimizer_type=Type.TREE):
        if optimizer_type is Type.MIN_SPAN_TREE_1:
            return PathOptimizer.__search_optimal_min_span_tree(pts, start)
        elif optimizer_type is Type.MIN_SPAN_TREE_2:
            return PathOptimizer.__search_optimal_min_span_tree_2(pts, start)
        elif optimizer_type is Type.HAMILTON:
            return PathOptimizer.__search_optimal_hamilton(pts, start)
        elif optimizer_type is Type.RECURSIVE:
            return PathOptimizer.__search_optimal_recursive(pts, start)
        elif optimizer_type is Type.TREE:
            return PathOptimizer.__get_tree(pts)
        else:
            graph, _ = PathOptimizer.__make_full_graph_for_order(pts)
            return graph.edges

    @staticmethod
    def draw_graph(name, pts, edges):
        sum_weight = 0
        plt.figure(name)
        graph = nx.Graph()
        for index, pt in enumerate(pts):
            graph.add_node(index, pos=[pt['X'], pt['Y']])
        for n0, n1 in edges:
            w = norm([pts[n0]['X'] - pts[n1]['X'], pts[n0]['Y'] - pts[n1]['Y']])
            sum_weight += w
            graph.add_edge(n0, n1, weight=round(w, 2))
        pos = nx.get_node_attributes(graph, 'pos')
        nx.draw(graph, pos, with_labels=True)
        labels = nx.get_edge_attributes(graph, 'weight')
        nx.draw_networkx_edge_labels(graph, pos, edge_labels=labels)
        return round(sum_weight, 2)

    @staticmethod
    def show():
        plt.show()

    @staticmethod
    def save(path):
        plt.savefig(path, bbox_inches="tight")

    @staticmethod
    def close(name):
        plt.close(name)

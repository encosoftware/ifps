from Sequence.PathOptimizer import PathOptimizer, Type
import time
from random import Random
from pathlib import Path
import logging

logger = logging.getLogger("Path test")


def generate_pts(number=8):
    rand = Random()
    return ((rand.randint(0, 100), rand.randint(0, 100), 0) for _ in range(number))


def data():
    # raw_data = [(3, 3, 0), (7, 7, 0), (10, 7, 0), (7, 10, 0)]
    return [{'X': x, 'Y': y, 'Z': z} for x, y, z in generate_pts(40)], {'X': 0, 'Y': 0, 'Z': 0}


def large(iteration=10):
    root = Path('A:/Data/Butor/opt_path/')
    logger.addHandler(logging.FileHandler(root / 'path_test.log', 'w', 'utf-8'))
    logger.error('Full;Tree;;Spanning_v1;;Spanning_v2;;Hamilton;;Recursive')
    for iteration in range(iteration):
        iter_time = time.time()
        pt_list, start = data()
        pts = [x for x in enumerate(pt_list)]

        # path = PathOptimizer.search_optimal_path(pts, start, Type.FULL)
        full_w = 0  # PathOptimizer.draw_graph('Full', pt_list, path)

        # PathOptimizer.save(root / Path("%s_Full" % iteration))
        PathOptimizer.close('Full')

        path = PathOptimizer.search_optimal_path(pts, start, Type.TREE)
        tree_w = PathOptimizer.draw_graph('Tree', pt_list, path)

        # PathOptimizer.save(root / Path("%s_Tree" % iteration))
        PathOptimizer.close('Tree')

        path = PathOptimizer.search_optimal_path(pts, start, Type.MIN_SPAN_TREE_1)
        span_v1_w = PathOptimizer.draw_graph('Spanning_v1', pt_list, zip(path[:-1], path[1:]))

        # PathOptimizer.save(root / Path("%s_Spanning_v1" % iteration))
        PathOptimizer.close('Spanning_v1')

        path = PathOptimizer.search_optimal_path(pts, start, Type.MIN_SPAN_TREE_2)
        span_v2_w = PathOptimizer.draw_graph('Spanning_v2', pt_list, zip(path[:-1], path[1:]))

        # PathOptimizer.save(root / Path("%s_Spanning_v2" % iteration))
        PathOptimizer.close('Spanning_v2')

        # path = PathOptimizer.search_optimal_path(pts, start, Type.HAMILTON)
        ham_w = 0  # PathOptimizer.draw_graph('Hamilton', pt_list, zip(path[:-1], path[1:]))

        # PathOptimizer.save(root / Path("%s_Hamilton" % iteration))
        # PathOptimizer.close('Hamilton')

        path = PathOptimizer.search_optimal_path(pts, start, Type.RECURSIVE)
        rec_w = PathOptimizer.draw_graph('Recursive', pt_list, zip(path[:-1], path[1:]))

        # PathOptimizer.save(root / Path("%s_Recursive" % iteration))
        PathOptimizer.close('Recursive')

        logger.error("%s;%s;;%s;;%s;;%s;;%s" % (full_w, tree_w, span_v1_w, span_v2_w, ham_w, rec_w))
        print("Iteration: %s --- %s sec" % (iteration, round(time.time() - iter_time, 3)))


if __name__ == "__main__":
    large()

    """pt_list, start = data()
    pts = [x for x in enumerate(pt_list)]

    tt = time.time()
    path = PathOptimizer.search_optimal_path(pts, start, Type.TREE)
    string = "Tree: %s sec " % round(time.time() - tt, 2)
    weight = PathOptimizer.draw_graph('Tree', pt_list, path)

    print(string + "weight: %s" % weight)

    tt = time.time()
    path = PathOptimizer.search_optimal_path(pts, start, Type.HAMILTON)
    string = "Hamilton2: %s sec " % round(time.time() - tt, 2)
    weight = PathOptimizer.draw_graph('Hamilton2', pt_list, zip(path[:-1], path[1:]))
    
    print(string + "weight: %s" % weight)
    PathOptimizer.show()"""

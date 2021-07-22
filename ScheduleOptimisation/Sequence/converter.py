from enum import Enum

import cv2
import networkx as nx
import numpy as np
import json

from Parser import transformParameters as tP
from Sequence.sequenceType import *
from Sequence.PathOptimizer import PathOptimizer, Type

class PolygonType(Enum):
    NONE = 0
    RECTANGLE = 1
    CIRCLE = 2
    POLYGON = 3


class SequenceMaker:
    def __init__(self, holes, polygons, size):
        self._holes = holes
        self._polygons = polygons
        self._size = size

    def make(self):
        seq = []
        poly_params = []
        min_seq_pts = []
        for ind, polygon in enumerate(self._polygons):
            poly_type, params, min_pt = self.calculate_polygon_params(polygon)
            min_seq_pts.append((ind, min_pt))
            poly_params.append((poly_type, params))

        if len(min_seq_pts) != 0:
            order = PathOptimizer.search_optimal_path(min_seq_pts, {'X': 0, 'Y': 0, 'Z': 0}, Type.MIN_SPAN_TREE_2)
        else:
            order = []

        for ind, or_num in enumerate(order):
            poly_type, params = poly_params[or_num]
            seq.append(SequenceMaker.make_specialised_polygon(poly_type, ind, params))

        hole_params = {Plane.XY: {}}
        hole_pts = {Plane.XY: {}}
        hole_id_inc = {Plane.XY: {}}
        for hole in sorted(self._holes, key=lambda x: x[2]):
            param = self.calculate_hole_params(hole)
            pt = param['Pt']
            key = param['Plane']
            dia = param['Diameter']
            if key not in hole_pts.keys():
                hole_id_inc[key] = {dia: 0}
                hole_pts[key] = {dia: [(hole_id_inc[key][dia], {'X': hole[0][0], 'Y': hole[0][1], 'Z': hole[0][2]})]}
                hole_params[key] = {dia: [pt]}
            elif dia in hole_pts[key].keys():
                hole_id_inc[key][dia] += 1
                hole_params[key][dia].append(pt)
                hole_pts[key][dia].append((hole_id_inc[key][dia], {'X': hole[0][0], 'Y': hole[0][1], 'Z': hole[0][2]}))
            else:
                hole_id_inc[key][dia] = 0
                hole_params[key][dia] = [pt]
                hole_pts[key][dia] = [(hole_id_inc[key][dia], {'X': hole[0][0], 'Y': hole[0][1], 'Z': hole[0][2]})]

        last_pt = {'X': 0, 'Y': 0, 'Z': 0}  # min_seq_pts[order[-1]][1]
        ind_shift = len(seq)
        drill_order = len(seq)
        for plane_key in hole_pts.keys():
            for dia_key, value in hole_pts[plane_key].items():
                order = PathOptimizer.search_optimal_path(value, last_pt, Type.MIN_SPAN_TREE_2)
                param_ = {'Plane': plane_key, 'Diameter': dia_key}
                drill2 = Drill2(drill_order, param_)
                holes = []
                for ind, or_num in enumerate(order):
                    holes.append(Hole(ind, hole_params[plane_key][dia_key][or_num]))
                drill2.add_hole_list(holes)
                seq.append(drill2)
                drill_order += 1
                last_pt = {'X': 0, 'Y': 0, 'Z': 0}  # value[order[-1]][1]  # or 0,0,0 all point ??
                ind_shift += len(order)+1
            last_pt = {'X': 0, 'Y': 0, 'Z': 0}
        return seq

    def calculate_hole_params(self, hole):
        st, ed, dia = hole
        diff = np.array(st) - np.array(ed)
        ind = np.argwhere(diff != 0)
        mirror = (st[ind[0, 0]] == 0)

        pt = RelativePoint({'X': ed[0], 'Y': ed[1], 'Z': ed[2]}, self._size)

        if ind == 2:
            plane = 1
        elif ind == 1:
            plane = 5
        elif ind == 0:
            plane = 3
        else:
            plane = 0

        return {'Pt': pt, 'Diameter': dia, 'Plane': Plane(plane + mirror)}

    def key_func(self, value):
        if not (0 < value[0] < self._size[0]):
            ret_value = value[1] + value[0] - 1 if value[0] <= 0 else value[1] + self._size[0] - value[0]
        elif not (0 < value[1] < self._size[1]):
            ret_value = value[1] + value[0] - 1 if value[1] <= 0 else value[0] + self._size[1] - value[1]
        else:
            ret_value = np.sqrt(value[0]**2 + value[1]**2)
        return ret_value

    def find_minimum_polygon_point_and_index(self, polygon):
        pt = min(polygon['Polygon'], key=lambda v: self.key_func(v))
        return pt, polygon['Polygon'].index(pt)

    def convert_polygon_params(self, polygon):
        ret, centers = [], []
        pt_prev, angle, arc = None, None, False
        z = polygon['Height']
        for x, y, b in polygon['Polygon']:
            pt_act = np.array([x, y])
            pt = {'pt': RelativePoint({'X': x, 'Y': y, 'Z': z}, self._size)}
            if arc:
                center = SequenceMaker.find_center(pt_prev, pt_act, angle)
                centers.append(tuple(center))
                i, j = center - pt_prev
                clw = True if angle > 0 else False
                pt_prev, angle, arc = None, None, False
                pt.update({'I': i, 'J': j, 'CLW': clw})
            if b['B'] != 0:
                pt_prev, angle, arc = pt_act, b['B'] * np.pi, True
            ret.append(pt)
        return ret, centers

    def calculate_polygon_params(self, polygon):
        pts, centers = self.convert_polygon_params(polygon)
        poly_type = SequenceMaker.polygon_type(centers, [pts[0]['pt'], pts[-1]['pt']], len(pts))

        pt, ind = self.find_minimum_polygon_point_and_index(polygon)
        if ind == 0:
            return poly_type, pts, {'X': pt[0], 'Y': pt[1], 'Z': 0}

        x, y, _ = polygon['Polygon'][ind]
        z = polygon['Height']
        first = {'pt': RelativePoint({'X': x, 'Y': y, 'Z': z}, self._size)}
        last = pts[ind]

        return poly_type, [first, *pts[ind+1:], *pts[1:ind], last], {'X': pt[0], 'Y': pt[1], 'Z': 0}

    @staticmethod
    def find_center(pt_a, pt_b, ab_angle):
        pt_m = (pt_a + pt_b) / 2
        if np.abs(ab_angle) == np.pi:
            return pt_m[:2]

        _slope = pt_a - pt_b
        slope = -_slope[0] / _slope[1]

        direction = ((pt_m - pt_a)[:2] / np.abs(pt_m - pt_a)[:2])[1]
        pt_dist = np.linalg.norm(pt_a - pt_b)
        ct_dist = pt_dist / (2 * np.tan(ab_angle / 2))

        xc = direction*ct_dist / np.sqrt(1 + slope ** 2) + pt_m[0]
        yc = slope * (xc - pt_m[0]) + pt_m[1]

        return np.array([xc, yc])

    @staticmethod
    def polygon_type(centers, pts, number_of_pt):
        if pts[0] == pts[1]:
            if len(centers) > 1 and len(set(centers)) == 1:
                return PolygonType.CIRCLE
            elif len(centers) == 0 and number_of_pt == 5:
                return PolygonType.RECTANGLE
        return PolygonType.POLYGON

    @staticmethod
    def make_specialised_polygon(poly_type, order_num, params):
        if poly_type is PolygonType.RECTANGLE:
            ret = Rectangle(order_num, [pt['pt'] for pt in params])
        elif poly_type is PolygonType.CIRCLE:
            par = [params[0]['pt'], params[1]['I'], params[1]['J']]
            ret = Circle(order_num, par)
        elif poly_type is PolygonType.POLYGON:
            ret = Polygon(order_num, (params[0]['pt'], params[-1]['pt']))
            for order, act_pt in enumerate(params[1:]):
                if 'I' in act_pt.keys():
                    command = Arc(order, [act_pt[k] for k in act_pt.keys()])
                else:
                    command = Line(order, act_pt['pt'])
                ret.add_command(command)
        else:
            ret = None
        return ret


class ComponentImage:
    def __init__(self, size):
        self.x = int(size[0])
        self.y = int(size[1])
        self.z = int(size[2])
        self._size = size
        self.plane_xy = np.ones((self.y, self.x, 3), np.uint8)*140
        self.plane_zy = np.ones((self.y, self.z, 3), np.uint8) * 140
        self.plane_mzy = np.ones((self.y, self.z, 3), np.uint8) * 140
        self.plane_zx = np.ones((self.z, self.x, 3), np.uint8) * 140
        self.plane_mzx = np.ones((self.z, self.x, 3), np.uint8) * 140
        self.__font = cv2.FONT_HERSHEY_SIMPLEX
        self.__fontScale = 0.7
        self.__fontColor = (0, 0, 255)
        self.__lineType = 2

    def draw(self, seqs):
        color = (255, 0, 0)
        plane = 1
        for seq in seqs:
            if seq.is_rectangle():
                self.draw_rect(seq, plane, color)
            elif seq.is_circle():
                self.draw_circle(seq, plane, color)
            elif seq.is_polygon():
                self.draw_polygon(seq, plane, color)
            else:
                color = (0, 255, 0)
                plane = seq.get_plane().value
                self.draw_drill(seq, plane, color)

        return self.merge()

    def merge(self):
        merge = np.zeros((self.y+20+self.z*2, self.x+20+self.z*2, 3), np.uint8)
        merge[10+self.z:10+self.z+self.y, 5:5+self.z] = self.plane_mzy
        merge[10+self.z:10+self.z+self.y, 10+self.z:10+self.z+self.x] = self.plane_xy
        merge[10+self.z:10+self.z+self.y, 15+self.z+self.x:15+self.z*2+self.x] = self.plane_zy
        merge[5:5+self.z, 10+self.z:10+self.z+self.x] = self.plane_mzx
        merge[15+self.z+self.y:15+self.z*2+self.y, 10+self.z:10+self.z+self.x] = self.plane_zx
        return merge

    def draw_polygon(self, polygon, plane, color):
        oo = polygon.get_order_number()
        prev = polygon.get_start_pt().get_absolute_point(self._size)
        pt_t_y, pt_t_x = [prev['Y']], [prev['X']]
        for command in polygon.get_commands():
            pt0 = (int(prev['X']), int(prev['Y']))
            prev = command.get_pt().get_absolute_point(self._size)
            pt1 = (int(prev['X']), int(prev['Y']))
            pt_t_y.append(prev['Y'])
            pt_t_x.append(prev['X'])
            if command.is_arc():
                ij = command.get_ij()
                clw = command.get_clw()
                center = (int(pt0[0] + ij['I']), int(pt0[1] + ij['J']))
                angles = np.arctan2(ij['I'], ij['J']) / np.pi * 180
                angle_st = np.arctan2(-ij['J'], -ij['I']) / np.pi * 180
                if angle_st < 0:
                    angle_ed = angles if angles <= 0 else angles - 180
                elif angle_st > 0:
                    angle_ed = angles if angles >= 0 else angles + 180
                else:
                    angle_ed = angles
                rad = int(round(np.linalg.norm([ij['I'], ij['J']])))
                # print("%s -> %s ::: %s %s" % (angle_st, angle_ed, ij, clw))
                cv2.ellipse(self.get_img_plane(plane), center, (rad, rad), 0, angle_st, angle_ed+30, color, 4)
                cv2.line(self.get_img_plane(plane), pt0, pt1, color, 1)
                # cv2.circle(self.get_img_plane(plane), center, 3, (255, 255, 0), -1)
                # cv2.circle(self.get_img_plane(plane), pt1, 3, (255, 0, 255), -1)
                # break
            else:
                cv2.line(self.get_img_plane(plane), pt0, pt1, color, 4)
        ct = int(sum(pt_t_x)/len(pt_t_x)), int(sum(pt_t_y)/len(pt_t_y))
        cv2.putText(self.get_img_plane(plane), '%s' % oo, ct,
                    self.__font, self.__fontScale, self.__fontColor, self.__lineType)

    def draw_rect(self, rectangle, plane, color):
        prev = rectangle.get_start_pt().get_absolute_point(self._size)
        oo = rectangle.get_order_number()
        pt_t_y, pt_t_x = [prev['Y']], [prev['X']]
        for point in rectangle.get_points():
            pt0 = (int(prev['X']), int(prev['Y']))
            prev = point.get_absolute_point(self._size)
            pt1 = (int(prev['X']), int(prev['Y']))
            pt_t_y.append(prev['Y'])
            pt_t_x.append(prev['X'])
            # print("%s - %s" % (pt0, pt1))
            cv2.line(self.get_img_plane(plane), pt0, pt1, color, 4)
        ct = int(sum(pt_t_x) / len(pt_t_x)), int(sum(pt_t_y) / len(pt_t_y))
        cv2.putText(self.get_img_plane(plane), '%s' % oo, ct,
                    self.__font, self.__fontScale, self.__fontColor, self.__lineType)

    def draw_circle(self, circle, plane, color):
        oo = circle.get_order_number()
        pt = circle.get_start_pt().get_absolute_point(self._size)
        ij = circle.get_ij()
        center = (int(pt['X']+ij['I']), int(pt['Y']+ij['J']))
        rad = int(round(np.linalg.norm([center[0]-pt['X'], center[1]-pt['Y']])))
        cv2.circle(self.get_img_plane(plane), center, rad, color, 4)
        cv2.putText(self.get_img_plane(plane), '%s' % oo, center,
                    self.__font, self.__fontScale, self.__fontColor, self.__lineType)

    def draw_drill(self, drill, plane, color):
        ks = self.get_act_parameter_keys(plane)[1]
        # print(plane)
        k0, k1 = ks
        rad = int(round(drill.get_diameter()/2))
        for hole in drill.get_holes():
            oo = hole.get_order()
            pt = hole.get_pt().get_absolute_point(self._size)
            # print(pt)
            center = (int(pt[k0] if k0 != 'Z'else -pt[k0]), int(pt[k1] if k1 != 'Z'else self._size[2] + pt[k1]))
            cv2.circle(self.get_img_plane(plane), center, rad, color, -1)
            cv2.putText(self.get_img_plane(plane), '%s' % oo, (center[0]-5, center[1]-7),
                        self.__font, self.__fontScale, self.__fontColor, self.__lineType)

    def get_img_plane(self, plane):
        if plane == 1:
            return self.plane_xy
        elif plane == 3:
            return self.plane_zy
        elif plane == 4:
            return self.plane_mzy
        elif plane == 5:
            return self.plane_zx
        elif plane == 6:
            return self.plane_mzx

    @staticmethod
    def get_act_parameter_keys(plane):
        if plane in [1]:
            return ['Z', 'XY']
        elif plane in [3, 4]:
            return ['X', 'ZY']
        elif plane in [5, 6]:
            return ['Y', 'ZX']
        return None


def make_unit(data):
    unit = Unit(data['name'], data['size'])
    for component_data in data['components']:
        unit.add_component(make_component(component_data))
    return unit


def make_component(data):
    component = Component(data['name'], data['rotation'], data['size'], data['center'])
    seq_maker = SequenceMaker(data['holes'], data['polygons'], data['size'])
    component.add_sequence_list(seq_maker.make())
    return component


def transform_component(component):
    tr = tP.TransformComponent(component)
    hole = tr.transform_holes()
    poly = tr.transform_polygons()
    size = tr.get_size()
    rot = tr.get_rotation()
    cent = tr.get_center()
    return {'name': component['Name'], 'rotation': rot, 'size': size, 'center': cent, 'holes': hole, 'polygons': poly}


def read_unit_from_json(path):
    with open(path) as json_file:
        data = json.load(json_file)
    return make_unit(data)


def transform_unit_from_json(path):
    with open(path) as json_file:
        data = json.load(json_file)
    components = [transform_component(comp) for comp in data['components']]
    return make_unit({'name': '', 'size': (0, 0, 0), 'components': components})

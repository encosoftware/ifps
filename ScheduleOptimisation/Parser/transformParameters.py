import json
import numpy as np


class TransformComponent:
    def __init__(self, data):
        self._holes = data['holes']
        self._subbodies = data['subbodies']
        self._size = np.array([float(data['Length']), float(data['Width']), float(data['Height'])])
        if "Rotation" in data.keys():
            rot = data['Rotation'].replace('(', '')[:-1]
            cent = data['Center'].replace('(', '')[:-1]
            self._rot = [bool(v) for v in self._coordinate_parser(rot)]
            self._center = {k: v for k, v in zip('XYZ', self._coordinate_parser(cent))}
        else:
            self._rot = (False, False, False)
            self._center = {k: v for k, v in zip('XYZ', self.get_size())}

    @staticmethod
    def hole_parser(hole):
        hole_params = []
        for key in ['StartPoint', 'EndPoint']:
            hole_params.append(TransformComponent._coordinate_parser(hole[key][1:-1]))
        hole_params.append(float(hole['Diameter']))
        return hole_params

    @staticmethod
    def polygon_parser(sub_body):
        polygon = {'Height': float(sub_body['Height'])}
        coordinate_system = sub_body['SubBodyUcs'][1:-1].split('),(')
        for coord, key in zip(coordinate_system, 'CIJK'):
            polygon[key] = TransformComponent._coordinate_parser(coord)
        pts = []
        for coord in sub_body['Polygon'][1:-1].split('),('):
            pts.append(TransformComponent._coordinate_parser(coord, True))
        polygon['Polygon'] = pts
        return polygon

    @staticmethod
    def _coordinate_parser(coord_string, polygon_bool=False):
        x, y, z = coord_string.split(',')
        if polygon_bool:
            key, value = z.split('=')
            return float(x), float(y), {key: float(value)}
        return float(x), float(y), float(z)

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

    def polygon_pt_to_absolute(self, polygon):
        pt_o = np.array([x + y/2 for x, y in zip(polygon['C'], self._size)])
        vi, vj, vk = [np.array(polygon[x]) for x in 'IJK']
        act_z = (pt_o * vk)[2]
        if act_z < 0:
            act_z += self._size[2]
        polygon['Height'] = act_z
        abs_polygon = []
        for x, y, b in polygon['Polygon']:
            abs_pt = pt_o + vi * x + vj * y
            abs_polygon.append((*abs_pt[:2], b))
        polygon['Polygon'] = abs_polygon

    def hole_to_absolute(self, hole):
        pt_o = self._size/2
        for ind in range(len(hole)-1):
            abs_pt = np.array(hole[ind]) + pt_o
            hole[ind] = tuple(abs_pt)

    def transform_holes(self):
        ret = []
        for hole in self._holes:
            params = TransformComponent.hole_parser(hole)
            self.hole_to_absolute(params)
            ret.append(params)
        return ret

    def transform_polygons(self):
        ret = []
        for sub in self._subbodies:
            polygon = TransformComponent.polygon_parser(sub)
            self.polygon_pt_to_absolute(polygon)
            ret.append(polygon)
        # print(self._subbodies[0]['PartIndex'])
        return ret

    def get_size(self):
        return self._size

    def get_rotation(self):
        return self._rot

    def get_center(self):
        return self._center


def transform_components(path, ind=66):
    # debug
    with open(path) as json_file:
        data = json.load(json_file)
    component = data['components'][ind]
    name = component['Name']
    return TransformComponent(component), name


"""
circle centers
g0 x2.02 y1.01
g1 y1.41
g2 x2.22 y1.61 i.23 j-.03
g1 x3.42
g2 x3.62 y1.41 i-.03 j-.23
g1 y1.01
g2 x3.42 y.81 i-.23 j.03
g1 x2.22
g2 x2.02 y1.01 i.03 j.23

g0 x4.02 y1.01
g1 y1.41
g2 x4.22 y1.61 i.2 j-.0
g1 x5.42
g2 x5.62 y1.41 i-.0 j-.2
g1 y1.01
g2 x5.42 y.81 i-.2 j.0
g1 x4.22
g2 x4.02 y1.01 i.0 j.2"""

from pathlib import Path
from enum import Enum
import cv2
import numpy as np
from matplotlib.pyplot import imsave


class Flag(Enum):
    DIAM = -1
    DEPTH = -2
    FIRST = -3


class NewProgramRequired(ValueError):
    pass


class TimeEstimator:
    def __init__(self, machine_property, time_shift=0):
        manual_estimation = 1.5
        self.__time = manual_estimation + time_shift
        self.__machine_property = machine_property

    def calculate_time_with(self, distance, key):
        self.__time += distance/self.__machine_property[key]

    def add_time_with(self, key):
        self.__time += self.__machine_property[key]

    def get_sum_time(self):
        return self.__time

    def write_out(self, path):
        with open(path, 'w') as time:
            time.write("Estimated time :: %s min." % self.get_sum_time())


class ConvertUnit:
    class Act:
        def __init__(self):
            self._model = None
            self._size = None
            self._name = None
            self._index = None
            self._estimate = None
            self._plane = None
            self._diameter = None
            self._position = None

        def __call__(self, model, size, name, index, estimate, plane, diameter, position):
            self._model = model
            self._size = size
            self._name = name
            self._index = index
            self._estimate = estimate
            self._plane = plane
            self._diameter = diameter
            self._position = position

        @property
        def model(self):
            return self._model

        @property
        def size(self):
            return self._size

        @property
        def name(self):
            return self._name

        @property
        def index(self):
            return self._index

        @index.setter
        def index(self, value):
            self._index = value

        @property
        def estimate(self):
            return self._estimate

        @property
        def plane(self):
            return self._plane

        @plane.setter
        def plane(self, value):
            self._plane = value

        @property
        def diameter(self):
            return self._diameter

        @diameter.setter
        def diameter(self, value):
            self._diameter = value

        @property
        def position(self):
            return self._position

        @position.setter
        def position(self, value):
            self._position = value

    def __init__(self, machine, unit, converter_function_class):
        self._funcs = converter_function_class
        self._machine = machine
        self._unit = unit
        self._act = ConvertUnit.Act()

    @property
    def act(self):
        return self._act

    def new_program_init(self, index, size, c_name, shift=0):
        est = TimeEstimator(self._machine.get_time_estimator_property(), shift)
        pl = 1
        d = self._machine.get_milling_tool_diameter()
        pos = {'X': 0, 'Y': 0, 'Z': 0}
        model = CNCProgramModel()
        self._act(model, size, c_name, index, est, pl, d, pos)

    def generate_program(self, sequences, dir_path, formatting):
        index = self._act.index
        c_name = self._act.name
        u_name = self._unit.get_name()

        self._act.model.add_to_header('ProgramInfo', index)
        self._act.model.add_to_header('ProgramInfo', c_name)
        made_seqs, failed_seqs = self.make_component(sequences, self._act.size, formatting)

        file_name = "iso_g_%s__%s_%s" % ("0%s" % index if index < 10 else index, u_name, c_name)
        out_path = dir_path / Path(file_name + ".txt")
        img_path = dir_path / Path(file_name + ".jpg")

        self._funcs.write_out(out_path, self._act.model)
        self.make_component_layout(made_seqs, img_path)
        if len(failed_seqs) != 0:
            shift = self._act.estimate.get_sum_time()
            self.new_program_init(self._act.index + 1, self._act.size, self._act.name, shift)
            rotated = [s.rotate_drill_with_180(self._act.size) for s in failed_seqs]
            self.generate_program(rotated, dir_path, False)

    def make_component_layout(self, sequences, path):
        comp = ComponentImage(self._act.size)
        img = comp.draw(sequences)
        imsave(path, img)

    def make_programs(self, dir_path, u_size=None):
        diff = {'X': 0, 'Y': 0, 'Z': 0} if u_size is None else self._unit.get_size_difference(u_size)
        for component in self._unit.get_converted_component_list(u_size):
            c_name = component.get_name()
            u_name = self._unit.get_name()
            if 'back' in c_name.lower():
                continue
            sorted_seqs = sorted(component.get_sequences(), key=lambda x: x.get_order_number())
            size = component.get_new_size_comp(diff)
            index = 0 if self._act.index is None else self._act.index

            self.new_program_init(index, size, c_name)

            self.generate_program(sorted_seqs, dir_path, "front" in c_name.lower())

            img_path = dir_path / Path("Component_plan__%s_%s.jpg" % (u_name, c_name))
            self.make_component_layout(sorted_seqs, img_path)

            time_path = dir_path / Path("Component_plan__%s_%s.txt" % (u_name, c_name))
            self._act.estimate.write_out(time_path)
            self._act.index += 1

    def make_component(self, seqs, size, is_front):
        codes = self.start_program(is_front, size)
        converted_sequences = []
        failed_sequences = []
        for seq in seqs:
            try:
                codes.extend(self.make_sequence_code_lines(seq))
                converted_sequences.append(seq)
            except NewProgramRequired:
                failed_sequences.append(seq)
        codes = [*codes, *self.end_program()]
        self._act.model.add_commands(codes)
        return converted_sequences, failed_sequences

    def start_program(self, is_front, size):
        tool = self._machine.get_milling_tool()
        speed = self._machine.get_milling_speed()
        spin = self._machine.is_spin_clockwise()
        cool = self._machine.get_coolant()
        code_lines = [*self._funcs.safety_block(), *self._funcs.load_tool({'T': tool, 'S': speed}, spin, cool)]
        if not is_front:
            code_lines.extend(self.set_offset())
            code_lines.extend(self._funcs.formatting(self._act.plane, self._machine.get_drill_return_height(), size))
            pts = self._funcs.get_formatting_arg(self._machine.get_drill_return_height(), size)
            self.calculate_distance([self._act.position, pts[0]], 'rapid')
            self._act.position = pts[-1]
            self.calculate_distance(pts, 'milling')
        return code_lines

    def end_program(self):
        return [*self._funcs.turn_off_tool(self._machine.get_coolant(), "End Program"), *self.clear_offset(),
                *self._funcs.end_block(self._act.plane)]

    def set_offset(self):
        return [*self._funcs.set_offset()]

    def clear_offset(self):
        return [*self._funcs.clear_offset()]

    def make_sequence_code_lines(self, sequence):
        if sequence.is_circle():
            return self.circle(sequence)
        elif sequence.is_rectangle():
            return self.rectangle(sequence)
        elif sequence.is_polygon():
            return self.polygon(sequence)
        else:
            return self.drill_circle(sequence)

    def rectangle(self, rectangle):
        shift = round(self._act.diameter/2+0.5)
        start_pt = rectangle.get_start_pt()
        rapid_pt = {}
        for ind, key in enumerate('XY'):
            if start_pt[key] == 0:
                rapid_pt[key] = start_pt[key] - shift
            elif self._act.size[ind] == start_pt[key]:
                rapid_pt[key] = start_pt[key] + shift
            else:
                rapid_pt[key] = start_pt[key]
        out_pt = {**rapid_pt, 'Z': self._machine.get_drill_return_height()}
        self.calculate_distance([self._act.position, rapid_pt], 'rapid')
        self.calculate_distance([start_pt, *rectangle.get_points(), out_pt], 'milling')
        self._act.position = out_pt
        return self._funcs.rectangle(self._act.plane, rapid_pt, [start_pt, *rectangle.get_points(), out_pt])

    def circle(self, circle):
        pt0 = circle.get_start_pt()
        ij = circle.get_ij()
        max_v, max_k = max([(abs(v), k) for k, v in ij.items()], key=lambda x: x[0])
        shift = {k: ((v+1)/abs(v+1))*(max_v/10) for k, v in ij.items()}
        rapid_pt = {'X': pt0['X'] + shift['I'], 'Y': pt0['Y'] + shift['J']}
        shift['IJ'.replace(max_k, '')] *= -1
        out_pt = {'X': pt0['X'] + shift['I'], 'Y': pt0['Y'] + shift['J'], 'Z': self._machine.get_drill_return_height()}
        pt1 = {'X': pt0['X'] + ij['I']*2, 'Y': pt0['Y'] + ij['J']*2, 'Z': pt0['Z']}
        pt2 = {**pt0}
        pt1.update(ij)
        ij[max_k] *= -1
        pt2.update(ij)
        self.calculate_distance([self._act.position, rapid_pt], 'rapid')
        self.calculate_distance(ij, 'milling', True)
        self._act.position = out_pt
        return self._funcs.circle(self._act.plane, rapid_pt, [pt0, pt1, pt2, out_pt],
                                  self._machine.is_milling_clockwise())

    def polygon(self, polygon):
        shift = round(self._act.diameter / 2 + 0.5)
        start_pt = polygon.get_start_pt()
        out_pt = {k: polygon.get_end_pt()[k] for k in 'XY'}
        out_pt['Z'] = self._machine.get_drill_return_height()
        rapid_pt = {}
        for ind, key in enumerate('XY'):
            if start_pt[key] == 0:
                rapid_pt[key] = start_pt[key] - shift
            elif self._act.size[ind] == start_pt[key]:
                rapid_pt[key] = start_pt[key] + shift
            else:
                rapid_pt[key] = start_pt[key]
        pts = []
        arcs = []
        for command in polygon.get_commands():
            arg = command.get_pt()
            if command.is_arc():
                arg.update(command.get_ij())
                arcs.append(command.get_clw())
            else:
                arcs.append(None)

            pts.append(arg)
        self.calculate_distance([self._act.position, rapid_pt], 'rapid')
        self.calculate_distance([start_pt, *pts, out_pt], 'milling')
        self._act.position = out_pt
        return self._funcs.polygon(self._act.plane, rapid_pt, [start_pt, *pts, out_pt], [None, *arcs, None])

    def drill_circle(self, seq):
        code_lines = []
        act_diam = seq.get_diameter()
        act_plane = seq.get_plane()
        if self._act.diameter != act_diam:
            self._act.diameter = act_diam
            tool = self._machine.get_tool_with_diameter(act_diam)
            speed = self._machine.get_drill_speed()
            cool = self._machine.get_coolant()
            spin = self._machine.is_spin_clockwise()
            code_lines.extend(self._funcs.change_tool({'T': tool, 'S': speed}, spin, cool, "Change tool!"))
            self._act.estimate.add_time_with('tool')

        if self._act.plane != act_plane.value:
            if self._machine.invalid_plane(act_plane.value):
                raise NewProgramRequired()
            self._act.plane = act_plane.value
            code_lines.extend([*self._funcs.return_to_zero(self._act.plane),
                               *self._funcs.change_plane(act_plane, "New plane is %s" % act_plane)])
            self._act.estimate.add_time_with('plane')

        return_height = self._machine.get_drill_return_height()
        start_pt = seq.get_start_pt()
        drill_type = self._machine.get_drill_code()
        pts = [hole.get_pt() for hole in seq.get_holes()[1:]]
        code_lines.extend(self._funcs.drill_circle({'R': return_height}, start_pt, pts, drill_type))
        self.calculate_distance([self._act.position, start_pt], 'rapid')
        self.calculate_distance([start_pt, *pts], 'drill')
        self._act.position = pts[-1]
        return code_lines

    def calculate_distance(self, params, machine_speed_key, is_circle=False):
        distance = 0
        k0, k1 = self._funcs.get_act_parameter_keys(self._act.plane)[1]
        if is_circle:
            radius = np.linalg.norm([params['I'], params['J']])
            distance = radius*2*np.pi
        else:
            for pt0, pt_next in zip(params[:-1], params[1:]):
                tmp = np.linalg.norm([pt0[k0] - pt_next[k0], pt0[k1] - pt_next[k1]])
                if 'I' in pt_next.keys():
                    hypotenuse = np.linalg.norm([pt_next['I'], pt_next['J']])
                    angle = np.arcsin(tmp/(2*hypotenuse))*2/np.pi*180
                    distance += hypotenuse*np.pi/180*angle
                else:
                    distance += tmp
        self._act.estimate.calculate_time_with(distance, machine_speed_key)


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

        return np.flip(self.merge(), axis=0)

    def merge(self):
        merge = np.zeros((self.y+20+self.z*2, self.x+20+self.z*2, 3), np.uint8)
        merge[10+self.z:10+self.z+self.y, 5:5+self.z] = self.plane_mzy
        merge[10+self.z:10+self.z+self.y, 10+self.z:10+self.z+self.x] = self.plane_xy
        merge[10+self.z:10+self.z+self.y, 15+self.z+self.x:15+self.z*2+self.x] = self.plane_zy
        merge[5:5+self.z, 10+self.z:10+self.z+self.x] = self.plane_mzx
        merge[15+self.z+self.y:15+self.z*2+self.y, 10+self.z:10+self.z+self.x] = self.plane_zx
        return merge

    def draw_polygon(self, polygon, plane, color):
        prev = polygon.get_start_pt()
        for command in polygon.get_commands():
            pt0 = (int(prev['X']), int(prev['Y']))
            prev = command.get_pt()
            pt1 = (int(prev['X']), int(prev['Y']))
            if command.is_arc():
                ij = command.get_ij()
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
                cv2.ellipse(self.get_img_plane(plane), center, (rad, rad), 0, angle_st, angle_ed, color, 4)
            else:
                cv2.line(self.get_img_plane(plane), pt0, pt1, color, 4)

    def draw_rect(self, rectangle, plane, color):
        prev = rectangle.get_start_pt()
        for point in rectangle.get_points():
            pt0 = (int(prev['X']), int(prev['Y']))
            prev = point
            pt1 = (int(prev['X']), int(prev['Y']))
            cv2.line(self.get_img_plane(plane), pt0, pt1, color, 4)

    def draw_circle(self, circle, plane, color):
        pt = circle.get_start_pt()
        ij = circle.get_ij()
        center = (int(pt['X']+ij['I']), int(pt['Y']+ij['J']))
        rad = int(round(np.linalg.norm([center[0]-pt['X'], center[1]-pt['Y']])))
        cv2.circle(self.get_img_plane(plane), center, rad, color, 4)

    def draw_drill(self, drill, plane, color):
        ks = self.get_act_parameter_keys(plane)[1]
        k0, k1 = ks
        rad = int(round(drill.get_diameter()/2))
        for hole in drill.get_holes():
            pt = hole.get_pt()
            center = (int(pt[k0] if k0 != 'Z'else -pt[k0]), int(pt[k1] if k1 != 'Z'else self._size[2] + pt[k1]))
            cv2.circle(self.get_img_plane(plane), center, rad, color, -1)

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


class CNCProgramModel:
    def __init__(self):
        self._header = {}
        self._commands = []

    def add_to_header(self, key, data):
        if key in self._header.keys():
            self._header[key] += [data]
        else:
            self._header[key] = [data]

    def add_command(self, line):
        self._commands.append(line)

    def add_commands(self, lines):
        self._commands += lines

    def get_header(self):
        return self._header

    def get_commands(self):
        return self._commands

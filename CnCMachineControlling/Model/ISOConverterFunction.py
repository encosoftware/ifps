from enum import Enum


class GCode(Enum):
    G00 = 0  # Rapid move
    G01 = 1  # Linear feed move
    G02 = 2  # Clockwise Arc feed move
    G03 = 3  # Counter clockwise arc feed move
    G04 = 4  # Linear with dwell
    G05 = 5  # High-precision contour control P10000
    G05_1 = 5.1  # AI advanced Preview control Q1.
    G06_1 = 6.1  # Non-uniform ration B-spline machining
    G07 = 7  # Imaginary axis designation
    G09 = 9  # Exact stop check, non-modal
    G10 = 10  # Programmable data input
    G11 = 11  # Data write cancel
    G17 = 17  # XY plane selection
    G18 = 18  # ZX plane selection
    G19 = 19  # YZ plane selection
    G20 = 20  # Inch
    G21 = 21  # Millimeter
    G28 = 28  # Return to home position
    G30 = 30  # Return to secondary home position
    G31 = 31  # Feed until skip function
    G32 = 32  #
    G33 = 33  # Constant-pitch threading
    G34 = 34  # variable-pitch threading
    G40 = 40  # Tool radius compensation off
    G41 = 41  # Tool radiation compensation left
    G42 = 42  # Tool radiation compensation right
    G43 = 43  # Tool height offset comperation negative
    G44 = 44  # Tool height offset comperation positive
    G45 = 45  # Axis offset single increase
    G46 = 46  # Axis offset single decrease
    G47 = 47  # Axis offset double increase
    G48 = 48  # Axis offset double decrease
    G49 = 49  # Tool length offset compensation cancel
    G50 = 50  # Scaling function cancel
    G52 = 52  # Local coordinate system
    G53 = 53  # Machine coordinate system
    G54 = 54  # 54 - 59 work coordinate system
    G55 = 55  #
    G56 = 56  #
    G57 = 57  #
    G58 = 58  #
    G59 = 59  #
    G54_1 = 54.1  # Extended work coordinate system P1 to P48
    G61 = 61  #
    G62 = 62  #
    G64 = 64  #
    G68 = 68  #
    G69 = 69  #
    G70 = 70  #
    G71 = 71  #
    G72 = 72  #
    G73 = 73  #
    G74 = 74  #
    G75 = 75  #
    G76 = 76  #
    G80 = 80  #
    G81 = 81  #
    G82 = 82  #
    G83 = 83  #
    G84 = 84  #
    G84_2 = 84.2  #
    G84_3 = 84.3  #
    G85 = 85  #
    G86 = 86  #
    G87 = 87  #
    G88 = 88  #
    G89 = 89  #
    G90 = 90  # Absolute programming
    G91 = 91  # Incremental programming
    G92 = 92  #
    G94 = 94  #
    G95 = 95  #
    G96 = 96  #
    G97 = 97  #
    G98 = 98  #
    G99 = 99  #
    G100 = 100  #


class MCode(Enum):
    M00 = 0  #
    M01 = 1  #
    M02 = 2  #
    M03 = 3  # Spindle on clockwise rotation
    M04 = 4  # Spindle on counter clockwise rotation
    M05 = 5  # Spindle stop
    M06 = 6  # Automatic tool change
    M07 = 7  # Coolant on mist
    M08 = 8  # Coolant on flood
    M09 = 9  # Coolant off
    M10 = 10  # Pallet clamp on
    M11 = 11  # Pallet clamp off
    M13 = 13  # M03 + M08
    M19 = 19  # Spindle orientation
    M21 = 21  # Mirror X-axis
    M22 = 22  # Mirror Y-axis
    M23 = 23  # Mirror off
    M24 = 24  #
    M30 = 30  # End program, with return to top
    M41 = 41  #
    M42 = 42  #
    M43 = 43  #
    M44 = 44  #
    M48 = 48  # Feedrate override allowed
    M49 = 49  # Feedrate override NOT allowed
    M52 = 52  # Unload Last tool from spindle
    M60 = 60  # Automatic pallet change
    M98 = 98  # Subprogram call
    M99 = 99  # Subprogram end


class CodeLine:
    def __init__(self, code, arg, info=None):
        self._type = type(code)
        self._code = code
        self._arg = arg
        self._info = info

    def __eq__(self, other):
        return self.get_code() == other.get_code() and self.get_arg() == other.get_arg()

    def __str__(self):
        return self.__repr__()

    def __repr__(self):
        s = ''
        if self._type in [GCode, MCode]:
            s += '%s%s' % (self._code.name.replace('_', '.'), '' if self._arg is None else ' ')
        if self._arg is not None:
            last = len(self._arg)

            for arg, ind in zip(self._arg.items(), range(last)):
                k, v = arg
                if k == 'CLW':
                    continue
                s += '%s%s%s' % (k, v, ' ' if ind < last - 1 else '')
        if self._info is not None:
            s += '%s;(%s)' % (' ' if len(s) > 0 else '', self._info)
        return s

    def set_info(self, info):
        if type(info) is not str:
            raise TypeError('The key must be a string!')
        self._info = info if self._info is None else self._info + " " + info

    def get_code(self):
        return self._code

    def get_arg(self):
        return self._arg

    def get_code_type(self):
        return self._type

    def get_info(self):
        return self._info


class ISOConverterFunction:
    def __init__(self, measure_sys=GCode(21)):
        self._measure_sys = measure_sys

    def change_measure_system(self, code):
        if code == self._measure_sys.value:
            return []
        else:
            self._measure_sys = GCode(code)
            return [CodeLine(self._measure_sys, None)]

    def safety_block(self):
        code_lines = [CodeLine(GCode(17), None, 'Safety Block')]
        for g_code in [self._measure_sys, GCode(40), GCode(49), GCode(90)]:
            code_lines.append(CodeLine(g_code, None))
        return code_lines

    def end_block(self, plane):
        return [CodeLine(GCode(90), None), self.return_to_zero(plane), CodeLine(MCode(30), None)]

    @staticmethod
    def change_plane(plane, info=None):
        codes = []
        if plane.value == 1:
            codes = [CodeLine(GCode(17), None, info)]
        elif plane.value == 3:
            codes = [CodeLine(GCode(19), None, info)]
        elif plane.value == 4:
            codes = [CodeLine(GCode(19), None, info), CodeLine(MCode(22), None)]
        elif plane.value == 5:
            codes = [CodeLine(GCode(18), None, info)]
        elif plane.value == 6:
            codes = [CodeLine(GCode(18), None, info), CodeLine(MCode(21), None)]
        return codes

    @staticmethod
    def load_tool(params, spin, coolant_code=None):
        coolant = [] if coolant_code is None else [CodeLine(MCode(coolant_code), None)]
        return [CodeLine(MCode(6), params), CodeLine(MCode(3 if spin else 4), None), *coolant]

    @staticmethod
    def turn_off_tool(coolant_code=None, info=None):
        coolant = [] if coolant_code is None else [CodeLine(MCode(9), None)]
        return [CodeLine(MCode(5), None, info), *coolant]

    @staticmethod
    def change_tool(params, spin, coolant_code=None, info=None):
        code_lines = ISOConverterFunction.turn_off_tool(coolant_code, info)
        code_lines += ISOConverterFunction.load_tool(params, spin, coolant_code)
        return code_lines

    @staticmethod
    def get_act_parameter_keys(plane):
        if plane in [1]:
            return ['Z', 'XY']
        elif plane in [3, 4]:
            return ['X', 'YZ']
        elif plane in [5, 6]:
            return ['Y', 'XZ']
        return None

    @staticmethod
    def return_to_zero(plane):
        keys = ISOConverterFunction.get_act_parameter_keys(plane)
        param0 = {keys[0]: 0}
        param1 = {k: 0 for k in keys[1]}
        return [CodeLine(GCode(28), param0), CodeLine(GCode(28), param1)]

    @staticmethod
    def rapid_move(plane, params, info=None):
        keys = ISOConverterFunction.get_act_parameter_keys(plane)
        param = {k: params[k] for k in keys[1]}
        return [CodeLine(GCode(0), param, info)]

    def formatting(self, plane, return_height, size):
        param_list = self.get_formatting_arg(return_height, size)
        code_lines = ISOConverterFunction.rapid_move(plane, param_list[0], "Formatting")
        for pt in param_list[1:]:
            code_lines.append(CodeLine(GCode(1), pt))
        return code_lines

    def get_formatting_arg(self, machine_return_height, size):
        sm, lg = [3, 18] if self._measure_sys == GCode(21) else [0.15, 0.9]
        x, y, z = size
        arg_list = []
        for x_, y_ in [(-sm, y/2+lg), (0, y/2+sm), (0, 0), (x, 0), (x, y), (0, y), (0, y/2-sm), (-sm, y/2-lg)]:
            arg_list.append({'X': x_, 'Y': y_, 'Z': -z-1})
        arg_list[-1]['Z'] = machine_return_height
        return arg_list

    @staticmethod
    def set_offset():
        return [CodeLine(GCode(55), {'X': 3, 'Y': 3})]

    @staticmethod
    def clear_offset():
        return [CodeLine(GCode(54), {'X': 0, 'Y': 0})]

    @staticmethod
    def rectangle(plane, init_pt, pt_list):
        return [*ISOConverterFunction.rapid_move(plane, init_pt, 'Rectangle'), *[CodeLine(GCode(1), pt)
                                                                                 for pt in pt_list]]

    @staticmethod
    def circle(plane, init_pt, pt_list, clockwise):
        c = 2 if clockwise else 3
        codes = [1, c, c, 1]
        return [*ISOConverterFunction.rapid_move(plane, init_pt, 'Circle'),
                *[CodeLine(GCode(c), pt) for pt, c in zip(pt_list, codes)]]

    @staticmethod
    def polygon(plane, init_pt, pt_list, arcs):
        codes = [1 if arc is None else (2 if arc else 3) for arc in arcs]
        return [*ISOConverterFunction.rapid_move(plane, init_pt, 'Polygon'),
                *[CodeLine(GCode(c), pt) for pt, c in zip(pt_list, codes)]]

    @staticmethod
    def drill_circle(r_arg, init_pt, pt_list, drill_type):
        return [CodeLine(GCode(99), r_arg, "Drill circle"), CodeLine(GCode(drill_type), init_pt),
                *[CodeLine(None, pt) for pt in pt_list], CodeLine(GCode(80), None)]

    @staticmethod
    def write_out(path, model, coding='utf-8'):
        header = model.get_header()
        info_list = header['ProgramInfo']

        program = "%\r\n" + "O%s " % info_list[0]
        for info in info_list[1:]:
            program += "%s" % info
        program += "\r\n"

        code_lines = model.get_commands()
        for command, num in zip(code_lines, range(1, len(code_lines) + 1)):
            program += "N%s %s" % (('0%s' % num if num < 10 else num), command)
            program += "\r\n"

        program += '%'
        binary = program.encode(coding)
        with open(path, 'wb') as cnc_file:
            cnc_file.write(binary)

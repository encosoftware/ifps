from Modell.GCodeConverter import ConvertUnit
from Modell.Machine import ISOMachineGenerator
from Modell.ISOConverterFunction import ISOConverterFunction
from pathlib import Path
# TODO delete this file
from os import path
import sys
sys.path.append(path.abspath('..\\..\\ScheduleOptimisation'))
from Sequence.converter import transform_unit_from_json, read_unit_from_json


class Case:
    def __init__(self, number):
        if number == 0:
            Case.case_0()
        elif number == 1:
            Case.case_1()
        elif number == 2:
            Case.case_2()

    @staticmethod
    def case_0():
        p_in = Path("A:/Projects/Butor/ScheduleOptimisation/Parser/json/test.json")
        p_out = Path("A:/Data/Butor/CNC")
        func = transform_unit_from_json
        run(func, p_in, p_out)

    @staticmethod
    def case_1():
        p_in = Path("A:/Projects/Butor/ScheduleOptimisation/Parser/json/test_unit.json")
        p_out = Path("A:/Data/Butor/CNC_dummy_2/original")
        func = read_unit_from_json
        run(func, p_in, p_out)

    @staticmethod
    def case_2():
        p_in = Path("A:/Projects/Butor/ScheduleOptimisation/Parser/json/test_unit.json")
        p_out = Path("A:/Data/Butor/CNC_dummy_2/new")
        size = [750, 618, 1000]
        func = read_unit_from_json
        run(func, p_in, p_out, size)


def run(case_func, in_path, out_path, size=None, resolver=None):
    machine = ISOMachineGenerator.generate()
    unit = case_func(in_path)

    con = ConvertUnit(machine, unit, ISOConverterFunction())

    param = []
    for arg in [out_path, size, resolver]:
        if arg is None:
            break
        param.append(arg)

    con.make_programs(*param)


if __name__ == "__main__":
    # Case(0)
    Case(1)
    Case(2)

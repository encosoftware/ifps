from Model.GCodeConverter import ConvertUnit, TimeEstimator, CNCProgramModel, Path, NewProgramRequired
from unittest.mock import MagicMock, call


class TestConvertUnit:
    unit_mock = MagicMock()
    machine_mock = MagicMock()
    converter_mock = MagicMock()
    component_mock = MagicMock()

    def __init__(self):
        self._exp = self.Expected()
        self._arg = self.Argument()
        self._this = None
        self.__make_this()

    def __make_this(self):

        self._this = self.__make_new()
        self._this.new_program_init(11, [150, 300, 10], 'Test_complete')

    def __make_new(self):
        return ConvertUnit(self.machine_mock, self.unit_mock, self.converter_mock)

    class Expected:
        @property
        def new_program_init(self):
            return {'name': "Init_complete", 'diameter': 3.14, 'plane': 1, 'estimator_type': TimeEstimator, 'index': 10,
                    'position': {'X': 0, 'Y': 0, 'Z': 0}, 'size': [200, 200, 20], 'model_type': CNCProgramModel}

        @property
        def generate_program_add_to_header(self):
            return [call('ProgramInfo', 11), call('ProgramInfo', 'Test_complete')]

        @property
        def generate_program_make_component(self):
            return ['Sequence_0', 'Sequence_1', 'Sequence_2', 'Sequence_3'], [150, 300, 10], False

        @property
        def generate_program_make_component_layout(self):
            return ['Sequence_0', 'Sequence_1', 'Sequence_2', 'Sequence_3'],\
                   Path("Root/dir/path/iso_g_11__Test_unite_Test_complete.jpg")

        @property
        def make_component_layout_comp_img(self):
            return [150, 300, 10]

        @property
        def make_component_layout_draw(self):
            return []

        @property
        def make_component_ret_val(self):
            return ['Sequence_0', 'Sequence_2'], ['Sequence_1', 'Sequence_3']

        @property
        def make_component_call(self):
            return call('Sequence_0'), call('Sequence_2'), call('Sequence_1'), call('Sequence_3')

        @property
        def make_component_start(self):
            return True, [150, 300, 10]

        @property
        def make_component_commands(self):
            return ['Start_lines', 'Code_line_0', 'Code_line_2', 'End_lines']

        @property
        def make_programs_init(self):
            return 11, [150, 300, 10], 'Test_complete'

        @property
        def make_programs_gen(self):
            return [], Path("Root/dir/path"), False

        @property
        def make_programs_layout(self):
            return [], Path("Root/dir/path/Component_plan__Test_unite_Test_complete.jpg")

    class Argument:
        @staticmethod
        def __set_mock_property():
            TestConvertUnit.machine_mock.get_time_estimator_property.side_effect = [None]
            TestConvertUnit.machine_mock.get_milling_tool_diameter.side_effect = [3.14]
            TestConvertUnit.unit_mock.get_name.side_effect = ['Test_unite']
            TestConvertUnit.unit_mock.get_converted_component_list.side_effect = [[TestConvertUnit.component_mock]]
            TestConvertUnit.component_mock.get_name.side_effect = ['Test_complete']
            TestConvertUnit.component_mock.get_sequences.side_effect = [[]]
            TestConvertUnit.component_mock.get_new_size_comp.side_effect = [[150, 300, 10]]

        @property
        def new_program_init(self):
            self.__set_mock_property()
            return 10, [200, 200, 20], 'Init_complete'

        @property
        def sequences(self):
            return ['Sequence_0', 'Sequence_1', 'Sequence_2', 'Sequence_3']

        @property
        def generate_program(self):
            self.__set_mock_property()
            return self.sequences, Path("Root/dir/path"), False

        @property
        def make_component_layout(self):
            return [], Path("Root/dir/path/iso_g_11__Test_unite_Test_complete.jpg")

        @property
        def make_component(self):
            return self.sequences, [150, 300, 10], True

        @property
        def make_sequence_exceptions(self):
            return [['Code_line_0'], NewProgramRequired, ['Code_line_2'], NewProgramRequired]

        @property
        def make_programs(self):
            self.__set_mock_property()
            return Path("Root/dir/path")

    @property
    def argument(self):
        return self._arg

    @property
    def expected(self):
        return self._exp

    @property
    def this(self):
        return self._this

    @property
    def new_instance(self):
        return self.__make_new()

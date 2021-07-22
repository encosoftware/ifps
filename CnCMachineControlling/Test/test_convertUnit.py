from unittest import TestCase
from unittest.mock import patch, PropertyMock
from Test import data


class TestConvertUnit(TestCase):
    def test_new_program_init(self):
        c = data.new_instance
        c.new_program_init(*data.argument.new_program_init)
        values = {'size': c.act.size, 'name': c.act.name, 'diameter': c.act.diameter, 'plane': c.act.plane,
                  'index': c.act.index, 'model_type': type(c.act.model), 'estimator_type': type(c.act.estimate),
                  'position': c.act.position}
        self.assertEqual(data.expected.new_program_init, values)

    @patch("Model.GCodeConverter.ConvertUnit.make_component_layout")
    @patch("Model.GCodeConverter.ConvertUnit.make_component", return_value=(data.argument.sequences, []))
    @patch("Model.GCodeConverter.CNCProgramModel.add_to_header")
    def test_generate_program_add_to_header(self, add_to_header, make, layout):
        data.this.generate_program(*data.argument.generate_program)
        add_to_header.assert_has_calls(data.expected.generate_program_add_to_header)

    @patch("Model.GCodeConverter.ConvertUnit.make_component_layout")
    @patch("Model.GCodeConverter.ConvertUnit.make_component", return_value=(data.argument.sequences, []))
    @patch("Model.GCodeConverter.CNCProgramModel.add_to_header")
    def test_generate_program_make_component(self, add_to_header, make, layout):
        data.this.generate_program(*data.argument.generate_program)
        make.assert_called_with(*data.expected.generate_program_make_component)

    @patch("Model.GCodeConverter.ConvertUnit.make_component_layout")
    @patch("Model.GCodeConverter.ConvertUnit.make_component", return_value=(data.argument.sequences, []))
    @patch("Model.GCodeConverter.CNCProgramModel.add_to_header")
    def test_generate_program_make_component_layout(self, add_to_header, make, layout):
        data.this.generate_program(*data.argument.generate_program)
        layout.assert_called_with(*data.expected.generate_program_make_component_layout)

    @patch("matplotlib.image.imsave")
    @patch("Model.GCodeConverter.ComponentImage")
    def test_make_component_layout_comp_img(self, comp_img, save):
        data.this.make_component_layout(*data.argument.make_component_layout)
        comp_img.assert_called_with(data.expected.make_component_layout_comp_img)

    @patch("matplotlib.image.imsave")
    @patch("Model.GCodeConverter.ComponentImage.draw")
    def test_make_component_layout_draw(self, draw, save):
        data.this.make_component_layout(*data.argument.make_component_layout)
        draw.assert_called_with(data.expected.make_component_layout_draw)

    @patch("Model.GCodeConverter.ConvertUnit.start_program", return_value=[])
    @patch("Model.GCodeConverter.ConvertUnit.make_sequence_code_lines",
           side_effect=data.argument.make_sequence_exceptions)
    def test_make_component_ret_val(self, make, start):
        value = data.this.make_component(*data.argument.make_component)
        self.assertEqual(data.expected.make_component_ret_val, value)

    @patch("Model.GCodeConverter.ConvertUnit.start_program", return_value=[])
    @patch("Model.GCodeConverter.ConvertUnit.make_sequence_code_lines",
           side_effect=data.argument.make_sequence_exceptions)
    def test_make_component_call(self, make, start):
        data.this.make_component(*data.argument.make_component)
        make.has_calls(data.expected.make_component_call)

    @patch("Model.GCodeConverter.ConvertUnit.start_program", return_value=[])
    @patch("Model.GCodeConverter.ConvertUnit.make_sequence_code_lines",
           side_effect=data.argument.make_sequence_exceptions)
    def test_make_component_start(self, make, start):
        data.this.make_component(*data.argument.make_component)
        start.assert_called_once_with(*data.expected.make_component_start)

    @patch("Model.GCodeConverter.ConvertUnit.start_program", return_value=[])
    @patch("Model.GCodeConverter.ConvertUnit.make_sequence_code_lines",
           side_effect=data.argument.make_sequence_exceptions)
    @patch("Model.GCodeConverter.ConvertUnit.end_program")
    def test_make_component_end(self, end, make, start):
        data.this.make_component(*data.argument.make_component)
        end.assert_called_once_with()

    @patch("Model.GCodeConverter.ConvertUnit.start_program", return_value=['Start_lines'])
    @patch("Model.GCodeConverter.ConvertUnit.make_sequence_code_lines",
           side_effect=data.argument.make_sequence_exceptions)
    @patch("Model.GCodeConverter.ConvertUnit.end_program", return_value=['End_lines'])
    @patch("Model.GCodeConverter.CNCProgramModel.add_commands")
    def test_make_component_commands(self, commands, end, make, start):
        data.this.make_component(*data.argument.make_component)
        commands.assert_called_with(data.expected.make_component_commands)

    @patch("Model.GCodeConverter.ConvertUnit.make_component_layout")
    @patch("Model.GCodeConverter.ConvertUnit.generate_program")
    @patch("Model.GCodeConverter.ConvertUnit.new_program_init")
    @patch("Model.GCodeConverter.ConvertUnit.Act.estimate")
    @patch("Model.GCodeConverter.ConvertUnit.Act.index", new_callable=PropertyMock, side_effect=[11]*5)
    def test_make_programs_init(self, index, est, init, gen, layout):
        data.new_instance.make_programs(data.argument.make_programs)
        init.assert_called_once_with(*data.expected.make_programs_init)

    @patch("Model.GCodeConverter.ConvertUnit.Act.estimate")
    @patch("Model.GCodeConverter.ConvertUnit.make_component_layout")
    @patch("Model.GCodeConverter.ConvertUnit.generate_program")
    def test_make_programs_gen(self, gen, layout, est):
        data.new_instance.make_programs(data.argument.make_programs)
        gen.assert_called_once_with(*data.expected.make_programs_gen)

    @patch("Model.GCodeConverter.ConvertUnit.Act.estimate")
    @patch("Model.GCodeConverter.ConvertUnit.make_component_layout")
    @patch("Model.GCodeConverter.ConvertUnit.generate_program")
    def test_make_programs_layout(self, gen, layout, est):
        data.new_instance.make_programs(data.argument.make_programs)
        layout.assert_called_once_with(*data.expected.make_programs_layout)

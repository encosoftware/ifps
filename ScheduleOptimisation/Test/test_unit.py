from unittest import TestCase
from unittest.mock import patch
from parameterized import parameterized
from Test import furniture_test_data as data


class TestUnit(TestCase):
    @parameterized.expand(zip(data.argument.components))
    @patch('Sequence.sequenceType.Unit._add')
    def test_add_component(self, component, add):
        data.new_instance.add_component(component)
        add.assert_called_with(component)

    @patch('Sequence.sequenceType.Unit._add')
    def test_add_component_list(self, add):
        components = data.argument.components
        data.new_instance.add_component_list(components)
        add.assert_has_calls(data.expected.add_calls, any_order=True)

    @parameterized.expand(zip(data.argument.add, data.expected.add_key_number))
    def test__add_key_number(self, component, expected):
        unit = data.new_instance
        for comp in component:
            unit._add(comp)
        self.assertEqual(expected, len(unit.get_component().keys()))

    @parameterized.expand(zip(data.argument.add, data.expected.add_keys))
    def test__add_key(self, component, expected):
        unit = data.new_instance
        for comp in component:
            unit._add(comp)
        self.assertEqual(expected, set(unit.get_component().keys()))

    @parameterized.expand(zip(data.argument.add, data.expected.add_numbers))
    def test__add_number(self, component, expected):
        unit = data.new_instance
        for comp in component:
            unit._add(comp)
        result = []
        for v in unit.get_component().values():
            result.extend(v)
        self.assertEqual(expected, len(result))

    @parameterized.expand(zip(data.argument.new_sizes, data.expected.new_size_differences))
    def test__get_size_difference(self, new_size, expected):
        self.assertEqual(expected, data.this.get_size_difference(new_size))

    @parameterized.expand(zip(data.argument.components, data.expected.conflict_index))
    def test__get_conflict_component(self, component, expected):
        conflict = data.this._get_conflict_component(component)
        self.assertEqual(expected, set(conflict))

    @parameterized.expand(zip(data.argument.components, data.expected.available))
    def test__conflict_available_dim(self, component, expected):
        conflict = data.this._get_conflict_component(component)
        key = component.get_exact_plane()[0]
        available = data.this._conflict_available_dim(key, conflict)
        self.assertEqual(expected, available)

    def test_get_component_list(self):
        test = set(data.this.get_component_list())
        self.assertEqual(data.expected.component_list, test)

    @patch("Sequence.sequenceType.Component.convert_sequences_with_conflict")
    @patch("Sequence.sequenceType.Component.convert_sequences")
    def test_convert_components_without_conflict(self, simple_converter, _):
        data.this.convert_components()
        simple_converter.assert_called_with(None)

    @patch("Sequence.sequenceType.Component.convert_sequences_with_conflict")
    @patch("Sequence.sequenceType.Component.convert_sequences")
    def test_convert_components_with_conflict(self, _, conflict_converter):
        data.this.convert_components()
        conflict_converter.assert_called_with(*data.expected.convert_with_conflict_arguments)

    @parameterized.expand(zip(data.argument.new_sizes, data.expected.new_size_differences))
    @patch("Sequence.sequenceType.Component.convert_sequences_with_conflict")
    @patch("Sequence.sequenceType.Component.convert_sequences")
    def test_convert_components_without_conflict_size(self, size, expected, simple_converter, _):
        data.this.convert_components(size)
        simple_converter.assert_called_with(expected)

    @parameterized.expand(zip(data.argument.new_sizes, data.expected.convert_with_conflict_arguments_new_size))
    @patch("Sequence.sequenceType.Component.convert_sequences_with_conflict")
    @patch("Sequence.sequenceType.Component.convert_sequences")
    def test_convert_components_with_conflict_size(self, size, expected, _, conflict_converter):
        data.this.convert_components(size)
        conflict_converter.assert_called_with(*expected)

    @patch("Sequence.sequenceType.Unit.convert_components")
    @patch("Sequence.sequenceType.Unit.get_component_list")
    def test_get_converted_component_list(self, component_list, _):
        data.this.get_converted_component_list()
        component_list.assert_called_with()

    @parameterized.expand(zip(data.argument.component_params, data.expected.component_converter_arguments))
    @patch("Sequence.sequenceType.Unit.convert_components")
    @patch("Sequence.sequenceType.Unit.get_component_list")
    def test_get_converted_component_list_new_size(self, param, expected, _, convert):
        data.this.get_converted_component_list(*param)
        convert.assert_called_with(*expected)

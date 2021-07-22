from unittest import TestCase
from unittest.mock import patch
from parameterized import parameterized
from Sequence.sequenceType import Component
from Test import component_test_data as data


class TestComponent(TestCase):
    @parameterized.expand(zip(data.this, data.expected.shifted_key))
    def test_shifted_keys(self, component, expected):
        self.assertEqual(expected, component.shifted_keys())

    @parameterized.expand(zip(data.this, data.expected.neutral_key))
    def test_get_neutral(self, component, expected):
        self.assertEqual(expected, component.get_neutral())

    @parameterized.expand(zip(data.argument.plane, data.this, data.expected.plane))
    def test_get_plane_to(self, plane_key, component, expected):
        for exp, comp in zip(expected, component.get_plane_to(plane_key)):
            self.assertAlmostEqual(exp, comp, delta=0.000001)

    @parameterized.expand(zip(data.this, data.expected.exact_plane))
    def test_get_exact_plane(self, component, expected):
        for exp, comp in zip(expected, component.get_exact_plane()):
            self.assertAlmostEqual(exp, comp, delta=0.000001)

    @parameterized.expand(zip(data.this, data.expected.size_unit))
    def test_get_original_size_unit(self, component, expected):
        self.assertEqual(expected, component.get_original_size_unit())

    @parameterized.expand(zip(data.this, data.expected.size_component))
    def test_get_original_size_comp(self, component, expected):
        self.assertEqual(expected, component.get_original_size_comp())

    @parameterized.expand(zip(data.argument.size_change, data.this, data.expected.size_new_unit))
    def test_get_new_size_unit(self, size, component, expected):
        self.assertEqual(expected, component.get_new_size_unit(size))

    @parameterized.expand(zip(data.argument.size_change, data.this, data.expected.size_new_unit))
    def test_get_new_size_unit_2(self, size, component, expected):
        self.assertEqual(expected, component.get_new_size_unit(size))

    @parameterized.expand(zip(data.argument.size_change, data.this, data.expected.size_new_component))
    def test_get_new_size_comp(self, size, component, expected):
        self.assertEqual(expected, component.get_new_size_comp(size))

    @parameterized.expand(zip(data.expected.size_unit, data.this, data.expected.size_component))
    def test_size_unit_to_component(self, size, component, expected):
        self.assertEqual(expected, component.size_unit_to_component(size))

    @parameterized.expand(zip(data.argument.add_sequence, data.new_instance))
    def test_add_sequence(self, sequence, component):
        expected = len(component.get_sequences()) + 1
        component.add_sequence(sequence)
        self.assertEqual(expected, len(component.get_sequences()))

    @parameterized.expand(zip(data.argument.add_sequence_list, data.new_instance))
    def test_add_sequence_list(self, sequences, component):
        expected = len(component.get_sequences()) + len(sequences)
        component.add_sequence_list(sequences)
        self.assertEqual(expected, len(component.get_sequences()))

    @parameterized.expand(zip(data.this, data.expected.sequence_type))
    def test_add_sequence_type(self, component, expected):
        self.assertEqual(expected, type(component.get_sequences()[0]))

    @parameterized.expand(zip(data.this, data.expected.size_component))
    @patch('Sequence.sequenceType.Component._convert_seq')
    def test_convert_sequences_original_size(self, component, expected, convert_seq):
        component.convert_sequences(None)
        convert_seq.assert_called_with(expected)

    @parameterized.expand(zip(data.this, data.expected.size_new_convert))
    @patch('Sequence.sequenceType.Component._convert_seq')
    def test_convert_sequences_new_size(self, component, expected, convert_seq):
        component.convert_sequences({'X': 20, 'Y': 5, 'Z': -10})
        convert_seq.assert_called_with(expected)

    @parameterized.expand(zip(data.this, data.expected.size_component))
    @patch('Sequence.sequenceType.Component._convert_seq')
    def test_convert_sequences_with_conflict_original_size(self, component, expected, convert_seq):
        component.convert_sequences_with_conflict(None, None, None, None)
        convert_seq.assert_called_with(expected)

    @parameterized.expand(zip(data.argument.available, data.this, data.expected.size_component))
    @patch('Sequence.sequenceType.Component._convert_seq')
    def test_convert_sequences_with_conflict_new_size_def(self, available, component, expected, convert_seq):
        original = data.argument.size_unit_original
        new = data.argument.size_unit_new
        component.convert_sequences_with_conflict(available, original, new, Component.default_resolver)
        convert_seq.assert_called_with(expected)

    @parameterized.expand(zip(data.argument.available, data.this, data.expected.size_new_conflict_convert_opt))
    @patch('Sequence.sequenceType.Component._convert_seq')
    def test_convert_sequences_with_conflict_new_size_opt(self, available, component, expected, convert_seq):
        original = data.argument.size_unit_original
        new = data.argument.size_unit_new
        component.convert_sequences_with_conflict(available, original, new, Component.optional_resolver)
        convert_seq.assert_called_with(expected)

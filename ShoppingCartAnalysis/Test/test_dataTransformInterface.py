from DataHandler.DataInterface import DataTransformInterface as Dti

from unittest import TestCase
from parameterized import parameterized
from unittest.mock import patch

from Test import INTERFACE_DATA as DATA


class TestDataTransformInterface(TestCase):
    def test__order_transform(self):
        with self.assertRaises(NotImplementedError) as context:
            Dti._order_transform([])
        self.assertTrue('.', context.exception)

    def test__people_transform(self):
        with self.assertRaises(NotImplementedError) as context:
            Dti._people_transform([])
        self.assertTrue('.', context.exception)

    @parameterized.expand(zip(DATA.argument.order_people_transform,))
    @patch('DataHandler.DataInterface.DataTransformInterface.order_validator')
    @patch('DataHandler.DataInterface.DataTransformInterface._order_transform')
    def test_order_transform_call(self, value, transform, _):
        Dti.order_transform(value)
        transform.assert_called_with(value)

    @parameterized.expand(zip(DATA.argument.order_people_transform,))
    @patch('DataHandler.DataInterface.DataTransformInterface.order_validator')
    @patch('DataHandler.DataInterface.DataTransformInterface._order_transform', side_effect=lambda x: hash(x))
    def test_order_validator_call(self, value, _, validator):
        Dti.order_transform(value)
        validator.assert_called_with(hash(value))

    @patch('DataHandler.DataInterface.DataTransformInterface.order_validator', side_effect=lambda x: False)
    @patch('DataHandler.DataInterface.DataTransformInterface._order_transform')
    def test_order_transform_exception(self, *_):
        with self.assertRaises(AttributeError):
            Dti.order_transform(None)

    @parameterized.expand(zip(DATA.argument.order_people_transform, ))
    @patch('DataHandler.DataInterface.DataTransformInterface.people_validator')
    @patch('DataHandler.DataInterface.DataTransformInterface._people_transform')
    def test_people_transform_call(self, value, transform, _):
        Dti.people_transform(value)
        transform.assert_called_with(value)

    @parameterized.expand(zip(DATA.argument.order_people_transform, ))
    @patch('DataHandler.DataInterface.DataTransformInterface.people_validator')
    @patch('DataHandler.DataInterface.DataTransformInterface._people_transform', side_effect=lambda x: hash(x))
    def test_people_validator_call(self, value, _, validator):
        Dti.people_transform(value)
        validator.assert_called_with(hash(value))

    @patch('DataHandler.DataInterface.DataTransformInterface.people_validator', side_effect=lambda x: False)
    @patch('DataHandler.DataInterface.DataTransformInterface._people_transform')
    def test_people_transform_exception(self, *_):
        with self.assertRaises(AttributeError):
            Dti.people_transform(None)

    @parameterized.expand(zip(DATA.argument.validator_exception))
    def test_order_validator_exception(self, value):
        with self.assertRaises(ValueError):
            Dti.order_validator(value)

    @parameterized.expand(zip(DATA.argument.order_true))
    def test_order_validator_true(self, order):
        self.assertTrue(Dti.order_validator(order))

    @parameterized.expand(zip(DATA.argument.order_false))
    def test_order_validator_false(self, order):
        self.assertFalse(Dti.order_validator(order))

    @parameterized.expand(zip(DATA.argument.validator_exception))
    def test_people_validator_exception(self, value):
        with self.assertRaises(ValueError):
            Dti.people_validator(value)

    @parameterized.expand(zip(DATA.argument.people_true))
    def test_people_validator_true(self, people):
        self.assertTrue(Dti.people_validator(people))

    @parameterized.expand(zip(DATA.argument.people_false))
    def test_people_validator_false(self, people):
        self.assertFalse(Dti.people_validator(people))

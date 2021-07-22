import * as common from '../../support/common';

describe('Orders', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/sales/orders');
    });
    it('List orders', () => {
        cy.wait(1000);
        cy.screenshot('orders_list');
    });
    // it('Create new order', () => {
    //     common.get_contains_click("button", "Create new order", true);
    //     cy.wait(1000);
    //     cy.screenshot('empty_create_new_order');
    //     cy.get(".mat-dialog-container").within(() => {
    //         common.withinNgSelectName('customerId', 'Fred Customer (seed)');
    //         common.getName_clear_type('orderName', 'ordername' + Math.floor(Math.random() * 10000));
    //         common.getName_clear_type('postCode', '1234');
    //         common.getName_clear_type('city', 'Budapest');
    //         common.getName_clear_type('address', '1234 enco utca');
    //         common.withinNgSelectName('salesId', 'Freddy SalesPerson');
    //         cy.get('input[name=deadline]').click();
    //         common.get_contains_click('button', 'Save', true);
    //     });
    // });
});
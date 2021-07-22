import * as common from '../../support/common';

describe('Order schedulings', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/order-scheduling');
        cy.wait(1000);
    });
    it('List order schedulings', () => {
        cy.screenshot('order_schedulings');
    });
});
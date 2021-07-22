import * as common from '../../support/common';

describe('General expenses', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/finance/general-expenses');
        cy.wait(1000);
    });
    it('List general expenses', () => {
        cy.screenshot('general-expenses_list');
    });
    it('Edit expense', () => {
        common.setupBackendProxy('GET', 'api/generalexpenses/**', 'getCall');
        cy.get('.cdk-row > .cdk-column-amount').click();
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_expense');
    });
    it('Filter Amount', () => {
        common.filterHelper('.cdk-row > .cdk-column-amount', 'amount', 'general-expenses', 'Clear filters');
    });
    it('Filter Name', () => {
        common.filterHelper('.cdk-row > .cdk-column-name', 'name', 'general-expenses', 'Clear filters');
    });
});
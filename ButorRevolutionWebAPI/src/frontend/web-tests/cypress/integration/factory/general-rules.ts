import * as common from '../../support/common';

describe('General rules', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/finance/general-rules');
        cy.wait(1000);
    });
    it('List general rules', () => {
        cy.screenshot('general-rules_list');
    });
    it('Edit rule', () => {
        common.setupBackendProxy('GET', 'api/generalexpenses/rules/**', 'getCall');
        cy.get('tbody > :nth-child(1) > .cdk-column-amount').click();
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_rule');
    });
    it('Filter Amount', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-amount', 'amount', 'general-rules', 'Clear filters');
    });
    it('Filter Name', () => {
        common.filterHelper('.cdk-header-row > .cdk-column-name', 'name', 'general-rules', 'Clear filters');
    });
});
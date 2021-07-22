import * as common from '../../support/common';

describe('Trends', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/trends');
    });
    it('List', () => {
        cy.wait(1000);
        cy.screenshot('trends');
        cy.contains('Trends');
    });
});
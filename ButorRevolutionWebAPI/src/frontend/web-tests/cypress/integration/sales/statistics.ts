import * as common from '../../support/common';

describe('Statistics', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/statiscs');
    });
    it('List statistics', () => {
        cy.wait(1000);
        cy.screenshot('statistics');
        cy.contains('Statistics');
    });
});
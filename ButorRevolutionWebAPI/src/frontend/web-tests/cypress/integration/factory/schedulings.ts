import * as common from '../../support/common';

describe('Schedulings', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/optimizations');
        cy.wait(1000);
    });
    it('List schedulings', () => {
        cy.screenshot('schedulings');
    });
    it('Filter Shift number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-shiftNumber','shiftNumber', 'schedulings', 'Clear filters');
    });
    it('Filter Shift length', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-shiftLength','shiftLength', 'schedulings', 'Clear filters');
    });
});
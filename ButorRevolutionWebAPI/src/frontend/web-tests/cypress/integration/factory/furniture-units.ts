import * as common from '../../support/common';

describe('Furniture units', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/furniture-units');
        cy.wait(1000);
    });
    it('List furniture units', () => {
        cy.screenshot('furniture-units_list');
    });
    it('Filter Description', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-description', 'description', 'furniture-units', 'Clear filters');
    });
    it('Filter Code', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-code', 'code', 'furniture-units', 'Clear filters');
    });
});
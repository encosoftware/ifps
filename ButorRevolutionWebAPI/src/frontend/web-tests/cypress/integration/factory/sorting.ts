import * as common from '../../support/common';

describe('Plans/Sorting', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/sorting');
        cy.wait(1000);
    });
    it('List sortings', () => {
        cy.screenshot('sortings_list');
    });
    it('Start sorting', () => {
        cy.get('.btn').first().click();
    });
    it('Filter Unit name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-unitId','unitId', 'sorting', 'Clear filters');
    });
    it('Filter Order name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-orderId','orderId', 'sorting', 'Clear filters');
    });
    it('Filter Working number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-workingNr','workingNr', 'sorting', 'Clear filters');
    });
    it('Filter Worker name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-workerName','workerName', 'sorting', 'Clear filters');
    });
});
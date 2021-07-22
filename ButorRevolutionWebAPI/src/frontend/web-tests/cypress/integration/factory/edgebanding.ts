import * as common from '../../support/common';

describe('Plans/Edgebanding', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/edgebanding');
        cy.wait(1000);
    });
    it('List endgebandings', () => {
        cy.screenshot('edgebandings_list');
    });
    it('Start edgebanding', () => {
        cy.get('.btn').first().click();
    });
    it('Filter Component', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-componentId > b','componentId', 'edgebanding', 'Clear filters');
    });
    it('Filter Order name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-orderId','orderId', 'edgebanding', 'Clear filters');
    });
    it('Filter Working number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-workingNr','workingNr', 'edgebanding', 'Clear filters');
    });
    it('Filter Worker name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-workerName','workerName', 'edgebanding', 'Clear filters');
    });
});
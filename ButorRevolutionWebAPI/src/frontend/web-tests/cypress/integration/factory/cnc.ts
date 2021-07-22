import * as common from '../../support/common';

describe('Plans/CNC', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/cnc');
        cy.wait(1000);
    });
    it('List cnc', () => {
        cy.screenshot('cnc_list');
    });
    it('Start cnc', () => {
        cy.get('.btn').first().click();
    });
    it('Filter Component', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-componentId > b','componentId', 'cnc', 'Clear filters');
    });
    it('Filter Material code', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-material','material', 'cnc', 'Clear filters');
    });
    it('Filter Order name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-orderId','orderId', 'cnc', 'Clear filters');
    });
    it('Filter Working number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-workingNr','workingNr', 'cnc', 'Clear filters');
    });
    it('Filter Worker name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-workerName','workerName', 'cnc', 'Clear filters');
    });
});
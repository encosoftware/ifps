import * as common from '../../support/common';

describe('Plans/Cutting', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/cuttings');
        cy.wait(1000);
    });
    it('List cuttings', () => {
        cy.screenshot('cuttings_list');
    });
    it('Filter Machine', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-machine > b','machine', 'cuttings', 'Clear filters');
    });
    it('Filter Material code', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-material','material', 'cuttings', 'Clear filters');
    });
    it('Filter Order name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-orderId','orderId', 'cuttings', 'Clear filters');
    });
    it('Filter Working number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-workingNr','workingNumber', 'cuttings', 'Clear filters');
    });
    it('Filter Worker name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-workerName','workerName', 'cuttings', 'Clear filters');
    });
});
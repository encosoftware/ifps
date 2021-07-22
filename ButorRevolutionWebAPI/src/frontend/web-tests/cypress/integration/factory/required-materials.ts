import * as common from '../../support/common';

describe('Required materials', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/supply/orders');
        cy.wait(1000);
    });
    it('List required materials', () => {
        cy.screenshot('required-materials_list');
    });
    it('Filter Code', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-orderId > b', 'orderId', 'required-materials');
    });
    it('Filter Working number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-workingNumber', 'workingNumber', 'required-materials');
    });
});
import * as common from '../../support/common';

describe('Stocked items', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/stock/stockeditems');
        cy.wait(1000);
    });
    it('List stocked items', () => {
        cy.screenshot('stocked-items_list');
    });
    it('Add new storage', () => {
        common.getContainsClick('button', 'Add new item', true);
        cy.wait(1000);
        cy.screenshot('add_stocked-cell_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'create');
            common.setupBackendProxy('POST', 'api/stocks', 'postCall');
            common.withinNgSelectName('code');
            common.withinNgSelectName('cell');
            common.getNameClearType('amount', '1');
            cy.screenshot('add_stocked-cell_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Edit storage', () => {
        common.setupBackendProxy('GET', 'api/stocks/**', 'getCall');
        cy.get(':nth-child(1) > .cdk-column-description > b').click();
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_stocked-item');
    });
    it('Filter Description', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-description > b', 'description', 'stocked-item');
    });
    it('Filter Code', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-code', 'code', 'stocked-item');
    });
});
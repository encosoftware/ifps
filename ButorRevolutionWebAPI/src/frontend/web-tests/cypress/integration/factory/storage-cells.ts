import * as common from '../../support/common';

describe('Storage cells', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/stock/cells');
        cy.wait(1000);
    });
    it('List storage cells', () => {
        cy.screenshot('storage-cells_list');
    });
    it('Add new storage cell', () => {
        common.getContainsClick('button', 'Create new', true);
        cy.wait(1000);
        cy.screenshot('add_storage-cell_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'new');
            common.setupBackendProxy('POST', 'api/storagecells', 'postCall');
            common.withinNgSelectName('stock');
            common.getNameClearType('name', 'tesztname');
            common.getNameClearType('description', 'tesztdescription');
            cy.screenshot('add_storage-cell_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Edit storage cell', () => {
        common.setupBackendProxy('GET', 'api/storagecells/**', 'getCall');
        cy.get('tbody > :nth-child(1) > .cdk-column-name').click();
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_storage-cell');
    });
    it('Filter Name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-name', 'name', 'storage-cell');
    });
    it('Filter Stock', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-stock', 'stock', 'storage-cell');
    });
});
import * as common from '../../support/common';

describe('Storages', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/stock/stocks');
        cy.wait(1000);
    });
    it('List storages', () => {
        cy.screenshot('storages_list');
    });
    it('Add new storage', () => {
        common.getContainsClick('button', 'New storage', true);
        cy.wait(1000);
        cy.screenshot('add_storage_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'new');
            common.setupBackendProxy('POST', 'api/storages', 'postCall');
            common.getNameClearType('name', common.generateRandomName());
            common.withinNgSelectName('countryId');
            common.getNameClearType('postCode', '3000');
            common.getNameClearType('city', 'Budapest');
            common.getNameClearType('address', '3rd street');
            cy.screenshot('add_storage_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Edit storage', () => {
        common.setupBackendProxy('GET', 'api/storages/**', 'getCall');
        cy.get('tbody > :nth-child(1) > .cdk-column-name').click();
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_storage');
    });
    it('Filter Name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-name', 'name', 'storage');
    });
    it('Filter Address', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-address', 'address', 'storage');
    });
});
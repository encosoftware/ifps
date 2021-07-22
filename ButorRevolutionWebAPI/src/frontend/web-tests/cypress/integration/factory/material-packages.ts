import * as common from '../../support/common';

describe('Material packages', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/admin/material-packages');
        cy.wait(1000);
    });
    it('List material packages', () => {
        cy.screenshot('material-packages_list');
    });
    it('Add new package', () => {
        common.getContainsClick('button', 'New package', true);
        cy.wait(1000);
        cy.screenshot('add_package_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3','new');
            common.setupBackendProxy('POST', 'api/materialpackages', 'postCall');
            common.getNameClearType("code", common.generateRandomName());
            common.withinNgSelectName("materialId");
            common.withinNgSelectName("supplierId");
            common.getNameClearType("description", "description");
            common.getNameClearType("price", "5");
            common.getNameClearType("size", "5");
            common.withinNgSelectName("currencyId");
            cy.screenshot('add_package_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Filter Code', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-code', 'code', 'material-packages', 'Clear filter');
    });
    it('Filter Working number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-description', 'description', 'material-packages', 'Clear filter');
    });
});
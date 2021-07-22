import * as common from '../../support/common';

describe('Webshop furnitureunits', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/wfurnitureunits');
        common.setupBackendProxy('POST', '/api/webshopfurnitureunits', 'postCall');

    });
    it('List units', () => {
        cy.wait(1000);
        cy.screenshot('wfurnitureunits');
        cy.contains('WEBSHOP FURNITUREUNITS');
    });
    it('Add new unit', () => {
        common.getContainsClick('button', 'Add new');
        cy.wait(1000);
        cy.screenshot('empty_add_furnitureunit');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('.dialog-title', 'new');
            common.withinNgSelectName('furnitureUnit');
            common.getNameClearType('price', '10');
            common.getContainsClick('button', 'Save', true);
            common.waitBackend('postCall');
        });
    });
    it('Edit unit', () => {
        common.setupBackendProxy('GET', '/api/webshopfurnitureunits/**', 'getCall');
        cy.get(':nth-child(1) > .cdk-column-clearFilter > butor-btn-hamburger > .btn').click();
        cy.get('[iconclass="icon icon-edit"] > .hamburger-menu-list-item > button').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('.dialog-title', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_unit');
    });
    it('Filter Code', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-code', 'code', 'webshopfurnitureunits');
    });
    it('Filter Description', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-description', 'description', 'webshopfurnitureunits');
    });
});
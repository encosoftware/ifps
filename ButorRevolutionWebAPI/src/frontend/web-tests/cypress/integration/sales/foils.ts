import * as common from '../../support/common';

describe('Materials/Foils', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/materials/foils');
        cy.wait(1000);
    });
    it('List foils', () => {
        cy.screenshot('foils_list');
        common.containsCaseInsensitive('h3','FOILS');
    });
    it('Add new foil', () => {
        cy.contains('Add new').click();
        cy.wait(1000);
        cy.screenshot('add_foil_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('.dialog-title', 'new');
            common.setupBackendProxy('POST', 'api/foils', 'postCall');
            common.uploadTestImage("logo.jpg");
            common.getNameClearType("code", common.generateRandomName());
            common.getNameClearType("description", "description");
            common.getNameClearType("transactionPrice", "5");
            common.getNameClearType("purchasingPrice", "5");
            common.withinNgSelectName("currency");
            common.getNameClearType("thickness", "5");
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
            cy.screenshot('add_foil_filled');
        });
    });
    it('Edit foil', () => {
        common.setupBackendProxy('GET', '/api/foils/**', 'getCall');
        cy.get(':nth-child(1) > .cdk-column-clearFilter > butor-btn-hamburger > .btn').click();
        cy.get('[iconclass="icon icon-edit"] > .hamburger-menu-list-item > button').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('.dialog-title', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_foil');
    });
    it('Filter Code', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-code > butor-tooltip', 'code', 'foils');
    });
    it('Filter Description', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-description', 'description', 'foils');
    });
});
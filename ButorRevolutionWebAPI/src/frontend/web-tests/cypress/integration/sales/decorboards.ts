import * as common from '../../support/common';

describe('Materials/Decorboards', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/materials/decorboards');
        cy.wait(1000);
    });
    it('List decorboards', () => {
        cy.screenshot('decorboards_list');
        common.containsCaseInsensitive('h3','DECORBOARDS');
    });
    it('Add new decorboard', () => {
        cy.contains('Add new').click();
        cy.wait(1000);
        cy.screenshot('add_decorboard_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('.dialog-title', 'new');
            common.setupBackendProxy('POST', 'api/decorboards', 'postCall');
            common.uploadTestImage("logo.jpg");
            common.getNameClearType("code", common.generateRandomName());
            common.getNameClearType("description", "description");
            common.getNameClearType("transactionPrice", "5");
            common.getNameClearType("purchasingPrice", "5");
            common.withinNgSelectName("currency");
            common.withinNgSelectName("category");
            common.getNameClearType("length", "5");
            common.getNameClearType("width", "5");
            common.getNameClearType("thickness", "5");
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
            cy.screenshot('add_decorboard_filled');
        });
    });
    it('Edit decorboard', () => {
        common.setupBackendProxy('GET', '/api/decorboards/**', 'getCall');
        cy.get(':nth-child(1) > .cdk-column-clearFilter > butor-btn-hamburger > .btn').click();
        cy.get('[iconclass="icon icon-edit"] > .hamburger-menu-list-item > button').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('.dialog-title', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_decorboard');
    });
    it('Filter Code', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-code > butor-tooltip', 'code', 'decorboards');
    });
    it('Filter Description', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-description', 'description', 'decorboards');
    });
});
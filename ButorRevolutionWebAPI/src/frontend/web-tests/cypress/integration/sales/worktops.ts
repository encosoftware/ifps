import * as common from '../../support/common';

describe('Materials/Work tops', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/materials/worktops');
        cy.wait(1000);
    });
    it('List work tops', () => {
        cy.screenshot('worktops_list');
        common.containsCaseInsensitive('h3','WORK TOPS');
    });
    it('Add new worktop', () => {
        cy.contains('Add new').click();
        cy.wait(1000);
        cy.screenshot('add_worktop_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('.dialog-title', 'new');
            common.setupBackendProxy('POST', 'api/worktopboards', 'postCall');
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
            cy.screenshot('add_worktop_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Edit worktop', () => {
        common.setupBackendProxy('GET', '/api/worktopboards/**', 'getCall');
        cy.get(':nth-child(1) > .cdk-column-clearFilter > butor-btn-hamburger > .btn').click();
        cy.get('[iconclass="icon icon-edit"] > .hamburger-menu-list-item > button').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('.dialog-title', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_worktop');
    });
    it('Filter Code', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-code > butor-tooltip', 'code', 'worktops');
    });
    it('Filter Description', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-description', 'description', 'worktops');
    });
});
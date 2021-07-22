import * as common from '../../support/common';

describe('Materials/Accessories', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/materials/accessories');
        cy.wait(1000);
    });
    it('List accessories', () => {
        cy.screenshot('accessories_list');
        common.containsCaseInsensitive('h3','ACCESSORIES');
    });
    it('Add new accessory', () => {
        cy.contains('Add new').click();
        cy.wait(1000);
        cy.screenshot('add_accessory_empty');
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('.dialog-title', 'new');
            common.setupBackendProxy('POST', 'api/accessories', 'postCall');
            common.uploadTestImage("logo.jpg");
            common.getNameClearType("code", common.generateRandomName());
            common.getNameClearType("description", "description");
            common.getNameClearType("transactionPrice", "5");
            common.withinNgSelectName("category");
            common.withinNgSelectName("currency");
            cy.screenshot('add_accessory_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Edit accessory', () => {
        common.setupBackendProxy('GET', '/api/accessories/**', 'getCall');
        cy.get(':nth-child(1) > .cdk-column-clearFilter > butor-btn-hamburger > .btn').click();
        cy.get('[iconclass="icon icon-edit"] > .hamburger-menu-list-item > button').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('.dialog-title', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_accessory');
    });
    it('Filter Code', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-code > butor-tooltip', 'code', 'accessories');
    });
    it('Filter Description', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-description', 'description', 'accessories');
    });
    it('Filter Structurally optional', () =>{
        common.filterHelperNgSelectWithInputText('true', 'structurallyOptional', 'accessories')
    });
    it('Filter Optionally mounting', () =>{
        common.filterHelperNgSelectWithInputText('true', 'optMount', 'accessories')
    });
    it('Filter Category', () =>{
        common.filterHelperNgSelectWithInputText('Accessories', 'categoryId', 'accessories')
    });
});
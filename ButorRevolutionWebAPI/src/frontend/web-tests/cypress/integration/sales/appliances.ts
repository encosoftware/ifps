import * as common from '../../support/common';

describe('Materials/Appliances', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/materials/appliances');
        cy.wait(1000);
    });
    it('List appliances', () => {
        cy.screenshot('appliances_list');
        common.containsCaseInsensitive('h3','APPLIANCES');
    })
    it('Add new appliance', () => {
        cy.contains('Add new').click();
        cy.wait(1000);
        cy.screenshot('add_appliance_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'new');
            common.setupBackendProxy('POST', 'api/appliances', 'postCall');
            common.uploadTestImage("logo.jpg");
            common.getNameClearType("code",common.generateRandomName());
            common.getNameClearType("hanaCode", "hana_testcode");
            cy.get(`textarea[name="description"]`).clear().type("teszt_description");
            common.withinNgSelectName("brand");
            common.getNameClearType("purchasingPrice", "5");
            common.getNameClearType("sellPrice", "5");
            common.withinNgSelectName("category");
            common.withinNgSelectName("purchasingCurrency");
            common.withinNgSelectName("sellingCurrency");
            cy.screenshot('add_appliance_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Edit appliance', () => {
        common.setupBackendProxy('GET', '/api/appliances/**', 'getCall');
        cy.get(':nth-child(1) > .cdk-column-clearFilter > butor-btn-hamburger > .btn').click();
        cy.get('[iconclass="icon icon-edit"] > .hamburger-menu-list-item > button').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('h3', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_appliance');
    });
    it('Filter Code', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-code > butor-tooltip', 'code', 'appliances');
    });
    it('Filter Description', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-description', 'description', 'appliances');
    });
    it('Filter Category', () => {
        common.filterHelperNgSelect('tbody > :nth-child(1) > .cdk-column-category', 'categoryId', 'appliances');
    });
    it('Filter Brand', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-brand', 'brand', 'appliances');
    });
});
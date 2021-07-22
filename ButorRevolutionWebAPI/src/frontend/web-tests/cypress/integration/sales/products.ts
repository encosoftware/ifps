import * as common from '../../support/common';

describe('Products', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/products');
    });
    it('List products', () => {
        cy.wait(1000);
        cy.screenshot('products');
        cy.contains('PRODUCTS');
    });
    it('Add new product', () => {
        common.getContainsClick('button', 'Add New');
        cy.wait(1000);
        cy.screenshot('product_add');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('.dialog-title', 'new');
            common.setupBackendProxy('POST', '/api/furnitureunits', 'postCall');
            common.uploadTestImage("logo.jpg");
            common.getNameClearType("code", common.generateRandomName());
            common.getNameClearType("description", "description");
            common.getNameClearType("height", "5");
            common.getNameClearType("width", "5");
            common.getNameClearType("depth", "5");
            common.withinNgSelectName("category");
            common.getContainsClick('button', 'Save');
            common.waitBackendAndScreenshot('postCall', 'edit_product');
        });
    });
});
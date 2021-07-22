import * as common from '../../support/common';

describe('Companies', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('admin/companies');
        common.setupBackendProxy('POST', '/api/companies', 'postCall');
    });
    it('List companies', () => {
        cy.wait(1000);
        cy.screenshot('companies_list');
        cy.contains('COMPANIES');
    });
    it('Add new company', () => {
        common.getContainsClick("button", "Add new company");
        cy.wait(1000);
        cy.screenshot('add_company_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'new');
            common.getNameClearType("name", "Test Company");
            common.withinNgSelectName("companyType");
            common.getNameClearType("taxnumber", "123 456 789");
            common.getNameClearType("registernumber", "987 654 321");
            common.withinNgSelectName("country");
            common.getNameClearType("postCode", "1094");
            common.getNameClearType("city", "Budapest");
            common.getNameClearType("address", "Bocskai út 77-79");
            cy.screenshot('add_company_filled');
            common.getContainsClick("button", "Add", true);
            common.waitBackendAndScreenshot('postCall', 'edit_company');
        });
    });
    it('Delete company', () => {
        common.request('api/companies',
            'POST',
            { "name": "törlésre", "companyTypeId": 1, "taxNumber": "1234", "registerNumber": "1234", "address": { "address": "teszt", "postCode": "123", "city": "teszt", "countryId": 1 } },
            false)
            .then(() => {
                common.getNameClearType('name', 'törlésre');
                cy.wait(1000);
                cy.get('.btn-hamburger').first().click();
                cy.screenshot('edit_company_hamburger');
                common.getContainsClick('button', 'Delete', true);
                cy.wait(2000);
                common.getNameClearType('name', 'törlésre');
                cy.wait(1000);
                cy.contains('No records found');
                cy.screenshot('company_deleted');
            });
    });
    it('Filter Name', () => {
        common.filterHelper('[ng-reflect-router-link="/admin/companies/edit/,1"] > .cdk-column-name', 'name', 'companies');
    });
    it('Filter Company type', () => {
        common.filterHelperNgSelect('[ng-reflect-router-link="/admin/companies/edit/,1"] > .cdk-column-companyType', 'companyType', 'companies');
    });
    it('Filter Headquarters (address)', () => {
        common.filterHelper('[ng-reflect-router-link="/admin/companies/edit/,1"] > .cdk-column-address', 'address', 'companies');
    });
});
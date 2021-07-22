import * as common from '../../support/common';

describe('Venues', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/venues');
    });
    it('List venues', () => {
        cy.wait(1000);
        cy.screenshot('venues_list');
        cy.contains('VENUES');
    })
    it('Add venue', () => {
        common.getContainsClick("button", "Add new venues", true);
        cy.wait(1000);
        cy.screenshot('empty_add_venue_dialog');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'new');
            common.setupBackendProxy('POST', 'api/venues', 'postCall');
            common.getNameClearType("name", "IKEA Örs vezér tere");
            common.getNameClearType("phone", "+36-1/460-3160");
            common.getNameClearType("email", "test@test.com");
            common.getRoleType("combobox", "Hungary");
            common.getContainsClick("span", "Hungary");
            common.getNameClearType("Postcode", "1148");
            common.getNameClearType("City", "Budapest");
            common.getNameClearType("Address", "Örs vezér tere 22.");
            cy.screenshot('filled_add_venue_dialog');
            common.getContainsClick('button[type="submit"]', "Add");
        });
        common.waitBackendAndScreenshot('postCall', 'edit_venue_page');
    });
    it('Delete venue', () => {
        common.request('api/venues',
            'POST',
            { "name": "törlés teszt", "phoneNumber": "+36-1/460-3160", "email": "test@test.com", "companyId": 1, "officeAddress": { "address": "Örs vezér tere 23.", "postCode": "1148", "city": "Budapest", "countryId": 1 } },
            false)
            .then(() => {
                common.getNameClearType('name', 'törlés teszt');
                cy.wait(500);
                cy.get('.btn-hamburger').first().click();
                cy.screenshot('edit_venue_hamburger');
                common.getContainsClick('button', 'Delete', true);
                cy.wait(500)
                cy.get('.mat-simple-snackbar').contains('SUCCESS');
                cy.screenshot('venue_deleted');
            });
    });
});
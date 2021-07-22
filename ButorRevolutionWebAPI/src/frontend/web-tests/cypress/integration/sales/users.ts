import * as common from '../../support/common';

describe('Users', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/admin/users');
    });
    it('List users', () => {
        cy.wait(1000);
        cy.screenshot('users_list');
        common.containsCaseInsensitive('h3','Users');
    });
    it('Add user', () => {
        common.getContainsClick('button', 'Add new user', true);
        cy.wait(1000);
        cy.screenshot('add_user_empty');
        common.setupBackendProxy('POST', '/api/users', 'postCall');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('h3', 'new');
            cy.get('input[name="name"]').type(common.generateRandomName());
            cy.get('input[name="phoneNumber"]').type('+36-1-460-3160');
            cy.get('input[name="email"]').type('test2' + Math.floor(Math.random() * 10000) + '@test.com');
            cy.get('input[role="combobox"]').type('Admin');
            cy.get('span').contains('Admin').click();
            cy.screenshot('add_user_filled');
            cy.get('button').contains('Add').click();
            common.waitBackendAndScreenshot('postCall', 'edit_user');
        });
    });
    it('Filter by name', () => {
        common.filterHelper('[ng-reflect-router-link="/admin/users,1"] > .cdk-column-name > butor-tooltip', 'name', 'users');
    });
});
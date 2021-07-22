import * as common from '../../support/common';

describe('Roles', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('admin/roles');
        cy.wait(1000);
    });
    it('List roles', () => {
        cy.screenshot('roles');
        cy.contains('USER ROLES');
        common.getContainsClick("mat-panel-title", "Admin module", true);
        cy.get('div[class="mat-expansion-panel-body"]').within(() => {
            cy.contains("Admin").click({ force: true });
        });
        cy.wait(1000);
        cy.screenshot('admin_roles');
    });
    it('Save role', () => {
        cy.get('div[class="mat-expansion-panel-body"]').within(() => {
            cy.contains("Admin").click({ force: true });
        });
        cy.wait(1000);
        cy.get('.btn-primary').click();
        cy.wait(1000);
        cy.screenshot('saved_role');
        cy.get('.mat-simple-snackbar').contains('Role saved');
    });
});
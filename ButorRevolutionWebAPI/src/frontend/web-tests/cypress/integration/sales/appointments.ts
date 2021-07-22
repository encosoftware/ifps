import * as common from '../../support/common';

describe('Appointments', () => {
    beforeEach(() => {
        common.login(false);
        cy.visit('/sales/appointments');
    });
    it('Calendar', () => {
        cy.wait(1000);
        cy.screenshot('appointment_calendar');
        cy.contains('Appointments');
    });
    it('Show/hide weekends', () => {
        cy.wait(1000);
        common.getContainsClick('button', 'Show weekends');
        cy.screenshot('appointment_weekends');
        cy.get('button').contains('Hide weekends').should('be.visible');
    });
});
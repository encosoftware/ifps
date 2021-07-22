import * as common from '../../support/common';

describe('Workload', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/workload');
        cy.wait(1000);
    });
    it('Workload page', () => {
        cy.screenshot('workload_page');
    });
});
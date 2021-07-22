import * as common from '../../support/common';

describe('Cargo', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/supply/cargo');
        cy.wait(1000);
    });
    it('List cargos', () => {
        cy.screenshot('cargos_list');
    });
    it('Edit cargo', () => {
        common.setupBackendProxy('GET', 'api/cargos/details/1', 'getCall');
        cy.get('tbody > :nth-child(1) > .cdk-column-cargoId').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('h3','Edit');
        });
        cy.wait('@getCall').then((xhr) => {
            assert.equal(xhr.status, 200);
            cy.screenshot('edit_cargo_page');
        });
    });
    it('Filter Name', () => {
        common.filterHelper(':nth-child(1) > .cdk-column-cargoId > b', 'cargoId', 'cargo');
    });
    it('Filter Status', () => {
        common.filterHelperNgSelect(':nth-child(1) > .cdk-column-status > .cargo-status', 'status', 'cargo');
    });
});
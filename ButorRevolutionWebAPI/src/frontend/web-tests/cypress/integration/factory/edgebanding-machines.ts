import * as common from '../../support/common';

describe('Machines/Edging', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/machines/edgings');
        cy.wait(1000);
    });
    it('List edging machines', () => {
        cy.screenshot('edgings-machine_list');
    }); 
    it('Add new edging machine', () => {
        common.getContainsClick('button', 'Add New', true);
        cy.wait(1000);
        cy.screenshot('add_edgings-machine_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('.dialog-title', 'new');
            common.setupBackendProxy('POST', 'api/edgingmachines', 'postCall');
            common.getNameClearType("machineName", common.generateRandomName());
            common.getNameClearType("serialNumber", "1")
            common.getNameClearType("yearOfManufacture", "2020")
            common.getNameClearType("softwareVersion", "2")
            common.getNameClearType("code", "2")
            common.withinNgSelectName("brandId");
            cy.screenshot('add_edgings-machine_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Edit edging machine', () => {
        common.setupBackendProxy('GET', '/api/edgingmachines/**', 'getCall');
        cy.get('tbody > :nth-child(1) > .cdk-column-machineName').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('.dialog-title', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_edging-machine');
    });
    it('Filter Machine name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-machineName','machineName', 'edging-machine', 'Clear filters');
    });
    it('Filter Software version', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-softwareVersion','softwareVersion', 'edging-machine', 'Clear filters');
    });
    it('Filter Serial number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-serialNumber','serialNumber', 'edging-machine', 'Clear filters');
    });
    it('Filter Code', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-code','code', 'edging-machine', 'Clear filters');
    });
    it('Filter Year of manufacture', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-yearOfManufacture','yearOfManufacture', 'edging-machine', 'Clear filters');
    });
});
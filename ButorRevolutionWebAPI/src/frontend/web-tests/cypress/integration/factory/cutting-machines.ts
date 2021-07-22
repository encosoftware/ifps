import * as common from '../../support/common';

describe('Machines/Cutting', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/machines/cuttings');
        cy.wait(1000);
    });
    it('List cutting machines', () => {
        cy.screenshot('cutting-machine_list');
    }); 
    it('Add new cutting machine', () => {
        common.getContainsClick('button', 'Add New', true);
        cy.wait(1000);
        cy.screenshot('add_cutting-machine_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('.dialog-title','new');
            common.setupBackendProxy('POST', 'api/cuttingmachines', 'postCall');
            common.getNameClearType("machineName", common.generateRandomName());
            common.getNameClearType("serialNumber", "1")
            common.getNameClearType("yearOfManufacture", "2020")
            common.getNameClearType("softwareVersion", "2")
            common.getNameClearType("code", "2")
            common.withinNgSelectName("brandId");
            cy.screenshot('add_cutting-machine_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Edit cutting machine', () => {
        common.setupBackendProxy('GET', '/api/cuttingmachines/**', 'getCall');
        cy.get('tbody > :nth-child(1) > .cdk-column-machineName').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('.dialog-title', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_cutting-machine');
    });
    it('Filter Machine name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-machineName','machineName', 'cutting-machine', 'Clear filters');
    });
    it('Filter Software version', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-softwareVersion','softwareVersion', 'cutting-machine', 'Clear filters');
    });
    it('Filter Serial number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-serialNumber','serialNumber', 'cutting-machine', 'Clear filters');
    });
    it('Filter Code', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-code','code', 'cutting-machine', 'Clear filters');
    });
    it('Filter Year of manufacture', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-yearOfManufacture','yearOfManufacture', 'cutting-machine', 'Clear filters');
    });
});
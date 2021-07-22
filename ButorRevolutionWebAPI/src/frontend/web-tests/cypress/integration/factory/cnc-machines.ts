import * as common from '../../support/common';

describe('Machines/CNC', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/machines/cncs');
        cy.wait(1000);
    });
    it('List cnc machines', () => {
        cy.screenshot('cnc-machine_list');
    }); 
    it('Add new cnc machine', () => {
        common.getContainsClick('button', 'Add New', true);
        cy.wait(1000);
        cy.screenshot('add_cnc-machine_empty');
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('.dialog-title','new');
            common.setupBackendProxy('POST', 'api/cncmachines', 'postCall');
            common.getNameClearType("machineName", common.generateRandomName());
            common.getNameClearType("sharedFolderPath", "path")
            common.getNameClearType("serialNumber", "1")
            common.getNameClearType("yearOfManufacture", "2020")
            common.getNameClearType("softwareVersion", "2")
            common.getNameClearType("code", "2")
            common.withinNgSelectName("brandId");
            cy.screenshot('add_cnc-machine_filled');
            common.getContainsClick("button", "Save");
            common.waitBackend('postCall');
        });
    });
    it('Edit cnc machine', () => {
        common.setupBackendProxy('GET', '/api/cncmachines/**', 'getCall');
        cy.get('tbody > :nth-child(1) > .cdk-column-machineName').click();
        cy.get(".mat-dialog-container").within(() => {
            common.containsCaseInsensitive('.dialog-title', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_cnc-machine');
    });
    it('Filter Machine name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-machineName','machineName', 'cnc-machine', 'Clear filters');
    });
    it('Filter Software version', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-softwareVersion','softwareVersion', 'cnc-machine', 'Clear filters');
    });
    it('Filter Serial number', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-serialNumber','serialNumber', 'cnc-machine', 'Clear filters');
    });
    it('Filter Code', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-code','code', 'cnc-machine', 'Clear filters');
    });
    it('Filter Year of manufacture', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-yearOfManufacture','yearOfManufacture', 'cnc-machine', 'Clear filters');
    });
    // it('Filter Brand', () => {
    //     common.filterHelperNgSelect('tbody > :nth-child(1) > .cdk-column-brandId','brandId', 'cnc', 'Clear filters');
    // });
});
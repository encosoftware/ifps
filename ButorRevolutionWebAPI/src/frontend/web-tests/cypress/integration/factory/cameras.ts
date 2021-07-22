import * as common from '../../support/common';

describe('Camera', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/production/cameras');
        cy.wait(1000);
    });
    it('List cameras', () => {
        cy.screenshot('cameras_list');
    });
    it('Add new camera', () => {
        common.getContainsClick('button', 'Add new', true);
        cy.wait(1000);
        cy.screenshot('add_camera_empty');
        cy.get('.mat-dialog-container').within(async () => {
            common.containsCaseInsensitive('.dialog-title', 'new');
            common.setupBackendProxy('POST', 'api/cameras', 'postCall');
            common.getNameClearType("name", common.generateRandomName());
            common.getNameClearType("ipAddress", "10.10.10.10");
            common.getNameClearType("type", "IP");
            cy.screenshot('add_camera_filled');
            common.getContainsClick("button", "Save");
            return await common.waitBackend('postCall');
        }).then(async (xhr: any) => {
            const id = await Cypress.Blob.blobToBinaryString(xhr.responseBody as Blob);
            common.deleteRequest('api/cameras', id, true);
        });
    });
    it('Edit camera', () => {
        common.setupBackendProxy('GET', 'api/cameras/**', 'getCall');
        cy.get('tbody > :nth-child(1) > .cdk-column-name').click();
        cy.get('.mat-dialog-container').within(() => {
            common.containsCaseInsensitive('.dialog-title', 'edit');
        });
        common.waitBackendAndScreenshot('getCall', 'edit_camera');
    });
    it('Filter Name', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-name', 'name', 'camera');
    });
    it('Filter IP Address', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-ipAddress', 'ipAddress', 'camera');
    });
    it('Filter Type', () => {
        common.filterHelper('tbody > :nth-child(1) > .cdk-column-type', 'type', 'camera');
    });
});
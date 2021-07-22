import * as common from '../../support/common';

describe('Inspection', () => {
    beforeEach(() => {
        common.login(true);
        cy.visit('/stock/inspection');
        cy.wait(1000);
    });
    it('List inspections', () => {
        cy.screenshot('inspection_list');
    });
    it('Filter Stock', () => {
        common.filterHelperNgSelect('[ng-reflect-router-link="/stock/inspection/,1,report"] > .cdk-column-stock', 'stock', 'inspection', 'Clear filter');
    });
    it('Filter Report', () => {
        common.filterHelper('[ng-reflect-router-link="/stock/inspection/,1,report"] > .cdk-column-report', 'report', 'inspection', 'Clear filter');
    });
    it('Filter Delegation', () => {
        common.filterHelper('[ng-reflect-router-link="/stock/inspection/,1,report"] > .cdk-column-delegation', 'delegation', 'inspection', 'Clear filter');
    });
});
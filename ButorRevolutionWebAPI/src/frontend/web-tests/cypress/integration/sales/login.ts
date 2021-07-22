import * as common from '../../support/common';

describe('Login', () => {
    beforeEach(() => {
        common.setupBackendProxy('POST', 'api/account/login', 'postLogin');
    });
    it('Successful UI login', () => {
        cy.screenshot('login');
        loginUI('enco@enco.hu', 'password');
        cy.wait('@postLogin')
            .then((xhr) => {
                assert.equal(xhr.status, 200);
                common.getShouldContains('a', 'be.visible', 'Logout');
            });
    });
    it('Successful API login', () => {
        loginCall('enco@enco.hu', 'password')
            .then((res) => {
                cy.setCookie('key', res.accessToken);
                cy.setCookie('key-r', res.refreshToken);
                cy.visit('/');
                common.getShouldContains('a', 'be.visible', 'Logout');
            });
    });
    it('Wrong email UI login', () => {
        loginUI('failed@enco.hu', 'password');
        cy.wait('@postLogin')
            .then((res) => {
                assert.equal(res.status, 400);
                cy.screenshot('invalid_login');
                common.getShouldContains('span', 'be.visible', 'Email or Password is incorrect!');
            });
    });
    it('Wrong email API login', () => {
        loginCall('failed@failed.email', 'password')
            .then((res) => {
                assert.equal(res.message, "User doesn't exist");
            });
    });
    it('Log in button is disabled when email and/or password is empty', () => {
        cy.visit('/login');
        common.clear_field('emailL')
        common.clear_field('password')
        cy.contains('button', 'Log in').should('be.disabled');

        common.getNameClearType('emailL', 'enco@enco.hu');
        cy.contains('button', 'Log in').should('be.disabled');

        common.clear_field('emailL')
        common.getNameClearType('password', 'password');
        cy.contains('button', 'Log in').should('be.disabled');
    });
    afterEach(() => {
        cy.clearCookies();
        cy.visit('/');
    });
});

function loginCall(email: string, password: string) {
    return cy.request({
        url: 'https://localhost:44348/api/account/login',
        method: 'POST',
        failOnStatusCode: false,
        body: { "email": email, "password": password, "rememberMe": true }
    }).its('body');
}

function loginUI(email: string, password: string) {
    cy.visit('login');
    common.getNameClearType('emailL', email);
    common.getNameClearType('password', password);
    common.getContainsClick('button', 'Log in', true);
}
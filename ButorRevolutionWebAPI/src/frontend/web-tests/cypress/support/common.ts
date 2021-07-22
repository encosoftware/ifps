import "cypress-file-upload";

const apiUrl = 'https://localhost:44348/';

export const setupBackendProxy = (method: string, url: string, name: string) => {
    cy.server();
    cy.route({
        method: method,
        url: url
    }).as(name);
};

export const waitBackend = (name: string) => {
    return new Cypress.Promise((resolve) => {
        cy.wait('@' + name).then((xhr) => {
            assert.equal(xhr.status, 200);
            resolve(xhr);
        });
    });
};

export const generateRandomName = () => {
    return "Test-Name " + Math.floor(Math.random() * 10000);
}

export const clear_field = (getText: string) => {
    cy.get(`input[name="${getText}"]`)
        .clear();
}

export const waitBackendAndScreenshot = (name: string, screenshotName: string) => {
    cy.wait('@' + name).then((xhr) => {
        assert.equal(xhr.status, 200);
        cy.screenshot(screenshotName);
    });
};

export const login = (isFactory: boolean) => {
    cy.request({
        url: (isFactory ? Cypress.env('factoryUrl') : Cypress.env('salesUrl')) + 'api/account/login',
        method: 'POST',
        failOnStatusCode: false,
        body: { "email": 'enco@enco.hu', "password": 'password', "rememberMe": true }
    }).its('body').then((res) => {
        cy.setCookie('key', res.accessToken);
        cy.setCookie('key-r', res.refreshToken);
    });
};

export const deleteRequest = (endpoint: string, id: any, isFactory: boolean) => {
    return cy.getCookie('key')
        .then((cookie) => {
            const token = cookie?.value;
            return cy.request({
                url: (isFactory ? Cypress.env('factoryUrl') : Cypress.env('salesUrl')) + endpoint + '/' + id,
                headers: {
                    'Content-Type': 'application/json',
                    'authorization': 'Bearer ' + token
                },
                method: 'DELETE',
                failOnStatusCode: false
            });
        });
};

export const request = (endpoint: string, method: string, body: any, isFactory: boolean) => {
    return cy.getCookie('key')
        .then((cookie) => {
            const token = cookie?.value;
            return cy.request({
                url: isFactory ? Cypress.env('factoryUrl') : Cypress.env('salesUrl') + endpoint,
                headers: {
                    'Content-Type': 'application/json',
                    'authorization': 'Bearer ' + token
                },
                method: method,
                failOnStatusCode: false,
                body: body
            });
        });
};

export const getNameClearType = (getNameText: string, typeText: string) => {
    cy.get(`input[name="${getNameText}"]`)
        .clear()
        .type(typeText);
};

export const containsCaseInsensitive = (getText: string, containsText: string) => {
    cy.get(getText).contains(containsText);
};

export const getRoleType = (getText: string, typeText: string) => {
    cy.get(`input[role="${getText}"]`).type(typeText);
};

export const getShouldContains = (getText: string, shouldText: string, containsText: string) => {
    cy.get(getText)
        .should(shouldText)
        .contains(containsText);
};

export const getClick = (getText: string, force?: boolean) => {
    cy.get(getText).click(force ? { force: force } : { force: false });
};

export const getContainsClick = (getText: string, containsText: string, forceBoolean?: boolean) => {
    cy.get(getText)
        .contains(containsText)
        .click(forceBoolean ? { force: forceBoolean } : { force: false });
};

export const withinNgSelectName = (getText: string) => {
    cy.get(`ng-select[name="${getText}"]`).within(() => {
        cy.get('input[role=combobox]').first().clear();
        cy.get('span.ng-option-label.ng-star-inserted').eq(0).click();
    });
};

export const uploadTestImage = (fileName: string) => {
    cy.fixture(fileName).then(fileContent => {
        cy.get('input[accept="image/*"]').upload(
            {
                fileContent: fileContent,
                fileName: fileName,
                mimeType: "image/jpeg"
            },
            { subjectType: "input" }
        );
    });
};

export const filterHelper = (getText: string, filter: string, photoName: string, clearFilters: string = "Clear filter") => {
    cy.get(getText).invoke('text').then((text => {
        if (text.toString().trim() == "") {
            return true;
        }
        cy.screenshot(`before_${photoName}_filter_${filter}`);
        getNameClearType(filter, text.toString().trim());
        cy.wait(1000);
        cy.screenshot(`after_${photoName}_filter_${filter}`);
        cy.get(getText).invoke('text').then((text2) => {
            assert.equal(text.toString(), text2.toString());
        });
        getContainsClick("a", clearFilters);
    }));
};

export const filterHelperNgSelect = (getText: string, filter: string, photoName: string, clearFilters: string = "Clear filter") => {
    cy.get(getText).invoke('text').then((text => {
        cy.screenshot(`before_${photoName}_filter_${filter}`);
        withinNgSelectName(filter);
        cy.wait(1000);
        cy.screenshot(`after_${photoName}_filter_${filter}`);
        cy.get(getText).invoke('text').then((text2) => {
            assert.equal(text.toString(), text2.toString());
        });
        getContainsClick("a", clearFilters);
    }));
};

export const filterHelperNgSelectWithInputText = (inputText: string, filter: string, photoName: string, clearFilters: string = "Clear filter") => {
    cy.screenshot(`before_${photoName}_filter_${filter}`);
    withinNgSelectName(filter);
    cy.wait(1000);
    cy.screenshot(`after_${photoName}_filter_${filter}`);
    getContainsClick("a", clearFilters);
};
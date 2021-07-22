Cypress.Commands.overwrite('contains', (originalFn, subject, filter, text, options = {}) => {
    // determine if a filter argument was passed
    if (typeof text === 'object') {
        options = text;
        text = filter;
        filter = undefined;
    }

    options.matchCase = false;

    return originalFn(subject, filter, text, options);
});
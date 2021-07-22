import { expect } from "chai";
import { Given, Then, When } from "cucumber";
import { browser, by, element, $ } from "protractor";
import { CategoryPageObj } from "../pages/categories";
import { nextTick } from "q";

const pageUrl = browser.baseUrl + "/admin/categories/";
const categoryPage = new CategoryPageObj();

Given("I am on the {string} page", { timeout: 2 * 5000 }, async (text) => {
    if (text === "Categories") {
        await browser.get(pageUrl);
        categoryPage.getHeaderCategoryPage().getText()
            .then((header) => expect(header).to.equal('CATEGORIES'))
            .catch((er) => { throw er });
    }
});

When("I click on {string} button", async (text) => {
    if (text === "Category") {
        await categoryPage.getMainCat("Fruit").click();
    } else if (text === "Hamburger") {
        await categoryPage.getHamburgerMenu().get(1).click();
    } else if (text === "Edit") {
        await categoryPage.getEditBtn().click();
    }
});

Then("I get SubCategories", async () => {
    await browser.wait(
        categoryPage.getTree().getText()
            .then((el) => expect(el).to.equal("Apple 0"))
            .catch((er) => { throw er }), 5000);

});


Then("I see the {string} popup", (text) => {

    categoryPage.getpopUpisPresent().then((pres) => {
        expect(pres).to.equal(true);
    }).catch((er) => { throw er });
});

Given("I am on the {string} popup", async (text) => {
    const present = await categoryPage.getPopupHeaderText();
    expect(present).to.be.equals(text);
});

When("I modify {string} from {string} to {string}", async (string, string2, string3) => {
    await categoryPage.getInput(string).clear();
    await categoryPage.getInput(string).sendKeys(string3);
});

When("I click on the {string} button", async (text) => {
    await categoryPage.getClickBtn(text);
});

Then("the modifications are sent to the DB", async () => {
    const present = await categoryPage.isPresentPopupHeader();
    expect(present).to.be.equals(false);
});

When('{string} is filled with {string}', async (text, string2) => {
    await categoryPage.getInput(text).sendKeys(string2);
});

When('in the {string} dropdown {string} is selected', async (string, string2) => {
    // Write code here that turns the phrase above into concrete actions
    try {
        await browser.wait(categoryPage.getSelectPopup("parent"), 3000);
        await categoryPage.getOptionSelect("option");
        await categoryPage.selectedElement("Fruit");
    } catch (err) {
       throw err
    }
});

Then('I should see the {string} page', async (string) => {
    // Write code here that turns the phrase above into concrete actions
    const present = await categoryPage.isPresentPopupHeader();
    expect(present).to.be.equals(false);
});

Then('a snackbar {string} appears', async (string) => {
    // Write code here that turns the phrase above into concrete actions
    const present = await categoryPage.isPresentSnackBar();
    expect(present).to.be.equals(true);

});

Then('a popup appears titled {string}', function (string) {
    // Write code here that turns the phrase above into concrete actions
    return 'pending';
});

When('the {string} is equal to {string}', async (string, string2) => {
    // Write code here that turns the phrase above into concrete actions
        await browser.wait(categoryPage.getSelectPopup(string), 3000)
        await categoryPage.getOptionSelect("option");
        await categoryPage.selectedElement(string2);        
});

Given('the dropdown is open', async () => {
    // Write code here that turns the phrase above into concrete actions
    await categoryPage.getHamburgerMenu().get(1).click();
    const present = await categoryPage.isPresentHamburger();
    expect(present).to.be.equals(true);
});


Given('the cursor is on the {string} subcategory\'s row', function (string) {
    // Write code here that turns the phrase above into concrete actions
    return 'pending';
});

Then('I see the {string} page', function (string) {
    // Write code here that turns the phrase above into concrete actions
    return 'pending';
});


When('I haven\'t filled any of the required fields', function () {
    // Write code here that turns the phrase above into concrete actions
    return 'pending';
});

When('I have filled some of the required fields', async () => {
    // Write code here that turns the phrase above into concrete actions
    await categoryPage.getInput("Eng").clear();
    await categoryPage.getInput("Eng").sendKeys('string3');
});

Given('I see a popup {string}', function (string) {
    // Write code here that turns the phrase above into concrete actions
    return 'pending';
});

When('I click {string}', function (string) {
    // Write code here that turns the phrase above into concrete actions
    return 'pending';
});

Then('I return to the {string} page', async (text) => {
    // Write code here that turns the phrase above into concrete actions
    const present = await categoryPage.isPresentPopupHeader();
    expect(present).to.be.equals(false);
});
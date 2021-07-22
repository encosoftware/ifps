import { browser, $, element, By } from "protractor";
import { EditSubCategory } from "../../pages/Nw/editSub";
const { Given, When, Then } = require("cucumber");
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const pageUrl = browser.baseUrl + "/admin/categories/";
const edit: EditSubCategory = new EditSubCategory();
// senario 1
Given(/^I am on the "(.*?)" page$/, async () => {
    await browser.navigate().to(pageUrl);
    expect(browser.getTitle()).to.eventually.equal("FrontendAdmin");
});
Given(/^I click on "(.*?)" button$/, async () => {
    await edit.clickLink.click();
});
Given(/^the cursor is on the "(.*?)" subcategory's row$/, (text) => {
    expect(edit.subCat.getText().then(x => x === text));
});
When(/^I click on the Hamburger button$/, async () => {
    await edit.hamburger.click();
});
When(/^I see the "(.*?)" button$/, async (text: string) => {
    await(edit.edit.getText().then(x => expect(x).to.equal(text)));
});
When(/^I click on the Edit button$/, () => {
    const el = edit.bal();
    el.click();
});
Then(/^I see the "(.*?)" popup$/, (text: string) => {
    edit.popupHeader.getText().then(x => x === text);
});
Given('I am on the {string} popup', async (string) => {
    await edit.popupHeader.getText().then(x => { console.log(x); browser.sleep(9000); });
});
When("I modify {string} from {string} to {string}", (string, string2, string3) => {
    edit.engInput().sendKeys(string3);
});
When("I click the {string} button", (string) => {
    return "pending";
});
Then("the modifications are sent to the DB", () => {
    return "pending";
});
Feature: I CREATE A NEW SUBCATEGORY SUCCESSFULLY (TOP BUTTON)
    @CucumberScenario
	Scenario: I open the "New Subcategory" popup
		Given I am on the "Categories" page
		When I click on the "Add new item" button
		Then I am on the "NEW CATEGORY" popup
	@CucumberScenario
	Scenario: I save a new item
		Given I am on the "NEW CATEGORY" popup
		When "Eng" is filled with "Untreated Pine"
		And "Hu" is filled with "Kezeletlen fenyő"
		And in the "Parent" dropdown "Fruit" is selected
		And I click on the "Save" button
		Then I should see the "Categories" page
		And a snackbar "New Subcategory Created" appears

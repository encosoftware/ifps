Feature: I HIT THE CANCEL BUTTON, FILLED SOME FIELDS IN THE FORM, I DISCARD THE NEW SUBCATEGORY
	@CucumberScenario
	Scenario: I click on the edit button
		Given I am on the "Categories" page
		And I click on "Category" button
		When I click on "Hamburger" button
		And I click on "Edit" button
		Then I see the "NEW CATEGORY" popup
	@CucumberScenario
	Scenario: I hit the cancel button
		Given I am on the "NEW CATEGORY" popup
		When I have filled some of the required fields
		And I click on the "Cancel" button
		Then I return to the "Categories" page

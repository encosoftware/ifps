Feature: I HIT THE CANCEL BUTTON, FILLED SOME FIELDS IN THE FORM, I DISCARD THE NEW SUBCATEGORY
    @CucumberScenario
	Scenario: I hit the cancel button
		Given I am on the "New Subcategory" popup
		When I have filled some of the required fields
		And click on the "Cancel" button
		Then I see a popup "Are you sure?"
    @CucumberScenario
	Scenario: I discard the new item
		Given I see a popup "Are you sure?"
		When I click "Discard info"
		Then I return to the "Categories" page
Feature: I HIT THE CANCEL BUTTON, I DID NOT FILL ANYTHING IN THE FORM
	@CucumberScenario
	Scenario: I hit the cancel button
		Given I am on the "New Subcategory" popup
		When I haven't filled any of the required fields
		And click on the "Cancel" button
		Then I see the "Categories" page
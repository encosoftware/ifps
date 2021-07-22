Feature: I CREATE A NEW SUBCATEGORY SUCCESSFULLY (MORE BUTTON)
    @CucumberScenario
	Scenario: I open the dropdown 
		Given I am on the "Categories" page
		And the cursor is on the "Painted Pine" subcategory's row
		When I click on the "More" button		
		Then a dropdown appears.
    @CucumberScenario
	Scenario: I open the "New Subcategory" popup
		Given I am on the "Categories" page
		And the dropdown is open
		When I click on the "Add new Subcategory" button
		Then a popup appears titled "New Subcategory"
		And the "parent" is equal to "Painted Pine"
	@CucumberScenario
	Scenario: I save a new item
		Given I am on the "New Subcategory" popup
		When "Eng" is filled with "Red"
		And "Hu" is filled with "Piros"
		And the "parent" is equal to "Painted Pine"
		And I click on the "Save" button
		Then I should see the "Categories" page
		And a snackbar "New Subcategory Created" appears


	
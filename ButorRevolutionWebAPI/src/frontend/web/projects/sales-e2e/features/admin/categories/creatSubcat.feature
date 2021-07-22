Feature: I CREATE A NEW SUBCATEGORY SUCCESSFULLY (MORE BUTTON)
    @CucumberScenario
	Scenario: I open the dropdown 
		Given I am on the "Categories" page
		And I click on "Category" button
		When I click on the "Hamburger" button		
		Then the dropdown is open
    @CucumberScenario
	Scenario: I open the "New Subcategory" popup
		Given I am on the "Categories" page
		And I click on "Category" button
		When I click on the "Add new item" button
		Then I am on the "NEW CATEGORY" popup
		And the "parent" is equal to "Fruit"
	@CucumberScenario
	Scenario: I save a new item
		Given I am on the "NEW CATEGORY" popup
		When "Eng" is filled with "Red"
		And "Hu" is filled with "Piros"
		And the "parent" is equal to "Fruit"
		And I click on the "Save" button
		Then a snackbar "New Subcategory Created" appears
		And I should see the "Categories" page
Feature: I EDIT A SUBCATEGORY
	@CucumberScenario		
	Scenario: I click on the edit button 
		Given I am on the "Categories" page
		And I click on "Category" button
		When I click on "Hamburger" button
		And I click on "Edit" button
		Then I see the "NEW CATEGORY" popup

    @CucumberScenario
	Scenario: I edit the subcategory
		Given I am on the "NEW CATEGORY" popup
		When I modify "Eng" from "category 1" to "Painted Pine"
		And I click on the "Save" button 
		Then the modifications are sent to the DB
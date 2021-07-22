Feature:I DELETE A SUBCATEGORY
    @CucumberScenario
	Scenario: I open the dropdown 
		Given I am on the "Categories" page
		And the cursor is on the "Untreated Pine" subcategory's row
		When I click on the "More" button		
		Then a dropdown appears.

    @CucumberScenario
	Scenario: I click the delete button
		Given I am on the "Categories" page
		And the dropdown is open 		
 		And I click on the "Delete" button
		Then a popup appears titled "Are you sure?"
    @CucumberScenario
	Scenario: I delete the subcategory
		Given I see the "Are you sure?" popup
		When I click delete
		Then I return to te "Categories" page
		And a "Untreated Pine deleted" snackbar appears
		And the modifications are sent to the DB
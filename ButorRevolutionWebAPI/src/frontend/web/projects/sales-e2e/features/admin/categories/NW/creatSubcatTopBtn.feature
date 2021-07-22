

Feature: I HIT THE CANCEL BUTTON, FILLED SOME FIELDS IN THE FORM, I RETURN TO THE "NEW SUBCATEGORY" POPUP
    @CucumberScenario
	Scenario: I hit the cancel button
		Given I am on the "New Subcategory" popup
		When I have filled some of the required fields and click on the "Cancel" button
		Then I see a popup "Are you sure?"
    @CucumberScenario
	Scenario: I return to the "New Subcategory" popup
			Given I see a popup "Are you sure?"
			When I click the "Cancel" button
			Then I return to the "New Subcategory" popup with my filled fields



# Feature: I EDIT A SUBCATEGORY
# 	@CucumberScenario		
# 	Scenario: I click on the edit button 
# 		Given I am on the "Categories" page
# 		And the cursor is on the "Untreated Pine" subcategory's row
# 		When I click on the "Edit" button
# 		Then I see the "Untreated Pine Edit" popup

#     @CucumberScenario
# 	Scenario: I edit the subcategory
# 		Given I am on the "Untreated Pine Edit" popup
# 		When I modify "Eng" from "Untreated Pine" to "Painted Pine"
# 		And I click the "Save" button 
# 		Then the modifications are sent to the DB



# Feature:I DELETE A SUBCATEGORY
#     @CucumberScenario
# 	Scenario: I open the dropdown 
# 		Given I am on the "Categories" page
# 		And the cursor is on the "Untreated Pine" subcategory's row
# 		When I click on the "More" button		
# 		Then a dropdown appears.

#     @CucumberScenario
# 	Scenario: I click the delete button
# 		Given I am on the "Categories" page
# 		And the dropdown is open 		
#  		And I click on the "Delete" button
# 		Then a popup appears titled "Are you sure?"
#     @CucumberScenario
# 	Scenario: I delete the subcategory
# 		Given I see the "Are you sure?" popup
# 		When I click delete
# 		Then I return to te "Categories" page
# 		And a "Untreated Pine deleted" snackbar appears
# 		And the modifications are sent to the DB
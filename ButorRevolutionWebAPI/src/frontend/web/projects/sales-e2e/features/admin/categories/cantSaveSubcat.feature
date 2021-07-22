Feature: I CAN'T SAVE THE NEW SUBCATEGORY
    @CucumberScenario
	Scenario Outline: I can't save the new subcategory
		Given I am on the "Categories" page
        And I click on "Category" button
        And I click on the "Add new item" button
        And I am on the "NEW CATEGORY" popup
		When "Eng" is filled with "<eng>"
		And "Hu" is filled with "<hu>"
		And the "parent" is equal to "<parent>"
		When I click on the "Save" button
		Then a snackbar "New Subcategory Created" appears
        #ScenarioOutlineLine
		Examples:

		| eng	  		|hu	           	| parent    	|error message	 					|

		| 			| Kezeletlen fenyő	| Fruit	|"this is a required field"				|

		| Untreated Pine	| 			| Fruit	|"this is a required field"				|

		| Untreated Pine	| Kezeletlen fenyő	| Fruit		|"please select a parent category"			|




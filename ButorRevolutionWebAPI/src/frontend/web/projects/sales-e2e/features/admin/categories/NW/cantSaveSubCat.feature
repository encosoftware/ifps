Feature: I CAN'T SAVE THE NEW SUBCATEGORY

    @CucumberScenario
	Scenario: I can't save the new subcategory
		Given I am on the the "New Subcategory" popup
		And "Eng" is filled with <eng>
		And "Hu" is filled with <hu>
		And "Parent" is <parent>
		When I click on the "Save" button
		Then <error message> appears above the incorrectly filled text field
		And the modifications are not sent to the DB

		Examples:

		| eng	  		|hu	           	| parent    	|error message|

		| null			| Kezeletlen fenyő	| Material	|"this is a required field"|

		| Untreated Pine	| null			| Material	|"this is a required field"|

		| Untreated Pine	| Kezeletlen fenyő	| null		|"please select a parent category"|
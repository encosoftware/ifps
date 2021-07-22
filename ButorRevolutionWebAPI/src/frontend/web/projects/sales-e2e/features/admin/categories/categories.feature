Feature: Navigation Steps As a user I should able to navigate on web page
    @CucumberScenario
    Scenario: Get Subcategories
        Given I am on the "Categories" page
        When I click on "Category" button
        Then I get SubCategories
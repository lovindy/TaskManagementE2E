Feature: User Login
  As a user
  I want to log in to my account
  So that I can access my personalized dashboard

  Scenario: Successful login with valid credentials
    Given I am on the login page
    When I enter "testuser@example.com" as username and "password123" as password
    And I click the login button
    Then I should see the user dashboard

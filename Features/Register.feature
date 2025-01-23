Feature: User Registration
  As a new user
  I want to register an account
  So that I can access the dashboard

  Scenario: Successful registration with valid details
    Given I am on the registration page
    When I enter username "newuser001"
    And I enter password "StrongPass123!"
    And I enter confirmation password "StrongPass123!"
    And I submit the registration form
    Then I should see a successful registration message
    And I should be redirected to the dashboard
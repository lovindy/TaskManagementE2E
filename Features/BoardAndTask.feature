Feature: Task Management
  As a logged-in user
  I want to create tasks in different lists
  So that I can manage my work effectively

  Scenario: Create a task in the To Do list
    Given I am logged in
    When I navigate to the "My Board" board
    And I click to add a new task to the "To Do" list
    And I enter task title "Implement user authentication"
    And I enter task description "Create login and registration functionality"
    And I submit the task creation form
    Then I should see the new task "Implement user authentication" in the "To Do" list
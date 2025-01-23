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

  Scenario: Create a task in the Doing list
    Given I am logged in
    When I navigate to the "My Board" board
    And I click to add a new task to the "Doing" list
    And I enter task title "Design dashboard layout"
    And I enter task description "Create responsive layout for the main dashboard"
    And I submit the task creation form
    Then I should see the new task "Design dashboard layout" in the "Doing" list

  Scenario: Create a task in the Done list
    Given I am logged in
    When I navigate to the "My Board" board
    And I click to add a new task to the "Done" list
    And I enter task title "Set up project repository"
    And I enter task description "Create GitHub repository and initial project structure"
    And I submit the task creation form
    Then I should see the new task "Set up project repository" in the "Done" list
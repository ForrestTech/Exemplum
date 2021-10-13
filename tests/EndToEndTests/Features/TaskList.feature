Feature: TaskList
	User can manage task lists

Scenario: User can access task lists
	Given a logged in user
	When the user navigates to the task lists
	Then the user can access the task lists page
Feature: Login
	User can login to the website

Scenario: User can login with valid credentails
	Given a logged out user
	When the user attempts to log in with valid credentials
	Then the user is logged in successfully
# UniSuper
Some simple test scenarios implemented on http://todomvc.com
  1. I want to add a To-do item
  2. I want to edit the content of an existing To-do item
  3. I can complete a To-do by clicking inside the circle UI to the left of the To-do
  4. I can re-activate a completed To-do by clicking inside the circle UI
  5.  	I can add a second To-do
  6. I can complete all active To-dos by clicking the down arrow at the top-left of the UI
  7. I can filter the visible To-dos by Completed state	
  8. I can clear a single To-do item from the list completely by clicking the Close icon.
  9. I can clear all completed To-do items from the list completely

Technologies Used : 
Selenium Web Driver with Visual Studio 2017 Community Edition and C#,as programming language

Limitations :
Please note : All the above test scenarios have been clubbed into a single unit test file UniSuperTests.cs 
and into a single test case TestScenarios due to time constraint.
Also I have not completely used Data Driven testing by storing my test data in the excel or the database due to the same  
reason.
In ideal case I would have used SpecFlow for TDD instead of clubbing all test cases as one and have built my scenarios  
accordingly using Hybrid framework.
The tests will run on Browser not maximized mode as I have not implemented driver.Manage().Window.Maximize() in my code as it was failing and I am investigating the issue.

How to run the above 9 Test Scenarios : 

1. Open the project on Visual Studio 2017(community or if you have the licenced version ,you can use that)
2. Build the Project by clicking on the menu Build -> Build Solution
3. Open the Test Explorer if its not open already by Clicking on the menu Test -> Windows -> Test Explorer
4. There you can see the test item TestScenarios
5. Click on the Run All link or right click on the TestScenarios test item and then click on the Run Selected Tests.

How to Run the tests on different Browsers : 

By default these test run on Chrome due to the preset configuration settings. To run on different browsers open the 
\\UniSuperAssignment\\UniSuper\\AutomationFramework\\Config\\GlobalConfig.xml
in there change the <BrowserType>Chrome</BrowserType> to <BrowserType>Fireox</BrowserType> to run on firefox
The accepted values here are 
1. Chrome
2. FireFox
3. InternetExplorer

All the test config settings are stored in this Global.xml file

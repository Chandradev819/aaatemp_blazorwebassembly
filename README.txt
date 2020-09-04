This app requires SQL Server and Visual Studio

I'm having various problems when running this blazor webassembly application.
When you login, an error comes up saying the Policy is undefined (you can see the error in the browser console window).
Also the navbar is not loading with items from the database.
------------------------------------------
Setting up the project:
-Open the project in Visual Studio.
-Do a "update-database" to move the migrations into your db.
-Run the project and go to the login screen.
-Click the red button once....then reload the login screen without the handler part in the url
-login as "user1@aaa.aaa"/"abCD$$123"
-once you login, you are taken to the index.razor page.....it shows an error in the screen. This error needs to be fixed.
-Also, the navbar menuitems are not showing....these need to show.

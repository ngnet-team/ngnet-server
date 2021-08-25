# ngnet-server
ASP.NET Web API

Steps before launching:

		- Clone "appSettings.json" file to an "appSettings.Development.json" one and add the followng structior:
		
		"Admin": {
			"Email": adminEmail,
			"Username": adminUsername,
			"Password": adminPassword,
			"FirstName": adminFirstName (optional),
			"LastName": adminLastName (optional)
		}
		
		- Create an initial migration using the following code: "dotnet ef migrations add Initial" in Data directory.
	


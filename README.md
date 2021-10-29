# ngnet-server
ASP.NET Web API

Launch guide:

  1. Once the repository is cloned duplicate "appSettings.json" file to an "appSettings.Development.json" one and add the followng structior:
	"Admin": {
		"Email": adminEmail,
		"Username": adminUsername,
		"Password": adminPassword,
		"FirstName": adminFirstName (optional),
		"LastName": adminLastName (optional)
	}
  2. Create an initial migration using the following code: "dotnet ef migrations add Initial" in Data directory.
  3. Start Ngnet.Web project.

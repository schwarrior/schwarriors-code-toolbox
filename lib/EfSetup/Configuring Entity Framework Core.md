Configuring EF Core within an ASP.NET Core project
==================================================


# Instructions

Add or update the database conn string to appsettings.json

```
"ConnectionStrings": {
	"DefaultConnection": "Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True"
},
```

Create DBContext class

```
public class MyDbContext : DbContext
    {
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
	    var connectionString = ConfigurationProvider.GetDatabaseConnectionString("DefaultConnection");
	    optionsBuilder.UseSqlServer(connectionString);
	}

	public DbSet<Template> Templates { get; set; }
    }
```

If the project won't compile you may need some additional NuGet packages.
- "microsoft.entityframeworkcore.tools"
- "

You might need to add nuget packages to your project to bring in ef core and ef core.sql packages. 

Add this to .csproj

	  <ItemGroup>
	      <Content Update="appsettings.json">
	        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
	      </Content>
	   </ItemGroup>

Unsure if this required, but you might need to create the database first.

Install or update the local Entity FRamework command line tools, which are extensions of dotnet.exe

enable ef cli tools:
```
dotnet tool install --global dotnet-ef
```
or
```
dotnet tool install --global dotnet-ef
```

Verify install:
```
dotnet ef
```

Add or Update the current VS project's NuGet packages. 
```
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```


[More info on entity framework through the  dotnet cli] (https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools)

From Powershell CLI window in context of .csproj location. 

```
# verify dbcontext is visible to EF
dotnet ef dbcontext info

# add a first migration,
dotnet ef migrations add initial

# after specifying a little beit of schema, send it to the development SQL Server.
# If the DB doesn't exist, it will be created
dotnet ef database update
```



dotnet ef cli ref
https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

general ref
https://docs.microsoft.com/en-us/ef/core/

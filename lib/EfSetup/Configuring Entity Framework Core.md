Configuring EF Core within an ASP.NET Core project
==================================================


# Instructions

Add or update the database conn string to appsettings.json

```
"ConnectionStrings": {
	"DefaultConnection": "Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True"
},
```

Create a DBContext class

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

If the project won't compile you may need some additional NuGet packages, more detail below.

You'll also need to to install the ef extensions for dotnet.exe

```
# Verify this dislays ef command help, not an error.
dotnet ef
```

You can add the EF CLI extensions with this command
```
nugent add package microsoft.entityframeworkcore.tools"
```

then enable ef cli tools:
```
dotnet tool install --global dotnet-ef
```
or
```
dotnet tool update --global dotnet-ef
```

Add or Update the current VS project's NuGet packages. 
```
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

[More info on entity framework through the  dotnet cli] (https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools)



Verify dbcontext is visible to EF
```
dotnet ef dbcontext info
```

Add a first migration
```
dotnet ef migrations add initial
```

After specifying a little bit of schema, send it to the development SQL Server.
If the DB doesn't exist, it will be created.
```
dotnet ef database update
```

# References

dotnet ef cli ref
https://learn.microsoft.com/en-us/ef/core/cli/dotnet

general ref
https://learn.microsoft.com/en-us/ef/core/

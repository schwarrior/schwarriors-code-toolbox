Connection String Examples

# For .NET Entity Framework

The MultipleActiveResultSets=True is important; otherwise highly prone to locking itself.

```
Data Source=.;Initial Catalog=MyDatabase;Persist Security Info=True;Integrated Security=True;MultipleActiveResultSets=True
```

# For LinqPad

Linqpad programs are sometimes prone to locking records. To fix, go to the connection advanced properties and add Additional connection string parameters:

```
MultipleActiveResultSets=True
```

# For Node.js to SQL Server

To do: implement Node.js example. 

Include the self signed certificate setting.

# For Node.js to SQL Server named instance

To do: implement Node.js example how to access.

Important Note: The SQL Browser service must be running on the Node.js host machine to be able to access named instances.
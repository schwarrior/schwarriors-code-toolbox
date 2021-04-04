context.People.AddOrUpdate(x => new { x.FirstName, x.LastName },
    new Person
    {
        FirstName = "Jane",
        LastName = "Smith",
        Age = "31",
        City = "Chicago"
    });
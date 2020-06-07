# Dapper.UnitTest

**Dapper.UnitTest** is a Unit Test tool for Dapper and ADO.Net.

### Motivation

It is difficult to implement UnitTest in data access layer, because everything we are writing is facing to Database. However, we don't really want to access the database in UnitTest. In addition, some companies set a code coverage threshold to compile/deploy. The purpose of this project is making data access layer UnitTest-able.

### What can **Dapper.UnitTest** do?

**Dapper.UnitTest** can help you:
- Verify expected database command has been executed with expected parameters.
- Verify expected value has been returned.
- Raise code coverage.

**Dapper.UnitTest** can't help you:
- ~~Verify the logic in your command text~~

### How to use?

In the UnitTest, write like below:
```csharp
// Arrange
var repo = SomeRepository(mockDbConnection.Object);
mockDbConnection.InjectReturnValues(/*Return values for each DbCommand*/);

// Act
var retVal = repo.AccessDatabase();

// Assert
Assert.AreEqual(/*retVal is the same as expected*/);
Assert.AreEqual(/*Number of DbCommands been executed*/, mockDbConnection.ExecutedDbCommands.Count);
// can also verify the executed command text & parameters
```

For details, please see [samples](https://github.com/vincent-scw/Dapper.UnitTest/blob/master/Dapper.UnitTest.Sample/UnitTest.cs)

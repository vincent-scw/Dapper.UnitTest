using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dapper.UnitTest.Sample
{
    [TestClass]
    public class UnitTest
    {
        private Mock<IConnectionFactory> _connectionFactory;
        private MockDbConnection _mockDbConnection;

        [TestInitialize]
        public void Init()
        {
            _connectionFactory = new Mock<IConnectionFactory>();
            _mockDbConnection = new MockDbConnection();
            _connectionFactory.Setup(x => x.GetConnection()).Returns(_mockDbConnection);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockDbConnection.Clean();
        }

        [TestMethod]
        public void Test_GetOne()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = new EmployeeRepository(_connectionFactory.Object);
            var retVal = new List<dynamic> { new { Id = id, Name = "Somebody", Age = 30, Joined = DateTime.Today } };
            _mockDbConnection.InjectReturnValues(retVal);

            // Act
            var employee = repo.GetOne(id);

            // Assert
            AssertObjectEqual(retVal[0], employee);
        }

        [TestMethod]
        public void Test_GetOneWithDapper()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = new EmployeeRepository(_connectionFactory.Object);
            var retVal = new List<dynamic> { new { Id = id, Name = "Somebody", Age = 30, Joined = DateTime.Today } };
            _mockDbConnection.InjectReturnValues(retVal);

            // Act
            var employee = repo.GetOneWithDapper(id);

            // Assert
            AssertObjectEqual(retVal[0], employee);
        }

        [TestMethod]
        public async Task Test_GetOneWithDapperAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = new EmployeeRepository(_connectionFactory.Object);
            var retVal = new List<dynamic> { new { Id = id, Name = "Somebody", Age = 30, Joined = DateTime.Today } };
            _mockDbConnection.InjectReturnValues(retVal);

            // Act
            var employee = await repo.GetOneWithDapperAsync(id);

            // Assert
            AssertObjectEqual(retVal[0], employee);
        }

        [TestMethod]
        public async Task Test_GetManyWithDapperAsync()
        {
            // Arrange
            var repo = new EmployeeRepository(_connectionFactory.Object);
            var retVal = new List<EmployeeModel>
            {
                new EmployeeModel { Id = Guid.NewGuid(), Name = "E1", Age = 31, Joined = DateTime.Today },
                new EmployeeModel { Id = Guid.NewGuid(), Name = "E2", Age = 32, Joined = DateTime.Today }
            };
            _mockDbConnection.InjectReturnValues(retVal);

            // Act
            var employees = await repo.GetManyWithDapperAsync();

            // Assert
            Assert.AreEqual(2, employees.Count);
            AssertObjectEqual(retVal[0], employees[0]);
            AssertObjectEqual(retVal[1], employees[1]);
        }

        [TestMethod]
        public async Task Test_GetScalarWithDapperAsync()
        {
            // Arrange
            var expectedCount = 10;
            var repo = new EmployeeRepository(_connectionFactory.Object);
            _mockDbConnection.InjectReturnValues(expectedCount);

            // Act
            var count = await repo.GetScalarWithDapperAsync();

            // Assert
            Assert.AreEqual(expectedCount, count);
        }

        [TestMethod]
        public async Task Test_ComplicateCaseAsync()
        {
            // Arrange
            var retEmployee = new { Id = Guid.NewGuid(), Name = "Somebody", Age = 30, Joined = DateTime.Today };
            var repo = new EmployeeRepository(_connectionFactory.Object);
            _mockDbConnection.InjectReturnValues(
                1,
                1,
                new List<dynamic> { retEmployee });

            // Act
            var employee = await repo.ComplicateCaseAsync(retEmployee.Id);

            // Assert
            AssertObjectEqual(retEmployee, employee); // Return value
            Assert.AreEqual(3, _mockDbConnection.ExecutedDbCommands.Count); // executed db command
            Assert.AreEqual("SELECT 1 WHERE Id=@id", _mockDbConnection.ExecutedDbCommands[0].CommandText);
            Assert.AreEqual(1, _mockDbConnection.ExecutedDbCommands[0].Parameters.Count); // db parameters
            Assert.AreEqual(retEmployee.Id, _mockDbConnection.ExecutedDbCommands[0].Parameters[0].Value);
        }

        private void AssertObjectEqual(dynamic retVal, EmployeeModel employee)
        {
            Assert.AreEqual(retVal.Id, employee.Id);
            Assert.AreEqual(retVal.Name, employee.Name);
            Assert.AreEqual(retVal.Age, employee.Age);
            Assert.AreEqual(retVal.Joined, employee.Joined);
        }
    }
}

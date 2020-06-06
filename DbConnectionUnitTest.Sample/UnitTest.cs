using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DbConnectionUnitTest.Sample
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
            var repo = new EmployeeRepository(_connectionFactory.Object);
            var retVal = new { Id = 1, Name = "Somebody" };
            _mockDbConnection.InjectReturnValues(new List<dynamic> { retVal });

            // Act
            var employee = repo.GetOne(1);

            // Assert
            Assert.AreEqual(retVal.Id, employee.Id);
            Assert.AreEqual(retVal.Name, employee.Name);
        }

        [TestMethod]
        public void Test_GetOneWithDapper()
        {
            // Arrange
            var repo = new EmployeeRepository(_connectionFactory.Object);
            var retVal = new { Id = 1, Name = "Somebody" };
            _mockDbConnection.InjectReturnValues(new List<dynamic> { retVal });

            // Act
            var employee = repo.GetOneWithDapper(1);

            // Assert
            Assert.AreEqual(retVal.Id, employee.Id);
            Assert.AreEqual(retVal.Name, employee.Name);
        }

        [TestMethod]
        public async Task Test_GetOneWithDapperAsync()
        {
            // Arrange
            var repo = new EmployeeRepository(_connectionFactory.Object);
            var retVal = new { Id = 1, Name = "Somebody" };
            _mockDbConnection.InjectReturnValues(new List<dynamic> { retVal });

            // Act
            var employee = await repo.GetOneWithDapperAsync(1);

            // Assert
            Assert.AreEqual(retVal.Id, employee.Id);
            Assert.AreEqual(retVal.Name, employee.Name);
        }
    }
}

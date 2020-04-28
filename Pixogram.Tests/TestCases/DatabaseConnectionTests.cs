using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Pixogram.Entities;
using SpringMvc.Datalayer;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Pixogram.Tests.TestCases
{
  public  class DatabaseConnectionTests
    {
        private Mock<IOptions<Mongosettings>> _mockOptions;

        private Mock<IMongoDatabase> _mockDB;

        private Mock<IMongoClient> _mockClient;

        public DatabaseConnectionTests()
        {
            _mockOptions = new Mock<IOptions<Mongosettings>>();
            _mockDB = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();
        }

        [Fact]
        public void MongoBookDBContext_Constructor_Success()
        {
            var settings = new Mongosettings()
            {
                Connection = "mongodb://localhost:27017",
                DatabaseName = "guestbook"
            };
            _mockOptions.Setup(s => s.Value).Returns(settings);
            _mockClient.Setup(c => c
            .GetDatabase(_mockOptions.Object.Value.DatabaseName, null))
                .Returns(_mockDB.Object);

            //Act 
            var context = new MongoUserDBContext(_mockOptions.Object);

            //Assert 
            Assert.NotNull(context);
        }


        [Fact]
        public void MongoBookDBContext_GetCollection_ValidName_Success()
        {
            //Arrange
            var settings = new Mongosettings()
            {
                Connection = "mongodb://localhost:27017",
                DatabaseName = "guestbook"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);

            _mockClient.Setup(c => c.GetDatabase(_mockOptions.Object.Value.DatabaseName, null)).Returns(_mockDB.Object);

            //Act 
            var context = new MongoUserDBContext(_mockOptions.Object);
            var myCollection = context.GetCollection<User>("User");

            //Assert 
            Assert.NotNull(myCollection);
        }
    }
}

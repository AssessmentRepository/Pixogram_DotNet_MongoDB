using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Pixogram.BusinessLayer.Repository;
using Pixogram.Entities;
using Pixogram.Tests.Exceptions;
using SpringMvc.Datalayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pixogram.Tests.TestCases
{
    public class ExceptionalTest
    {
        private Mock<IMongoCollection<User>> _mockCollection;
        private Mock<IMongoUserDBContext> _mockContext;
        private User _user;
        private readonly IList<User> _list;
        // MongoSettings declaration
        private Mock<IOptions<Mongosettings>> _mockOptions;


        public ExceptionalTest()
        {

        _user = new User
            {
               FirstName="Baby",
               LastName="Bab",
                UserName = "bb",
                Email = "bbb@gmail.com",
                Password = "123456789",
               ConfirmPassword = "123456789",
                ProfilePicture = "Pho"
            };
            _mockCollection = new Mock<IMongoCollection<User>>();
            _mockCollection.Object.InsertOne(_user);
            _mockContext = new Mock<IMongoUserDBContext>();
            //MongoSettings initialization
            _mockOptions = new Mock<IOptions<Mongosettings>>();
            _list = new List<User>();
            _list.Add(_user);
        }
        Mongosettings settings = new Mongosettings()
        {
            Connection = "mongodb://localhost:27017",
            DatabaseName = "guestbook"
        };

        [Fact]
        public async void CreateNewUser_Null_User_Failure()
        {
            // Arrange
            _user = null;

            //Act 
            var bookRepo = new UserRepository(_mockContext.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => bookRepo.Create(_user));
        }

        [Fact]
        public async Task ExceptionTestFor_ValidRegistration()
        {
            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            //Assert
          await Assert.ThrowsAsync<UserExistException>(async () => await userRepo.Create(_user));
           

        }

        [Fact]
        public async Task ExceptionTestFor_UserNotFound()
        {
            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            var ex = await Assert.ThrowsAsync<UserNotFoundException>(() => userRepo.SignIn(_user.UserName, _user.Password));

            Assert.Equal("User Not Found ", ex.Messages);

        }


        [Fact]
        public async Task ExceptionTestFor_ValidUserName_InvalidPassword()
        {
            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context); 

            //Assert
            var ex = await Assert.ThrowsAsync<InvalidCredentialsExceptions>(() => userRepo.SignIn(_user.UserName, _user.Password));

            Assert.Equal("Please enter valid usename & password", ex.Messages);

        }

        [Fact]
        public async Task ExceptionTestFor_InvalidUserName_validPassword()
        {
            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Assert
            var ex = await Assert.ThrowsAsync<InvalidCredentialsExceptions>(() => userRepo.SignIn(_user.UserName, _user.Password));
            Assert.Equal("Please enter valid usename & password", ex.Messages);
        }


    }
}

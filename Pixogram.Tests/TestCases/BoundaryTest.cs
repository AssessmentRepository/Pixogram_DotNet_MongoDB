using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Pixogram.BusinessLayer.Repository;
using Pixogram.Entities;
using SpringMvc.Datalayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pixogram.Tests.TestCases
{
  public  class BoundaryTest
    {
        private Mock<IMongoCollection<User>> _mockCollection;
        private Mock<IMongoUserDBContext> _mockContext;
        private User _user;
        private readonly IList<User> _list;
        // MongoSettings declaration
        private Mock<IOptions<Mongosettings>> _mockOptions;
        

        public BoundaryTest()
        {

            _user = new User
            {
                FirstName = "Baby",
                LastName = "Bab",
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
        public async Task BoundaryTestfor_ValidUserEmailAsync()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_user, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            await userRepo.Create(_user);
            var result = await userRepo.Get(_user.Id);

            ////Action
          //  var getregisteredUser = await _userServices.GetUser(user.UserName);
            bool CheckEmail = Regex.IsMatch(result.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            bool isEmail = Regex.IsMatch(_user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            //Assert
            Assert.True(isEmail);
            Assert.True(CheckEmail);
        }


        [Fact]
        public async Task BoundaryTestFor_validUserNameLengthAsync()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_user, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            await userRepo.Create(_user);
            var result = await userRepo.Get(_user.Id);

            var MinLength = 3;
            var MaxLength = 50;

            //Action
            var actualLength = _user.UserName.Length;
        
            //Assert
            Assert.InRange(result.UserName.Length, MinLength, MaxLength);
            Assert.InRange(actualLength, MinLength, MaxLength);
        }

        [Fact]
        public async Task BoundaryTestfor_ValidUserNameAsync()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_user, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            await userRepo.Create(_user);
            var result = await userRepo.Get(_user.Id);
         
            bool getisUserName = Regex.IsMatch(result.UserName, @"^[a-zA-Z0-9]{4,10}$", RegexOptions.IgnoreCase);
            bool isUserName = Regex.IsMatch(_user.UserName, @"^[a-zA-Z0-9]{4,10}$", RegexOptions.IgnoreCase);
            //Assert
            Assert.True(isUserName);
            Assert.True(getisUserName);
        }

        [Fact]
        public async Task BoundaryTestfor_ValidId()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_user, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            await userRepo.Create(_user);
            var result = await userRepo.Get(_user.Id);
           
                Assert.InRange(_user.Id.Length, 20, 30);

        }

        [Fact]
        public async Task BoundaryTestFor_PasswordLengthAsync()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_user, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            await userRepo.Create(_user);
            var result = await userRepo.Get(_user.Id);

            var MinLength = 8;
            var MaxLength = 25;

            //Action
            var actualLength = _user.Password.Length;
         
            //Assert
            Assert.InRange(result.Password.Length, MinLength, MaxLength);
            Assert.InRange(actualLength, MinLength, MaxLength);
        }


    }
    }

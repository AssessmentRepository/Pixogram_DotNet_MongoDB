using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Pixogram.BusinessLayer.Repository;
using Pixogram.Entities;
using SpringMvc.Datalayer;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pixogram.Tests.TestCases
{
    public class FuctionalTests
    {
        private Mock<IMongoCollection<User>> _mockCollection;
        private Mock<IMongoCollection<Content>> _ContentMockCollection;
        private Mock<IMongoCollection<Feedback>> _feedbackmMockCollection;
        private Mock<IMongoUserDBContext> _mockContext;
        private Mock<IMongoCollection<Follow>> _followMockCollection;
        private Mock<IMongoCollection<ILog>> _iLogmockCollection;
        private User _user;
        private User _senderUser;
        private ILog _iLog;
        private readonly IList<User> _list;
        private Content _content;
        private Feedback _feedback;
        private Follow _follow;
        private List<ILog> _loglist;
        private List<Content> contentslist;
        private List<Follow> followedlist;
        // MongoSettings declaration
        private Mock<IOptions<Mongosettings>> _mockOptions;

        public FuctionalTests()
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
            _senderUser = new User
            {
                FirstName = "John",
                LastName = "Bab",
                UserName = "Jb",
                Email = "jjb@gmail.com",
                Password = "123456789",
                ConfirmPassword = "123456789",
                ProfilePicture = "Pho"
            };

             _feedback = new Feedback
            {
            UserId=_user.Id,
            SenderUserId= _senderUser.Id,
            Comment="comment",
            Like=true
            };

            _content = new Content()
            {
            Image="url//image",
            UserId = _user.Id
            };

            _follow = new Follow()
            {
            UserId=_user.Id,
            FollowUserId=_senderUser.Id
            };

            _iLog = new ILog
            {
                content = _content,
                feedback = _feedback,
                follow = _follow
            };
              
       
            _mockCollection = new Mock<IMongoCollection<User>>();
            _mockCollection.Object.InsertOne(_user);
            _ContentMockCollection = new Mock<IMongoCollection<Content>>();
            _ContentMockCollection.Object.InsertOne(_content);
            _feedbackmMockCollection = new Mock<IMongoCollection<Feedback>>();
            _feedbackmMockCollection.Object.InsertOne(_feedback);
            _followMockCollection = new Mock<IMongoCollection<Follow>>();
            _followMockCollection.Object.InsertOne(_follow);
            _iLogmockCollection = new Mock<IMongoCollection<ILog>>();
            _iLogmockCollection.Object.InsertOne(_iLog);

            _mockContext = new Mock<IMongoUserDBContext>();
            //MongoSettings initialization
            _mockOptions = new Mock<IOptions<Mongosettings>>();
            _list = new List<User>();
            _list.Add(_user);
            followedlist = new List<Follow>();
            followedlist.Add(_follow);
            _loglist = new List<ILog>();
            _loglist.Add(_iLog);
            contentslist = new List<Content>();
            contentslist.Add(_content);
        }
        Mongosettings settings = new Mongosettings()
        {
            Connection = "mongodb://localhost:27017",
            DatabaseName = "kavya"
        };



        [Fact]
        public async Task GetAllUsers()
        {
            //Arrange
            //Mock MoveNextAsync
            Mock<IAsyncCursor<User>> _userCursor = new Mock<IAsyncCursor<User>>();
            _userCursor.Setup(_ => _.Current).Returns(_list);
            _userCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<User>>(),
            It.IsAny<FindOptions<User, User>>(),
             It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

            //Jayanth Creating one more instance of DB

            _mockOptions.Setup(s => s.Value).Returns(settings);

            // Creating one more instance of DB
            // Passing _mockOptions instaed of _mockContext
            var context = new MongoUserDBContext(_mockOptions.Object);

            var userRepo = new UserRepository(context);

            //Act
            var result = await userRepo.GetAll();

            //Assert 
            //loop only first item and assert
            foreach (User user in result)
            {
                Assert.NotNull(user);
                Assert.Equal(user.UserName, _user.UserName);
                Assert.Equal(user.Email, _user.Email);
                break;
            }
        }


        [Fact]
        public async void TestFor_CreateNewUser()
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

            //Assert
            Assert.Equal(_user.UserName, result.UserName);
        }

        [Fact]
        public async Task TestFor_UpDateUser()
        {
            //Arrange
            _mockCollection.Setup(s => s.UpdateOneAsync(It.IsAny<FilterDefinition<User>>(), It.IsAny<UpdateDefinition<User>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()));
            //mocking
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            await userRepo.Create(_user);
            userRepo.Update(_user);
            var result = await userRepo.Get(_user.Id);

            //Assert
            Assert.Equal(_user.UserName, result.UserName);

        }

        [Fact]
        public async Task TestFor_DeleteUser()
        {
            //Arrange

            //mocking
            _mockCollection.Setup(op => op.FindOneAndDelete(_user.Id, null, default(CancellationToken)));
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name));

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            await userRepo.Create(_user);
            userRepo.Delete(_user.Id);
            var result = await userRepo.Get(_user.Id);

            //Assert
            Assert.Null(result);

        }
        [Fact]
        public async Task TestFor_GetUserById()
        {
            //Arrange

            //mocking
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<User>>(),
            It.IsAny<FindOptions<User, User>>(),
             It.IsAny<CancellationToken>()));

            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name));

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            await userRepo.Create(_user);
            var result = await userRepo.Get(_user.Id);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestFor_ResetPassword()
        {
            //Arrange
            string Password = "bb123";

            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_user, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name));

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);


            //Act
            await userRepo.ResetPassword(_user.Id, Password);
            var result = await userRepo.Get(_user.Id);

            //Assert
            Assert.Equal(Password, result.Password);
        }

        [Fact]
        public async Task TestFor_GetProfile()
        {
            //Arrange
            //mocking
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<User>>(),
            It.IsAny<FindOptions<User, User>>(),
             It.IsAny<CancellationToken>()));

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            var result = await userRepo.GetProfile(_user.Id);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestFor_UpDateUserProfile()
        {
            //Arrange

            //mocking
            _mockCollection.Setup(s => s.UpdateOneAsync(It.IsAny<FilterDefinition<User>>(), It.IsAny<UpdateDefinition<User>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()));
            _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            userRepo.Update(_user);
            var result = await userRepo.Get(_user.Id);

            //Assert
            Assert.Equal(_user, result);

        }

        [Fact]
        public async Task TestFor_AddContent()
        {
            //Arrange
            //mocking
           _ContentMockCollection.Setup(op => op.InsertOneAsync(_content, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);
            //mocking
            

            //Act
            var updated = userRepo.AddContent(contentslist, _user.Id);
            var result = await userRepo.GetAllContent(_user.Id,contentslist);

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task TestFor_OrganizeImage()
        {
            //Arrange
            //Arrange
            //Mock MoveNextAsync
            Mock<IAsyncCursor<Content>> _userCursor = new Mock<IAsyncCursor<Content>>();
            _userCursor.Setup(_ => _.Current).Returns(contentslist);
            _userCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _ContentMockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Content>>(),
            It.IsAny<FindOptions<Content, Content>>(),
             It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);
      

          //  Act
            var updated = await userRepo.OrganizeImage(_user.Id, contentslist);

          //  Assert
            Assert.NotNull(updated);

        }

        [Fact]
        public async Task TestFor_OrganizeVideo()
        {
            //Arrange

            //Mock MoveNextAsync
            Mock<IAsyncCursor<Content>> _userCursor = new Mock<IAsyncCursor<Content>>();
            _userCursor.Setup(_ => _.Current).Returns(contentslist);
            _userCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _ContentMockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Content>>(),
            It.IsAny<FindOptions<Content, Content>>(),
             It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            var updated = await userRepo.OrganizeVideo(_user.Id, contentslist);

            //Assert
            Assert.NotNull(updated);

        }

        [Fact]
        public async Task TestFor_GetAllContents()
        {
            //Arrange
            //Mock MoveNextAsync
            Mock<IAsyncCursor<Content>> _userCursor = new Mock<IAsyncCursor<Content>>();
            _userCursor.Setup(_ => _.Current).Returns(contentslist);
            _userCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _ContentMockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Content>>(),
            It.IsAny<FindOptions<Content, Content>>(),
             It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            var updated = await userRepo.GetAllContent(_user.Id, contentslist);

            //Assert
            Assert.NotNull(updated);

        }

        [Fact]
        public async Task TestFor_UpdateContent()
        {
            //Arrange

            //mocking
            _ContentMockCollection.Setup(s => s.UpdateOneAsync(It.IsAny<FilterDefinition<Content>>(), It.IsAny<UpdateDefinition<Content>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()));
            _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act

            var updatedContent = await userRepo.UpdateContent(_user.Id, _content);
           // var result = await userRepo.Get(_content.Id);

            //Assert
            Assert.NotNull(updatedContent);
        }
        [Fact]
        public async Task TestFor_AddComment()
        {
            //Arrange

            //mocking
            _feedbackmMockCollection.Setup(op => op.InsertOneAsync(_feedback, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<Feedback>(typeof(Feedback).Name)).Returns(_feedbackmMockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act

            var updatedComment = await userRepo.AddComment(_feedback);

            //Assert
            Assert.Equal(_feedback, updatedComment);
        }
        [Fact]
        public async Task TestFor_FollowUser()
        {
            //Arrange
           var _senderusers = new User
            {
                FirstName = "Baby",
                LastName = "Bab",
                UserName = "bb",
                Email = "bbb@gmail.com",
                Password = "123456789",
                ConfirmPassword = "123456789",
                ProfilePicture = "Pho"
            };

            //Craetion of new Db
           // _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act

            var isFollowed = await userRepo.FollowUser(_user.Id, _senderusers.Id);

            //Assert
            Assert.True(isFollowed);
        }

        [Fact]
        public async Task TestFor_FollowList()
        {
            //Arrange
            //Mock MoveNextAsync
            Mock<IAsyncCursor<Follow>> _userCursor = new Mock<IAsyncCursor<Follow>>();
            _userCursor.Setup(_ => _.Current).Returns(followedlist);
            _userCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _followMockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Follow>>(),
            It.IsAny<FindOptions<Follow, Follow>>(),
             It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<Follow>(typeof(Follow).Name)).Returns(_followMockCollection.Object);

            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

            //Act
            var listOfFollewers = await userRepo.FollowList(_user.Id);

            //Assert
            Assert.NotNull(listOfFollewers);
        }

        [Fact]
        public async Task TestFor_HideMedia()
        {
            //mocking
            //  Craetion of new Db
            var context = new MongoUserDBContext(_mockOptions.Object);
            var userRepo = new UserRepository(context);

          //  Act
            var IsHided = await userRepo.HideMedia(_content.Image,_content.Visibility,_content.Video);

            //Assert
            Assert.True(IsHided);
        }


        [Fact]
        public async Task TestFor_ActivityLog()
        {
            //Arrange

            Mock<IAsyncCursor<ILog>> _userCursor = new Mock<IAsyncCursor<ILog>>();
            _userCursor.Setup(_ => _.Current).Returns(_loglist);
            _userCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _iLogmockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<ILog>>(),
            It.IsAny<FindOptions<ILog, ILog>>(),
             It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<ILog>(typeof(ILog).Name)).Returns(_iLogmockCollection.Object);

            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoUserDBContext(_mockOptions.Object);

            var userRepo = new UserRepository(context);
            //Act

            var ILog = await userRepo.ActivityLog(_user.Id);

            //Assert
            Assert.NotNull(ILog);
        }


    }
}

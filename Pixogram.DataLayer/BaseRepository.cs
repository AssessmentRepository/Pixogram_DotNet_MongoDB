using MongoDB.Bson;
using MongoDB.Driver;
using Pixogram.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpringMvc.Datalayer
{
  public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoUserDBContext _mongoContext;
        protected IMongoCollection<TEntity> _dbCollection;

        protected BaseRepository(IMongoUserDBContext context)
        {
            _mongoContext = context;
            _dbCollection = _mongoContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task Create(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }
            _dbCollection = _mongoContext.GetCollection<TEntity>(typeof(TEntity).Name);
            await _dbCollection.InsertOneAsync(obj);
        }

        public void Delete(string id)
        {
            var objectId = new ObjectId(id);
            _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));
        }


        public async Task<TEntity> Get(string id)
        {
            var objectId = new ObjectId(id);
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            _dbCollection = _mongoContext.GetCollection<TEntity>(typeof(TEntity).Name);
            return await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<TEntity>> GetAll()
        {
           var all = await _dbCollection.FindAsync(Builders<TEntity>.Filter.Empty, null);
            return await all.ToListAsync();
        }


        public Task<IEnumerable<ILog>> ActivityLog(string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProfile(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Follow>> FollowList(string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FollowUser(string UserId, string SenderId)
        {
            throw new NotImplementedException();
        }

        public Task<Feedback> AddComment(Feedback Feedback)
        {
            throw new NotImplementedException();
        }

        public Task AddContent(List<Content> content, string UserID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Content>> GetAllContent(string userId, List<Content> Content)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Content>> GetContentByUserId(string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetProfile(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HideMedia(string picturePath, bool Visibility, string VideoPath)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Content>> OrganizeImage(string UserId, List<Content> Content)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Content>> OrganizeVideo(string UserId, List<Content> Content)
        {
            throw new NotImplementedException();
        }

       

        public Task<User> ResetPassword(string Id, string Password)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> SignIn(string username, string Password)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity obj)
        {
            throw new NotImplementedException();
        }

        public Task<Content> UpdateContent(string UserId, Content Content)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProfile(User User)
        {
            throw new NotImplementedException();
        }
    }
}

using Pixogram.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpringMvc.Datalayer
{
  public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> SignIn(string username, string Password);
        Task Create(TEntity obj);
        void Update(TEntity obj);
        void Delete(string id);
        Task<TEntity> Get(string id);
        Task<IEnumerable<TEntity>> GetAll();

        Task<User> ResetPassword(string Id,string Password);
        Task<User> GetProfile(string userId);
        Task<bool> UpdateProfile(User User);
        Task<bool> DeleteProfile(string userId);

        Task AddContent(List<Content> content, string UserID);

        Task<IEnumerable<Content>> OrganizeImage(string UserId, List<Content> Content);
        Task<IEnumerable<Content>> OrganizeVideo(string UserId, List<Content> Content);

        Task<IEnumerable<Content>> GetAllContent(string userId, List<Content> Content);
        Task<Content> UpdateContent(string UserId, Content Content);
        Task<Feedback> AddComment(Feedback Feedback);

        Task<bool> FollowUser(string UserId, string SenderId);
        Task<IEnumerable<Follow>> FollowList(string UserId);
        Task<bool> HideMedia(string picturePath, bool Visibility, string VideoPath);

        Task<IEnumerable<ILog>> ActivityLog(string UserId);
        Task<IEnumerable<Content>> GetContentByUserId(string UserId);

    }
}

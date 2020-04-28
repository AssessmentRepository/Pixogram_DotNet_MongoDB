using Pixogram.Entities;
using SpringMvc.Datalayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pixogram.BusinessLayer.Repository
{
   public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoUserDBContext context) : base(context)
        {
        }

     
    }
}

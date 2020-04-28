using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pixogram.Entities
{
    public class Follow

    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
        public virtual string UserId { get; set; }

        public virtual string FollowUserId { get; set; }
      
    }
}

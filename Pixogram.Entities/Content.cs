using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pixogram.Entities
{
   public class Content
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
        public virtual string Image { get; set; }
        public virtual string Video { get; set; }
        public virtual string Caption { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Visibility { get; set; }
        public virtual string UserId { get; set; }
    }
}

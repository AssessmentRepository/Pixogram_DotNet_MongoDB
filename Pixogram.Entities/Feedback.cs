using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pixogram.Entities
{
    public class Feedback

    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual string SenderUserId { get; set; }
        public virtual string Comment { get; set; }
        public virtual bool Like { get; set; }
    }
}

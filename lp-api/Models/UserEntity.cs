using AspNetCore.Identity.Mongo.Model;
using System;

namespace lp_api.Models
{
    public class UserEntity : MongoUser
    {
        public UserEntity() : base() { }

        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

    }
}

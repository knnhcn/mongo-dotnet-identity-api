

using AspNetCore.Identity.Mongo.Model;

namespace lp_api.Models
{
    public class UserRoleEntity : MongoRole
    {
        public UserRoleEntity() : base() { }

        public UserRoleEntity(string roleName) : base(roleName) { }

    }
}

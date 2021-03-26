using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Database
{
    public class Follow
    {
        [ForeignKey("FollowerId")]
        public User Follower { get; set; }
        [ForeignKey("FollowingId")]
        public User Following { get; set; }
    }
}

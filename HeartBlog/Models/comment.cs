namespace HeartBlog.Models
{
    using System;
    using System.Collections.Generic;

    public partial class comment
    {
        public int id { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string body { get; set; }
        public Nullable<int> postId { get; set; }
        public Nullable<int> userId { get; set; }
        public virtual post post { get; set; }
        public virtual person user { get; set; }
    }
}

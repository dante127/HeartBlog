namespace HeartBlog.Models
{
    using System;
    using System.Collections.Generic;
    public partial class category
    {
        public category()
        {
            this.posts = new List<post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<post> posts { get; set; }
    }
}

namespace HeartBlog.Models
{
    using System;
    using System.Collections.Generic;

    public partial class consult
    {
        public int id { get; set; }
        public string history { get; set; }
        public string body { get; set; }
        public Nullable<int> userid { get; set; }
        public string answer { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }

        public virtual person user { get; set; }
    }
}

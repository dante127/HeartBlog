namespace HeartBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class person
    {
        public person()
        {
            this.comments = new List<comment>();
            this.consults = new List<consult>();
        }
        public int Id { get; set; }
        [Required]
        [Display(Name ="Full Name")]
        public string Name { get; set; }
        [Required]
        public string Age { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        public string gender { get; set; }
        public virtual List<comment> comments { get; set; }
        public virtual List<consult> consults { get; set; }
    }
}

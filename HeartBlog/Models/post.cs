﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HeartBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    public partial class post
    {
        public int Id { get; set; }

        [AllowHtml]
      
        public string Body { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string title { get; set; }
        [DataType(DataType.MultilineText)]
        public string shortBody { get; set; }
        public string image { get; set; }
        public Nullable<int> cat_id { get; set; }
        public Nullable<int> numofvisitor { get; set; }
        public post()
        {
            this.comments = new List<comment>();
        }
        //navigation propery 
        //database relation 
        // if we define object that mean the relation is one 
        //if we define list of object the relation is many
        public virtual category category { get; set; }

        public virtual List<comment> comments { get; set; }
    }
}

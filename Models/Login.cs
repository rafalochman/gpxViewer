using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gpxViewer.Models
{
    public class Login
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredUsername")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredPassword")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
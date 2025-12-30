using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRUD_APP.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
    }
}

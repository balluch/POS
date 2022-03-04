﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichDomainModelDemo.Application.Model
{
    [Table("Store")]
    public class Store
    {
        public Store(string name)
        {
            Name = name;
        }
#pragma warning disable CS8618
        protected Store() { }
#pragma warning restore CS8618 
        public int Id { get; private set; }
        [MaxLength(255)]
        public string Name { get; set; }
    }
}

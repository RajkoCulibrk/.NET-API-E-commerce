﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Dtos.Category
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class AppConstants
    {
        [Key]
      public int APP_CONSTANT_ID { get; set; }
      public int APP_CONSTANT_TYPE_ID { get; set; }
      public string APP_CONSTANT_TYPE_VALUE { get; set; }
      public int? CREATED_BY { get; set; }
      public DateTime? CREATED_DATE { get; set; }
      public int? UPDATED_BY { get; set; }
      public DateTime? UPDATED_DATE { get; set; }
}
}

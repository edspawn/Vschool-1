﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ILS.Domain
{
    public class PersonalThemeLink : EntityBase 
    {
        public string Status { get; set; }

        [ForeignKey("ThemeLink")] public Guid ThemeLink_Id { get; set; }
        [ForeignKey("CourseRun")] public Guid? CourseRun_Id { get; set; }

        public virtual ThemeLink ThemeLink { get; set; }
        public virtual CourseRun CourseRun { get; set; }
    }
}
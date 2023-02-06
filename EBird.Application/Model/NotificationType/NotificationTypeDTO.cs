﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.NotificationType
{
    public class NotificationTypeDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Notification type code is required")]
        [StringLength(50, ErrorMessage = "Notification type code cannot be longer than 50 characters")]
        public string TypeCode { get; set; }

        [Required(ErrorMessage = "Notifiaction Type Name is required")]
        [StringLength(50, ErrorMessage = "Notification Type name cannot be longer than 50 characters")]
        public string TypeName { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;

namespace Catalyst.Fabric.Authorization.Models
{
    public class GroupRoleApiModel : IIdentifiable<Guid?>
    {
        public Guid? Id { get; set; }

        public string GroupName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public IEnumerable<RoleApiModel> Roles { get; set; }

        /// <summary>
        ///     Group source (e.g., Custom, Windows, Google). For custom groups, use "Custom".
        /// </summary>
        public string GroupSource { get; set; }
    }
}
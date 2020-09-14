﻿using System;

namespace EF.Models.Models
{
    public class PushSubscription
    {
        public Guid UserId { get; set; }

        public string Endpoint { get; set; }

        public string Auth { get; set; }

        public string P256DH { get; set; }

        public virtual User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagger.Models.JSON
{
    public class LoginWithSteam
    {
        public int code { get; set; }
        public string status { get; set; }
        public Data data { get; set; }

        public class Data
        {
            public string SessionTicket { get; set; }
            public string PlayFabId { get; set; }
            public bool NewlyCreated { get; set; }
            public Settingsforuser SettingsForUser { get; set; }
            public DateTime LastLoginTime { get; set; }
            public Entitytoken EntityToken { get; set; }
            public Treatmentassignment TreatmentAssignment { get; set; }
        }

        public class Settingsforuser
        {
            public bool NeedsAttribution { get; set; }
            public bool GatherDeviceInfo { get; set; }
            public bool GatherFocusInfo { get; set; }
        }

        public class Entitytoken
        {
            public string EntityToken { get; set; }
            public DateTime TokenExpiration { get; set; }
            public Entity Entity { get; set; }
        }

        public class Entity
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string TypeString { get; set; }
        }

        public class Treatmentassignment
        {
            public object[] Variants { get; set; }
            public object[] Variables { get; set; }
        }

    }
}

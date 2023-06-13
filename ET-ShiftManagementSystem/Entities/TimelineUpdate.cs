using Microsoft.Graph;
using org.apache.zookeeper.data;
using System;

namespace ET_ShiftManagementSystem.Entities
{
    public class TimelineUpdate
    {
        public Guid Id { get; set; }
        public Guid Alertid { get; set; }
        public string keyvalue { get; set; }
        public string status { get; set; }
        public DateTime datetime { get; set; }
        public Guid createdby { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }

        public string createdbyname { get; set; }

    }
}

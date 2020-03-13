using System;
using System.Collections.Generic;
using System.Text;

namespace COVID19Tracker.ServerData
{
    [Serializable]
    public class ServerInstanceReportPacket
    {
        public string MachineID;
        public string FullName;
        public string PhoneNumber;
        public string DoctorsName;
        public string DoctorsPhoneNumber;
    }
}

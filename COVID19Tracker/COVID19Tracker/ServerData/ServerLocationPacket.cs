using System;
using System.Collections.Generic;
using System.Text;

namespace COVID19Tracker.ServerData
{
    [Serializable]
    public class ServerLocationPacket
    {
        public string MachineID;
        public string TransactionID;
        public double Latitude;
        public double Longitude;
        public double Altitude;
        public long Timestamp;
    }
}

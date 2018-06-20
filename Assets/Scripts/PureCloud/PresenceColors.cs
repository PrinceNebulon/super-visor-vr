using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.PureCloud
{
    public static class PresenceColors
    {
        public static Color Available = new Color(119, 221, 34);
        public static Color Busy = new Color(255, 0, 0);
        public static Color Away = new Color(241, 159, 0);
        public static Color OutOfOffice = new Color(255, 29, 206);
        public static Color OnQueue = new Color(82, 206, 248);
        public static Color Offline = new Color(102, 102, 102);

        public static Color FromSystemPresence(string systemPresence)
        {
            Debug.Log("Checking presence: " + systemPresence.ToUpperInvariant());
            switch (systemPresence.ToUpperInvariant())
            {
                case "AVAILABLE":
                    return Available;
                case "AWAY":
                case "MEAL":
                case "TRAINING":
                case "BREAK":
                    return Away;
                case "BUSY":
                case "MEETING":
                    return Busy;
                case "IDLE":
                case "ON_QUEUE":
                    return OnQueue;
                default:
                    return Offline;
            }
        }
    }
}

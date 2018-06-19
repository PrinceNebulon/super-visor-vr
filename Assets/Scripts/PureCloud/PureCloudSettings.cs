using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.PureCloud
{
    class PureCloudSettings
    {
        private static PureCloudSettings _instance;
        public static PureCloudSettings Instance {
            get
            {
                if (_instance == null)
                    _instance = new PureCloudSettings();
                return _instance;
            }
        }

        public Dictionary<string, string> Settings { get; private set; }

        private PureCloudSettings()
        {
            this.Settings = new Dictionary<string, string>();

            Debug.Log("CWD: " + Directory.GetCurrentDirectory());
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/local/settings.txt");

            foreach (var line in lines)
            {
                if (line.Trim().Length == 0) continue;
                if (line.StartsWith("//")) continue;

                var parts = line.Trim().Split(new char[] { '=' }, 2);
                Debug.Log(parts[0] + " = " + parts[1]);
                this.Settings[parts[0]] = parts[1];
            }
        }

        public string Get(string key)
        {
            if (this.Settings.ContainsKey(key))
                return this.Settings[key];
            else
                return "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Frame.DAL.Model
{
    public class ControlSettings
    {
        public bool IsDisplayAtBeginning;
        public static ControlSettings LoadFromInfo(string info)
        {
            return JsonUtility.FromJson<ControlSettings>(info);
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }
}

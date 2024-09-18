using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Frame.DAL.Model
{
    public class GeneralSettings
    {
        public static GeneralSettings LoadFromInfo(string info)
        {
            return JsonUtility.FromJson<GeneralSettings>(info);
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }
}

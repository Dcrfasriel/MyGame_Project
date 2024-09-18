using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Frame.DAL.Model
{
    public class RenderSettings
    {
        public static RenderSettings LoadFromInfo(string info)
        {
            return JsonUtility.FromJson<RenderSettings>(info);
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }
}

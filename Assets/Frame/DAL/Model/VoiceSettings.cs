using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Frame.DAL.Model
{
    public class VoiceSettings
    {
        public float EffectVolume;
        public float BackGroundVolume;

        public VoiceSettings()
        {
            
        }
        public static VoiceSettings LoadFromInfo(string info)
        {
            return JsonUtility.FromJson<VoiceSettings>(info);
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }
}

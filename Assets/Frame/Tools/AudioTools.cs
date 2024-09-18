using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Frame.Tools
{
    public static class AudioTools
    {
        public static void ControllAudioSource(AudioSourceController audio, bool IsPlay, float MaxVolume, float IncreaseSpeed, float DecreaseSpeed)
        {
            if (IsPlay)
            {
                if (audio.FVolume < MaxVolume)
                {
                    audio.FVolume += IncreaseSpeed * Time.deltaTime;
                }
                else
                {
                    audio.FVolume = MaxVolume;
                }
            }
            else
            {
                if (audio.FVolume > 0)
                {
                    audio.FVolume -= DecreaseSpeed * Time.deltaTime;
                }
                else
                {
                    audio.FVolume = 0f;
                }
            }
        }
    }
}

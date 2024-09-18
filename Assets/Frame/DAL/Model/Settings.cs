using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Frame.DAL.Model
{
    public class Settings
    {
        public string Name { get; set; }
        public VoiceSettings VoiceSetting { get;set; }

        public ControlSettings ControlSetting { get;set; }

        public RenderSettings RenderSetting { get;set; }

        public PlayerInfo PlayerInfo { get; set; }

        public GeneralSettings GeneralSetting { get;set; }

        public Settings(BaseInfo info)
        {
            if (info.Name == "" || info.Name == null) Name = "";
            else Name = info.Name;

            if (info.VolumeInfo==""||info.VolumeInfo==null)VoiceSetting = new VoiceSettings();
            else VoiceSetting=VoiceSettings.LoadFromInfo(info.VolumeInfo);

            if(info.RenderInfo==""||info.RenderInfo==null)RenderSetting = new RenderSettings();
            else RenderSetting=RenderSettings.LoadFromInfo(info.RenderInfo);

            if (info.ControlInfo == "" || info.ControlInfo == null) ControlSetting = new ControlSettings();
            else ControlSetting = ControlSettings.LoadFromInfo(info.ControlInfo);

            if (info.GeneralSettingInfo == "" || info.GeneralSettingInfo == null) GeneralSetting = new GeneralSettings();
            else GeneralSetting = GeneralSettings.LoadFromInfo(info.GeneralSettingInfo);

            if (info.PlayerInfo == "" || info.PlayerInfo == null) PlayerInfo = new PlayerInfo();
            else PlayerInfo = PlayerInfo.LoadFromInfo(info.PlayerInfo);
        }

        public BaseInfo ToBaseInfo()
        {
            return new BaseInfo(Name, VoiceSetting.Serialize(), RenderSetting.Serialize(), ControlSetting.Serialize(), PlayerInfo.Serialize(), GeneralSetting.Serialize());

        }

        public Settings(string Name = "", VoiceSettings voiceSettings = null, RenderSettings renderSettings = null, ControlSettings controlSettings = null, PlayerInfo playerInfo = null, GeneralSettings generalSettings = null)
        {
            this.Name = Name;
            this.VoiceSetting = voiceSettings ?? new VoiceSettings();
            this.RenderSetting = renderSettings ?? new RenderSettings();
            this.ControlSetting = controlSettings ?? new ControlSettings();
            this.PlayerInfo = playerInfo ?? new PlayerInfo();
            this.GeneralSetting = generalSettings ?? new GeneralSettings();
        }

    }
}

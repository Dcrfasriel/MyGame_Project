using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Frame.DAL.Model
{
    public class BaseInfo
    {
        public long ID { get; set; } 

        public string Name { get; set; }    

        public string VolumeInfo { get; set; }

        public string RenderInfo { get; set; }

        public string ControlInfo { get; set; }

        public string  PlayerInfo  { get; set; }

        public string GeneralSettingInfo { get; set; }

        public BaseInfo(object[] data)
        {
            ID = (long)data[0];
            Name = (string)data[1];
            VolumeInfo = (string)data[2];
            RenderInfo = (string)data[3];
            ControlInfo = (string)data[4];
            PlayerInfo = (string)data[5];
            GeneralSettingInfo = (string)data[6];
        }

        public BaseInfo(string name="", string volumeInfo="", string renderInfo="", string controlInfo = "", string playerInfo = "",string generalinfo="")
        {
            Name = name;
            VolumeInfo = volumeInfo;
            RenderInfo = renderInfo;
            ControlInfo = controlInfo;
            PlayerInfo = playerInfo;
            GeneralSettingInfo=generalinfo;
        }
    }
}

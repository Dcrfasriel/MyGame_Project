using Assets.Frame.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Assets.Library.DAL;

namespace Assets.Frame.DAL
{
    public static class DataAccessor
    {
        public static DALConfig config;

        private static SqlLiteHelper sql;

        static DataAccessor()
        {
            //string json = File.ReadAllText(Application.dataPath+"/Resources/Config/DALconfig.json");
            string json = "{\r\n  \"BaseInfoPath\": \"/Resources/DataBase/BaseInfo.sqlite\"\r\n}";
            config = JsonUtility.FromJson<DALConfig>(json);
            sql = new SqlLiteHelper(config.BaseInfoPath);
        }

        public static BaseInfo LoadBaseInfo(string Name)
        {
            sql.Open();
            List<BaseInfo> infos=sql.ExecuteMutipleQuery("SELECT * FROM BaseInfoTable").Select(p=>new BaseInfo(p)).Where(p=>string.Compare(p.Name,Name)==0).ToList();
            sql.Close();
            return infos.FirstOrDefault();
        }

        public static int SaveBaseInfo(BaseInfo info)
        {

            // 检查数据库中是否已经存在具有相同Name的记录
            BaseInfo existingInfo = LoadBaseInfo(info.Name);

            if (existingInfo != null)
            {
                sql.Open();
                // 如果存在，则执行更新操作
                int rowsAffected = sql.ExecuteNonQuery($"UPDATE BaseInfoTable SET VolumeInfo='{info.VolumeInfo}', RenderInfo='{info.RenderInfo}', ControlInfo='{info.ControlInfo}', PlayerInfo='{info.PlayerInfo}', GeneralSettingInfo='{info.GeneralSettingInfo}' WHERE Name='{info.Name}'");
                sql.Close();
                return rowsAffected;
            }
            else
            {
                sql.Open();
                // 否则执行插入操作
                int rowsAffected = sql.ExecuteNonQuery($"INSERT INTO BaseInfoTable (Name, VolumeInfo, RenderInfo, ControlInfo, PlayerInfo, GeneralSettingInfo) VALUES ('{info.Name}', '{info.VolumeInfo}', '{info.RenderInfo}', '{info.ControlInfo}', '{info.PlayerInfo}', '{info.GeneralSettingInfo}')");
                sql.Close();
                return rowsAffected;
            }
        }
    }
}

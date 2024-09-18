using Assets.Frame.DAL;
using Assets.Frame.DAL.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsLoader : MonoBehaviour
{
    #region Singleton
    private static SettingsLoader instance;

    public static SettingsLoader GetInstance() { return instance; }

    #endregion

    public Settings CurrentSettings;
    public Settings DefaultSettings;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        CurrentSettings = new Settings(DataAccessor.LoadBaseInfo("Current") ?? DataAccessor.LoadBaseInfo("Default"));
        DefaultSettings = new Settings(DataAccessor.LoadBaseInfo("Default"));
        CurrentSettings.Name = "Current";
    }

    private void OnApplicationQuit()
    {
        DataAccessor.SaveBaseInfo(CurrentSettings.ToBaseInfo());
    }
}

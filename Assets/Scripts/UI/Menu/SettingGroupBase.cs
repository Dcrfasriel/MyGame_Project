using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingGroupBase : MonoBehaviour
{
    private SettingsPanel settingsPanel;
    private bool HasSet = false;
    private List<GameObject> SelectObjects = new List<GameObject>();

    private void Awake()
    {
        settingsPanel=transform.parent.GetComponent<SettingsPanel>();
    }

    private void Update()
    {
        if (!HasSet&&settingsPanel.IsLoadComplete())
        {
            HasSet = true;
            for(int i=0;i<transform.childCount;i++)
            {
                GameObject gameObject=transform.GetChild(i).gameObject;
                if(gameObject.GetComponent<ISelectHandler>() != null)
                {
                    SelectObjects.Add(gameObject);
                }
            }
        }
    }

    public void FirstSelect()
    {
        if(SelectObjects.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(SelectObjects[0]);
        }
    }
}

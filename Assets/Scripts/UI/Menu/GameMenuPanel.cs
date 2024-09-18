using Assets.Frame.Interface;
using Assets.Frame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuPanel : MonoBehaviour, IUIPanel
{
    [Header("ÔÝÍ£²Ëµ¥")]
    public PausePanel pausePanel;

    private EventGroup eventGroup = new EventGroup();
    
    public void FirstSelect()
    {
        pausePanel.FirstSelect();
        return;
    }

    public EventGroup GetEventGroup()
    {
        return eventGroup;
    }

    public void SetAble(bool able)
    {
        gameObject.SetActive(able);
    }
}

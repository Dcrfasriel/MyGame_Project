using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SpaceShip : MonoBehaviour
{
    [Header("登入或离开飞船按键")]
    public KeyCode EnterOrExitSpaceShipKeyCode;
    [Header("最小进入距离")]
    public float MinEnterDistance;

    private GameObject SpaceShipModule;
    private Transform CenterPoint;
    private CharacterEventCenter eventCenter;
    private Transform ViewPoint;
    private Transform ExitPoint;
    private float EnterTime=0;
    private AudioSource audioSource;

    private void Awake()
    {
        CenterPoint = transform.Find("CenterPoint");
        ViewPoint= transform.Find("ViewPoint");
        ExitPoint = transform.Find("ExitPoint");
        audioSource=GetComponent<AudioSource>();
    }

    private void Start()
    {
        eventCenter=CharacterEventCenter.GetInstance();
        eventCenter.RegisterStateAction(CharacterState.Normal, CharacterState.OnSpaceShip, () =>
        {
            BoardOnSpaceShip();
        });
        eventCenter.RegisterStateAction(CharacterState.OnSpaceShip, CharacterState.Normal, () =>
        {
            ExitSpaceShip();
        });
        GameUIManager.GetInstance().AddEventToSurfaceUI((key, u) =>
        {
            if (!u)
            {
                return;
            }
            if (Input.GetKeyDown(EnterOrExitSpaceShipKeyCode)&&CanEnter())
            {
                eventCenter.CurrentState = CharacterState.OnSpaceShip;
            }
            if (Input.GetKeyDown(EnterOrExitSpaceShipKeyCode) && CanExit())
            {
                eventCenter.CurrentState = CharacterState.Normal;
            }
        }, EnterOrExitSpaceShipKeyCode);
    }
    private void Update()
    {
        if(eventCenter.CurrentState == CharacterState.OnSpaceShip)
        {
            EnterTime += Time.deltaTime;
        }
        else
        {
            EnterTime = 0;
        }
        SetCharacterSpeedForSeconds();
    }
    private void OnGUI()
    {
        if(CanEnter())
        {
            GameUIManager.GetInstance().SurfaceUI.SetKeyCodeMonitor(true, "Press " + EnterOrExitSpaceShipKeyCode.ToString() + " to Enter or Exit");
        }
        else
        {
            GameUIManager.GetInstance().SurfaceUI.SetKeyCodeMonitor(false, "");
        }
    }

    private bool CanEnter()
    {
        return eventCenter.CurrentState == CharacterState.Normal && (eventCenter.Character.transform.position - CenterPoint.position).magnitude <= MinEnterDistance;
    }

    private bool CanExit()
    {
        return eventCenter.CurrentState == CharacterState.OnSpaceShip&&EnterTime>=3f;
    }

    private void BoardOnSpaceShip()
    {
        eventCenter.Character.SetActive(false);
        GameDataCenter.GetInstance().AxisPoint.GetComponent<AxisPointFol>().SetFollow(ViewPoint, Vector3.zero,true);
        Camera.main.GetComponent<CameraFol>().IsLockTransform = true;
        GameObject chara = CharacterEventCenter.GetInstance().Character;
        chara.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void ExitSpaceShip()
    {
        GameObject chara=eventCenter.Character;
        chara.SetActive(true);
        chara.transform.position=ExitPoint.position;
        SettingCount = 1f;
        Vector3 a = GetComponent<Rigidbody>().velocity;
        eventCenter.Character.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        GameDataCenter.GetInstance().AxisPoint.GetComponent<AxisPointFol>().RecoverData();
        Camera.main.GetComponent<CameraFol>().IsLockTransform = false;
        audioSource.volume = 0;
    }
    private float SettingCount = 0;
    private void SetCharacterSpeedForSeconds()
    {
        if (SettingCount > 0.1)
        {
            SettingCount-=Time.deltaTime;
            Vector3 a = GetComponent<Rigidbody>().velocity;
            if (!eventCenter.Character.GetComponent<Rigidbody>().isKinematic)
            {
                eventCenter.Character.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            }
        }
    }
}

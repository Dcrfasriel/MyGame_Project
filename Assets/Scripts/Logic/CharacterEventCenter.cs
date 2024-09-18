using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEventCenter : MonoBehaviour
{
    [Header("角色")]
    public GameObject Character;

    private BoxCheckPoint CheckPoint;
    private void Awake()
    {
        instance = this;
        CheckPoint = Character.transform.Find("GroundCheckPoint").gameObject.GetComponent<BoxCheckPoint>();
        PlayerRigibody = Character.GetComponent<Rigidbody>();
    }

    #region BaseInfo

    private Rigidbody PlayerRigibody;
    
    public Rigidbody GetPlayerRigibody()
    {
        return PlayerRigibody;
    }

    #endregion

    #region Singleton

    private static CharacterEventCenter instance;

    public static CharacterEventCenter GetInstance()
    {
        return instance;
    }

    
    #endregion

    #region Floating
    private void CheckFloating()
    {
        FloatingCount++;
        if (FloatingCount >= 5)
        {
            FloatingCount = 5;
            if (CurrentCenter != null)
            {
                OnCenterChange?.Invoke(null);
            }
            CurrentCenter = null;
        }
    }

    private int FloatingCount = 0;
    public bool IsFloating
    {
        get { return FloatingCount >= 5; }
    }
    public Vector3? CurrentCenter=null;
    public void SetCurrentCenter(Vector3 Center)
    {
        FloatingCount = 0;
        if(CurrentCenter!=Center)
        {
            OnCenterChange?.Invoke(Center);
        }
        CurrentCenter = Center;

    }

    public event Action<Vector3?> OnCenterChange;

    #endregion

    #region OnGround

    public bool IsOnGround
    {
        get
        {
            if(CheckPoint.isActiveAndEnabled)  return CheckPoint.IsOnGround;
            else return false;
        }
    }

    #endregion

    #region State
    private StateTransitTable TransitionTable =new();
    private CharacterState currentState=CharacterState.Normal;
    public CharacterState CurrentState
    {
        get { return currentState; }
        set
        {
            OnStateChange?.Invoke(currentState,value);
            TransitionTable.InvokeActions(CurrentState, value);
            currentState = value;
        }
    }
    public event Action<CharacterState, CharacterState> OnStateChange;

    public void RegisterStateAction(CharacterState last, CharacterState now,Action action)
    {
        TransitionTable.AddTransition(last, now,action);
    }
    private class StateTransitTable
    {
        Dictionary<(CharacterState Last, CharacterState Now), List<Action>> StateTable = new Dictionary<(CharacterState, CharacterState), List<Action>>();

        // 添加状态过渡
        public void AddTransition(CharacterState lastState, CharacterState nowState, Action transitionAction)
        {
            (CharacterState, CharacterState) key = (lastState, nowState);

            // 如果状态过渡已存在，则添加过渡行为到现有列表中
            if (StateTable.ContainsKey(key))
            {
                StateTable[key].Add(transitionAction);
            }
            else // 否则创建新的列表，并添加新的状态过渡
            {
                List<Action> transitionActions = new List<Action>();
                transitionActions.Add(transitionAction);
                StateTable.Add(key, transitionActions);
            }
        }

        // 删除状态过渡
        public void RemoveTransition(CharacterState lastState, CharacterState nowState)
        {
            (CharacterState, CharacterState) key = (lastState, nowState);

            // 如果状态过渡存在，则移除它
            if (StateTable.ContainsKey(key))
            {
                StateTable.Remove(key);
            }
        }

        // 查询状态过渡
        public void InvokeActions(CharacterState lastState, CharacterState nowState)
        {
            if (ContainsKey(lastState, nowState))
            {
                foreach(Action action in StateTable[(lastState, nowState)])
                {
                    action.Invoke();
                }
            }
        }

        // 判断状态过渡是否存在
        public bool ContainsKey(CharacterState lastState, CharacterState nowState)
        {
            (CharacterState, CharacterState) key = (lastState, nowState);
            return StateTable.ContainsKey(key);
        }
    }

    #endregion

    private void Update()
    {
        CheckFloating();
    }

    
}

public enum CharacterState
{
    Normal,
    OnSpaceShip
}

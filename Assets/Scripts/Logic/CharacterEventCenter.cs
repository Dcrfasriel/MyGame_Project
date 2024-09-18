using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEventCenter : MonoBehaviour
{
    [Header("��ɫ")]
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

        // ���״̬����
        public void AddTransition(CharacterState lastState, CharacterState nowState, Action transitionAction)
        {
            (CharacterState, CharacterState) key = (lastState, nowState);

            // ���״̬�����Ѵ��ڣ�����ӹ�����Ϊ�������б���
            if (StateTable.ContainsKey(key))
            {
                StateTable[key].Add(transitionAction);
            }
            else // ���򴴽��µ��б�������µ�״̬����
            {
                List<Action> transitionActions = new List<Action>();
                transitionActions.Add(transitionAction);
                StateTable.Add(key, transitionActions);
            }
        }

        // ɾ��״̬����
        public void RemoveTransition(CharacterState lastState, CharacterState nowState)
        {
            (CharacterState, CharacterState) key = (lastState, nowState);

            // ���״̬���ɴ��ڣ����Ƴ���
            if (StateTable.ContainsKey(key))
            {
                StateTable.Remove(key);
            }
        }

        // ��ѯ״̬����
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

        // �ж�״̬�����Ƿ����
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

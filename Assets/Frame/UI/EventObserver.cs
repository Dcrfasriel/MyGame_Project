using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Frame.UI
{
    public class ConditionObserver
    {
        private Dictionary<string, bool> CloseTable;
        private Dictionary<string, bool> ConditionTable;
        private Dictionary<string, Func<bool>> CheckConditionTable;
        private Dictionary<string, bool> CheckByOneTimeTable;
        private Dictionary<string, List<TrigAction>> EventTable;
        private List<string> AvoidCheckList = new List<string>();

        public ConditionObserver()
        {
            ConditionTable = new Dictionary<string, bool>();
            CloseTable = new Dictionary<string, bool>();
            EventTable = new Dictionary<string, List<TrigAction>>();
            CheckConditionTable = new Dictionary<string, Func<bool>>();
            CheckByOneTimeTable = new Dictionary<string, bool>();
        }

        public void SetEvent(string Name,bool Value,bool isClose=false)
        {
            ConditionTable[Name] = Value;
            if(!CloseTable.ContainsKey(Name))
            {
                CloseTable[Name] = isClose;
            }
        }

        public bool GetValue(string Name)
        {
            return ConditionTable.ContainsKey(Name) && ConditionTable[Name];
        }

        public void AddObserver(string Name,Func<bool, bool> Observer,bool IsTrigForOneTime=true)
        {
            if(EventTable.ContainsKey(Name))
            {
                EventTable[Name].Add(new TrigAction(Observer,IsTrigForOneTime));
            }
            else
            {
                EventTable[Name] = new List<TrigAction>();
                EventTable[Name].Add(new TrigAction(Observer, IsTrigForOneTime));
            }
        }

        public bool IsClose(string Name)
        {
            return CloseTable.ContainsKey(Name)&&CloseTable[Name];
        }

        public void AddCondtionCheck(string Name,Func<bool> func,bool IsCheckOneTime=false)
        {
            CheckConditionTable[Name] = func;
            CheckByOneTimeTable[Name] = IsCheckOneTime;
        }

        public void Update()
        {
            foreach(string key in CheckConditionTable.Keys)
            {
                if (!AvoidCheckList.Contains(key)&&CheckConditionTable[key].Invoke())
                {
                    SetEvent(key, true);
                    if (CheckByOneTimeTable[key])
                    {
                        AvoidCheckList.Add(key);
                    }
                }
            }
            foreach(string Key in EventTable.Keys)
            {
                foreach(TrigAction Action in EventTable[Key])
                {
                    if(!Action.IsTrigForOneTime||!Action.HasTrig)
                    {
                        bool isTrig=Action.action.Invoke(GetValue(Key));
                        if (isTrig)
                        {
                            Action.HasTrig=true;
                        }
                    }
                }
                if(GetValue(Key) &&IsClose(Key))
                {
                    SetEvent(Key,false);
                }
            }
        }
    }

    public class TrigAction
    {
        public Func<bool,bool> action;
        public bool IsTrigForOneTime;
        public bool HasTrig;

        public TrigAction(Func<bool, bool> action, bool isTrigForOneTime)
        {
            this.action = action;
            IsTrigForOneTime = isTrigForOneTime;
            HasTrig = false;
        }
    }
}

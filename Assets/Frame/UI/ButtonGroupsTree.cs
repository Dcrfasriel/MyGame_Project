using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Frame.UI
{
    public class ButtonGroupsTree
    {
        private IList<ButtonGroup> groups;

        private int AppearIndex=-1;
        private int activeGroupIndex=-1;
        public int ActiveGroupIndex
        {
            get { return activeGroupIndex ; }
            set
            {
                if (activeGroupIndex != -1) groups[activeGroupIndex].IsActive = false;
                activeGroupIndex = value ;
                if(value!=-1) groups[activeGroupIndex].IsActive = true;
            }
        }

        public int AppearIndexGroup
        {
            get { return AppearIndex; }
        }

        public ButtonGroupsTree(params ButtonGroup[] groups)
        {
            this.groups = groups;
            SetAllGroupDisAppear();
        }
        
       

        private void SetAllGroupDisAppear()
        {
            foreach(ButtonGroup group in groups)
            {
                group.AnimationCount = 0;
            }
            UpdateAll();
        }

        public void ActiveGroup(int index)
        {
            ActiveGroupIndex = index ;
        }

        public void Update()
        {
            if (AppearIndex != -1 && AppearIndex != ActiveGroupIndex)
            {
                EventSystem.current.SetSelectedGameObject(null);
                groups[AppearIndex].Update();
                if (groups[AppearIndex].IsDisAppear)
                {
                    groups[AppearIndex].SetActive(false);
                    AppearIndex = ActiveGroupIndex;
                }
            }
            else
            {
                AppearIndex = ActiveGroupIndex;
                if (ActiveGroupIndex != -1)
                {
                    for(int i=0;i<groups.Count;i++)
                    {
                        if (i == ActiveGroupIndex) groups[i].SetActive(true);
                        else groups[i].SetActive(false);
                    }
                    groups[ActiveGroupIndex].Update();
                }
            }
        }

        public void UpdateAll()
        {
            foreach(ButtonGroup buttonGroup in groups)
            {
                buttonGroup.Update();
            }
        }

        public void Select(int index)
        {
            if (activeGroupIndex != -1)
            {
                groups[activeGroupIndex].Select(index);
            }
        }

        public void SetAllActive(bool value)
        {
            foreach(ButtonGroup group in groups)
            {
                group.SetActive(value);
            }
        }
    }
}

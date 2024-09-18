using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Frame.UI
{
    public class ButtonGroup
    {
        public float AnimationCount { get; set; } = 0;

        public float FullMovingTime { get;set; } = 0;

        public float V0 { get; set; } = 1;

        public float Interval { get;set; } = 0;

        public float StartX { get; set; } = 0;

        public float EndX { get; set; } = 0;

        public bool IsActive { get; set; } = false;

        public bool IsDisAppear
        {
            get
            {
                return AnimationCount <=0;
            }
        }

        public float DispearParam { get; set; } = 5;

        public int ParentGroupIndex = -1;


        private float D = 0;
        private IList<GameObject> Buttons;
        private List<RectTransform> Transforms;
        public ButtonGroup(IList<GameObject> Buttons,float Interval,float StartX,float EndX,float FullMovingTime,float V0,float DisParam)
        {
            this.Buttons = Buttons;
            Transforms=Buttons.Select(t=>t.GetComponent<RectTransform>()).ToList();
            this.StartX = StartX;
            this.EndX = EndX;
            this.FullMovingTime = FullMovingTime;
            this.V0 = V0;
            D = EndX - StartX;
            this.Interval = Interval;
            this.DispearParam = DisParam;
        }

        public void Update()
        {
            if(IsActive)
            {
                AnimationCount =Math.Clamp(AnimationCount+Time.deltaTime,0,FullMovingTime);
            }
            else
            {
                AnimationCount = Math.Clamp(AnimationCount - Time.deltaTime*Mathf.Max(AnimationCount*DispearParam,1), 0, FullMovingTime);
            }
            for(int i = 0; i < Buttons.Count; i++)
            {
                Vector3 Org = Transforms[i].position;
                float t = AnimationCount - i * Interval;
                t = Mathf.Clamp(t, 0, FullMovingTime);
                Transforms[i].position = new Vector3(CalcualateX(t), Org.y, Org.z);
            }
        }

        private float CalcualateX(float t)
        {
            return D - D * Mathf.Exp(-V0 * t / D)+StartX;
        }

        public void Select(int index)
        {
            EventSystem.current.SetSelectedGameObject(Buttons[index]);
        }

        public void SetActive(bool value)
        {
            foreach(GameObject gameObject in Buttons)
            {
                gameObject.SetActive(value);
            }
        }
    }
}

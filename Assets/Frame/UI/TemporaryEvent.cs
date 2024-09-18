using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Frame.UI
{
    public class TemporaryEvent
    {
        public Action Event;

        public Func<bool> Condition;

        public bool WillTrig = false;

        public TemporaryEvent()
        {
            Condition = DefaultFunc;
        }
        public void CheckUpdate()
        {
            if (WillTrig&&Condition.Invoke())
            {
                Event?.Invoke();
                WillTrig = false;
            }
        }
        private static Func<bool> DefaultFunc;
        public void StartEvent(Action action,Func<bool> Condition = null)
        {
            if(Condition==null)Condition = DefaultFunc;
            Event = action;
            this.Condition = Condition;
            WillTrig=true;
        }
    }
}

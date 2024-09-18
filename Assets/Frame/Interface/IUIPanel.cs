using Assets.Frame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Frame.Interface
{
    public interface IUIPanel
    {
        public EventGroup GetEventGroup();

        public void SetAble(bool able);

        public void FirstSelect();
    }
}

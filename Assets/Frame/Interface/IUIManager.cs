using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Frame.Interface
{
    public interface IUIManager
    {
        public void OnButtonHover();
        public void OnButtonClick();
        public void OnSliderMove();
    }
}

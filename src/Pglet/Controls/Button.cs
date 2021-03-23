using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Button : Control
    {
        public string Text
        {
            get { return GetAttr("text"); }
            set { SetAttr("text", value); }
        }

        public EventHandler OnClick
        {
            get { return GetEventHandler("click"); }
            set { SetEventHandler("click", value); }
        }
    }
}

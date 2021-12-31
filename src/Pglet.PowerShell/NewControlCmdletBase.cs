using System;
using System.Management.Automation;
using Pglet.PowerShell.Controls;

namespace Pglet.PowerShell
{
    public class NewControlCmdletBase : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Id { get; set; }

        [Parameter(Mandatory = false)]
        public string Width { get; set; }

        [Parameter(Mandatory = false)]
        public string Height { get; set; }

        [Parameter(Mandatory = false)]
        public string Padding { get; set; }

        [Parameter(Mandatory = false)]
        public string Margin { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Visible { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Disabled { get; set; }

        [Parameter(Mandatory = false)]
        public object Data { get; set; }

        protected void SetControlProps(Control control)
        {
            var eventCtrl = control as IPsEventControl;
            if (eventCtrl != null)
            {
                eventCtrl.Cmdlet = this;
            }

            control.Id = Id;
            control.Width = Width;
            control.Height = Height;
            control.Padding = Padding;
            control.Margin = Margin;

            if (Visible.IsPresent)
            {
                control.Visible = Visible.ToBool();
            }

            if (Disabled.IsPresent)
            {
                control.Disabled = Disabled.ToBool();
            }

            control.Data = Data;
        }
    }
}

﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pglet.Controls
{
    public class Stack : Control
    {
        protected override string ControlName => "stack";

        ControlCollection<Control> _controls = new();

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _controls.SetDataLock(dataLock);
        }

        public ControlCollection<Control> Controls
        {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _controls;
                }
                finally
                {
                    dlock.ReleaseReaderLock();
                }
            }
            set
            {
                var dlock = _dataLock;
                dlock.AcquireWriterLock();
                try
                {
                    _controls.SetDataLock(_dataLock);
                    _controls = value;
                }
                finally
                {
                    dlock.ReleaseWriterLock();
                }
            }
        }

        public bool Horizontal
        {
            get { return GetBoolAttr("horizontal"); }
            set { SetBoolAttr("horizontal", value); }
        }

        public bool VerticalFill
        {
            get { return GetBoolAttr("verticalFill"); }
            set { SetBoolAttr("verticalFill", value); }
        }

        public Align HorizontalAlign
        {
            get { return GetEnumAttr<Align>("horizontalAlign"); }
            set { SetEnumAttr("horizontalAlign", value); }
        }

        public Align VerticalAlign
        {
            get { return GetEnumAttr<Align>("verticalAlign"); }
            set { SetEnumAttr("verticalAlign", value); }
        }

        public string MinWidth
        {
            get { return GetAttr("minWidth"); }
            set { SetAttr("minWidth", value); }
        }

        public string MaxWidth
        {
            get { return GetAttr("maxWidth"); }
            set { SetAttr("maxWidth", value); }
        }

        public string MinHeight
        {
            get { return GetAttr("minHeight"); }
            set { SetAttr("minHeight", value); }
        }

        public string MaxHeight
        {
            get { return GetAttr("maxHeight"); }
            set { SetAttr("maxHeight", value); }
        }

        public int Gap
        {
            get { return GetIntAttr("gap"); }
            set { SetIntAttr("gap", value); }
        }

        public bool Wrap
        {
            get { return GetBoolAttr("wrap"); }
            set { SetBoolAttr("wrap", value); }
        }

        public bool ScrollX
        {
            get { return GetBoolAttr("scrollx"); }
            set { SetBoolAttr("scrollx", value); }
        }

        public bool ScrollY
        {
            get { return GetBoolAttr("scrolly"); }
            set { SetBoolAttr("scrolly", value); }
        }

        public string BgColor
        {
            get { return GetAttr("bgColor"); }
            set { SetAttr("bgColor", value); }
        }

        public string Border
        {
            get { return GetAttr("border"); }
            set { SetAttr("border", value); }
        }

        public string BorderRadius
        {
            get { return GetAttr("borderRadius"); }
            set { SetAttr("borderRadius", value); }
        }

        public string BorderLeft
        {
            get { return GetAttr("borderLeft"); }
            set { SetAttr("borderLeft", value); }
        }

        public string BorderRight
        {
            get { return GetAttr("borderRight"); }
            set { SetAttr("borderRight", value); }
        }

        public string BorderTop
        {
            get { return GetAttr("borderTop"); }
            set { SetAttr("borderTop", value); }
        }

        public string BorderBottom
        {
            get { return GetAttr("borderBottom"); }
            set { SetAttr("borderBottom", value); }
        }

        public EventHandler OnSubmit
        {
            get { return GetEventHandler("submit"); }
            set
            {
                SetEventHandler("submit", value);
                SetBoolAttr("onsubmit", value != null);
            }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls.GetInternalList();
        }

        public override async Task CleanAsync()
        {
            await base.CleanAsync();
            _controls.Clear();
        }
    }
}

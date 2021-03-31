using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Text : Control
    {
        protected override string ControlName => "text";

        public string Value
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", value); }
        }

        public bool Markdown
        {
            get { return GetBoolAttr("markdown"); }
            set { SetBoolAttr("markdown", value); }
        }

        public TextAlign Align
        {
            get { return GetEnumAttr<TextAlign>("align"); }
            set { SetEnumAttr("align", value); }
        }

        public TextVerticalAlign VerticalAlign
        {
            get { return GetEnumAttr<TextVerticalAlign>("verticalAlign"); }
            set { SetEnumAttr("verticalAlign", value); }
        }

        public TextSize Size
        {
            get { return GetEnumAttr<TextSize>("size"); }
            set { SetEnumAttr("size", value); }
        }

        public bool Bold
        {
            get { return GetBoolAttr("bold"); }
            set { SetBoolAttr("bold", value); }
        }

        public bool Italic
        {
            get { return GetBoolAttr("italic"); }
            set { SetBoolAttr("italic", value); }
        }

        public bool Pre
        {
            get { return GetBoolAttr("pre"); }
            set { SetBoolAttr("pre", value); }
        }

        public bool Nowrap
        {
            get { return GetBoolAttr("nowrap"); }
            set { SetBoolAttr("nowrap", value); }
        }

        public bool Block
        {
            get { return GetBoolAttr("block"); }
            set { SetBoolAttr("block", value); }
        }

        public string Color
        {
            get { return GetAttr("color"); }
            set { SetAttr("color", value); }
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

        public BorderStyle BorderStyle
        {
            get { return GetEnumAttr<BorderStyle>("borderStyle"); }
            set { SetEnumAttr("borderStyle", value); }
        }

        public string BorderWidth
        {
            get { return GetAttr("borderWidth"); }
            set { SetAttr("borderWidth", value); }
        }

        public string BorderColor
        {
            get { return GetAttr("borderColor"); }
            set { SetAttr("borderColor", value); }
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
    }
}

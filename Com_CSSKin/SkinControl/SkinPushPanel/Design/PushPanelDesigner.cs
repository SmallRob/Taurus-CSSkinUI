
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Collections;

namespace Com_CSSkin.SkinControl.Design
{
    internal class PushPanelDesigner : PanelDesigner
    {
        private DesignerActionListCollection _actionLists;

        public PushPanelDesigner()
            : base()
        {
        }

        public override bool CanParent(Control control)
        {
            if (control != null)
            {
                return control is PushPanelItem;
            }
            return false;
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_actionLists == null)
                {
                    _actionLists = base.ActionLists;
                    _actionLists.Add(new PushPanelActionList(this));
                }
                return _actionLists;
            }
        }

        protected override void PostFilterProperties(IDictionary properties)
        {
            base.PreFilterAttributes(properties);
            properties.Remove("AccessibilityObject");
            properties.Remove("AccessibleDefaultActionDescription");
            properties.Remove("AccessibleDescription");
            properties.Remove("AccessibleName");
            properties.Remove("AccessibleRole");
            properties.Remove("AntiAliasing");
            properties.Remove("AutoScroll");
            properties.Remove("AutoScrollMargin");
            properties.Remove("AutoScrollMinSize");
        }
    }

    internal class PushPanelActionList : DesignerActionList
    {
        private PushPanelDesigner _designer;

        public PushPanelActionList(PushPanelDesigner designer)
            : base(designer.Component)
        {
            _designer = designer;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(
                this,
                "InvokeItemDialog",
                "�༭��...",
                true));
            return items;
        }

        public void InvokeItemDialog()
        {
            EditorServiceContext.EditValue(_designer, base.Component, "Items");
        }
    }
}

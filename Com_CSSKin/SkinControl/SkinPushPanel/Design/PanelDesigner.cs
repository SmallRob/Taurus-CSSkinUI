
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing.Design;

namespace Com_CSSkin.SkinControl.Design
{
    #region PanelDesigner

    internal class PanelDesigner : ScrollableControlDesigner
    {
        public PanelDesigner()
        {
            base.AutoResizeHandles = true;
        }

        protected virtual void DrawBorder(Graphics graphics)
        {
            Panel component = (Panel)base.Component;
            if ((component != null) && component.Visible)
            {
                Pen borderPen = BorderPen;
                Rectangle clientRectangle = Control.ClientRectangle;
                clientRectangle.Width--;
                clientRectangle.Height--;
                graphics.DrawRectangle(borderPen, clientRectangle);
                borderPen.Dispose();
            }
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            Panel component = (Panel)base.Component;
            if (component.BorderStyle == BorderStyle.None)
            {
                DrawBorder(pe.Graphics);
            }
            base.OnPaintAdornments(pe);
        }

        protected Pen BorderPen
        {
            get
            {
                Color color = (Control.BackColor.GetBrightness() < 0.5) ?
                    ControlPaint.Light(Control.BackColor) :
                    ControlPaint.Dark(Control.BackColor);
                Pen pen = new Pen(color);
                pen.DashStyle = DashStyle.Dash;
                return pen;
            }
        }
    }

    #endregion

    #region EditorServiceContext

    internal class EditorServiceContext :
        IWindowsFormsEditorService,
        ITypeDescriptorContext,
        IServiceProvider
    {
        private IComponentChangeService _componentChangeSvc;
        private ComponentDesigner _designer;
        private PropertyDescriptor _targetProperty;

        internal EditorServiceContext(ComponentDesigner designer)
        {
            _designer = designer;
        }

        internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
        {
            _designer = designer;
            _targetProperty = prop;
            if (prop == null)
            {
                prop = TypeDescriptor.GetDefaultProperty(designer.Component);
                if ((prop != null) &&
                    typeof(ICollection).IsAssignableFrom(prop.PropertyType))
                {
                    _targetProperty = prop;
                }
            }
        }

        internal EditorServiceContext(
            ComponentDesigner designer,
            PropertyDescriptor prop,
            string newVerbText)
            : this(designer, prop)
        {
            _designer.Verbs.Add(new DesignerVerb(
                newVerbText,
                new EventHandler(OnEditItems)));
        }

        public static object EditValue(
            ComponentDesigner designer,
            object objectToChange,
            string propName)
        {
            PropertyDescriptor prop = TypeDescriptor.GetProperties(objectToChange)[propName];
            EditorServiceContext context = new EditorServiceContext(designer, prop);
            UITypeEditor editor = prop.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
            object obj2 = prop.GetValue(objectToChange);
            object obj3 = editor.EditValue(context, context, obj2);
            if (obj3 != obj2)
            {
                try
                {
                    prop.SetValue(objectToChange, obj3);
                }
                catch (CheckoutException)
                {
                }
            }
            return obj3;
        }

        private void OnEditItems(object sender, EventArgs e)
        {
            object component = _targetProperty.GetValue(_designer.Component);
            if (component != null)
            {
                CollectionEditor editor = TypeDescriptor.GetEditor(
                    component,
                    typeof(UITypeEditor)) as CollectionEditor;
                if (editor != null)
                {
                    editor.EditValue(this, this, component);
                }
            }
        }

        void ITypeDescriptorContext.OnComponentChanged()
        {
            ChangeService.OnComponentChanged(
                _designer.Component,
                _targetProperty,
                null,
                null);
        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            try
            {
                ChangeService.OnComponentChanging(
                    _designer.Component,
                    _targetProperty);
            }
            catch (CheckoutException exception)
            {
                if (exception != CheckoutException.Canceled)
                {
                    throw;
                }
                return false;
            }
            return true;
        }

        object IServiceProvider.GetService(System.Type serviceType)
        {
            if ((serviceType == typeof(ITypeDescriptorContext)) ||
                (serviceType == typeof(IWindowsFormsEditorService)))
            {
                return this;
            }
            if (_designer.Component.Site != null)
            {
                return _designer.Component.Site.GetService(serviceType);
            }
            return null;
        }

        void IWindowsFormsEditorService.CloseDropDown()
        {
        }

        void IWindowsFormsEditorService.DropDownControl(Control control)
        {
        }

        DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
        {
            IUIService service = (IUIService)((IServiceProvider)this).GetService(
                typeof(IUIService));
            if (service != null)
            {
                return service.ShowDialog(dialog);
            }
            return dialog.ShowDialog(_designer.Component as IWin32Window);
        }

        private IComponentChangeService ChangeService
        {
            get
            {
                if (_componentChangeSvc == null)
                {
                    _componentChangeSvc =
                        (IComponentChangeService)((IServiceProvider)this).GetService(
                        typeof(IComponentChangeService));
                }
                return _componentChangeSvc;
            }
        }

        IContainer ITypeDescriptorContext.Container
        {
            get
            {
                if (_designer.Component.Site != null)
                {
                    return _designer.Component.Site.Container;
                }
                return null;
            }
        }

        object ITypeDescriptorContext.Instance
        {
            get
            {
                return _designer.Component;
            }
        }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
        {
            get
            {
                return _targetProperty;
            }
        }
    }

    #endregion
}

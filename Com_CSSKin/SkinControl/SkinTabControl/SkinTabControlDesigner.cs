
using Com_CSSkin.Win32.Const;
using Com_CSSkin.Win32.Struct;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Com_CSSkin.SkinControl
{
    internal class SkinTabControlDesigner : ParentControlDesigner
    {
        #region Variables
        /// <summary>
        /// 
        /// </summary>
        private readonly DesignerVerbCollection _verbs = new DesignerVerbCollection();
        /// <summary>
        /// 
        /// </summary>
        private IDesignerHost _designerHost;
        /// <summary>
        /// 
        /// </summary>
        private ISelectionService _selectionService;
        #endregion

        #region Fields
        public override SelectionRules SelectionRules {
            get {
                return Control.Dock == DockStyle.Fill ? SelectionRules.Visible : base.SelectionRules;
            }
        }
        public override DesignerVerbCollection Verbs {
            get {
                if (_verbs.Count == 2) {
                    var myControl = (SkinTabControl)Control;
                    _verbs[1].Enabled = myControl.TabCount != 0;
                }
                return _verbs;
            }
        }

        public IDesignerHost DesignerHost {
            get {
                return _designerHost ?? (_designerHost = (IDesignerHost)(GetService(typeof(IDesignerHost))));
            }
        }

        public ISelectionService SelectionService {
            get {
                return _selectionService ?? (_selectionService = (ISelectionService)(GetService(typeof(ISelectionService))));
            }
        }
        #endregion

        #region Constructor
        public SkinTabControlDesigner() {
            var verb1 = new DesignerVerb("添加选项卡", OnAddPage);
            var verb2 = new DesignerVerb("移除选项卡", OnRemovePage);
            _verbs.AddRange(new[] { verb1, verb2 });
        }
        #endregion

        #region Private methods
        private void OnAddPage(Object sender, EventArgs e) {
            var parentControl = (SkinTabControl)Control;
            var oldTabs = parentControl.Controls;

            RaiseComponentChanging(TypeDescriptor.GetProperties(parentControl)["TabPages"]);

            var p = (SkinTabPage)(DesignerHost.CreateComponent(typeof(SkinTabPage)));
            p.Text = p.Name;
            parentControl.TabPages.Add(p);

            RaiseComponentChanged(TypeDescriptor.GetProperties(parentControl)["TabPages"],
                                  oldTabs, parentControl.TabPages);
            parentControl.SelectedTab = p;

            SetVerbs();
        }

        private void OnRemovePage(Object sender, EventArgs e) {
            var parentControl = (SkinTabControl)Control;
            var oldTabs = parentControl.Controls;

            if (parentControl.SelectedIndex < 0) {
                return;
            }

            RaiseComponentChanging(TypeDescriptor.GetProperties(parentControl)["TabPages"]);

            DesignerHost.DestroyComponent(parentControl.TabPages[parentControl.SelectedIndex]);

            RaiseComponentChanged(TypeDescriptor.GetProperties(parentControl)["TabPages"],
                                  oldTabs, parentControl.TabPages);

            SelectionService.SetSelectedComponents(new IComponent[]
            {
                parentControl
            }, SelectionTypes.Auto);

            SetVerbs();
        }

        private void SetVerbs() {
            var parentControl = (SkinTabControl)Control;

            switch (parentControl.TabPages.Count) {
                case 0:
                    Verbs[1].Enabled = false;
                    break;
                default:
                    Verbs[1].Enabled = true;
                    break;
            }
        }
        #endregion

        #region Overrides
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            switch (m.Msg) {
                case WM.WM_NCHITTEST:
                    if (m.Result.ToInt32() == (int)HITTEST.HTTRANSPARENT) {
                        m.Result = (IntPtr)HITTEST.HTCLIENT;
                    }
                    break;
            }
        }

        protected override bool GetHitTest(System.Drawing.Point point) {
            if (SelectionService.PrimarySelection == Control) {
                var hti = new TCHITTESTINFO {
                    Point = Control.PointToClient(point),
                    Flags = 0
                };

                var m = new Message {
                    HWnd = Control.Handle,
                    Msg = TCM.TCM_HITTEST
                };

                var lparam =
                    System.Runtime.InteropServices.Marshal.AllocHGlobal(System.Runtime.InteropServices.Marshal.SizeOf(hti));
                System.Runtime.InteropServices.Marshal.StructureToPtr(hti,
                                                                      lparam, false);
                m.LParam = lparam;

                base.WndProc(ref m);
                System.Runtime.InteropServices.Marshal.FreeHGlobal(lparam);

                if (m.Result.ToInt32() != -1) {
                    return hti.Flags != (int)TCHT.TCHT_NOWHERE;
                }
            }

            return false;
        }

        protected override void PreFilterProperties(IDictionary properties) {
            //properties.Remove("ImeMode");
            //properties.Remove("Padding");
            //properties.Remove("FlatAppearance");
            //properties.Remove("FlatStyle");
            //properties.Remove("AutoEllipsis");
            //properties.Remove("UseCompatibleTextRendering");

            //properties.Remove("Image");
            //properties.Remove("ImageAlign");
            //properties.Remove("ImageIndex");
            //properties.Remove("ImageKey");
            //properties.Remove("ImageList");
            //properties.Remove("TextImageRelation");

            //            properties.Remove("BackgroundImage");
            //           properties.Remove("BackgroundImageLayout");
            properties.Remove("UseVisualStyleBackColor");

            //properties.Remove("Font");
            //properties.Remove("ForeColor");
            //properties.Remove("RightToLeft");

            base.PreFilterProperties(properties);
        }
        #endregion
    }
}
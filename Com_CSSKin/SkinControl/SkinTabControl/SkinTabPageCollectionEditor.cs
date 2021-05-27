
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Com_CSSkin.SkinControl
{
    internal class SkinTabPageCollectionEditor : CollectionEditor
    {
        protected override CollectionForm CreateCollectionForm()
        {
            var baseForm = base.CreateCollectionForm();
            baseForm.Text = "SkinTabPage Collection Editor";
            return baseForm;
        }

        public SkinTabPageCollectionEditor(Type type)
            : base(type)
        { }

        protected override Type CreateCollectionItemType()
        {
            return typeof(SkinTabPage);
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new[] { typeof(SkinTabPage) };
        }
    }
}


using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Com_CSSkin.SkinControl
{
    public class SkinFileTansfersContainer : Panel
    {
        private IFileTransfersItemText _fileTransfersItemText;

        public SkinFileTansfersContainer()
            : base()
        {
            AutoScroll = true;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFileTransfersItemText FileTransfersItemText
        {
            get 
            {
                if (_fileTransfersItemText == null)
                {
                    _fileTransfersItemText = new FileTransfersItemText();
                }
                return _fileTransfersItemText;
            }
            set
            {
                _fileTransfersItemText = value;
                foreach (SkinFileTransfersItem item in Controls)
                {
                    item.FileTransfersText = _fileTransfersItemText;
                }
            }
        }

        public SkinFileTransfersItem AddItem(
            string text,
            string fileName,
            Image image,
            long fileSize,
            FileTransfersItemStyle style)
        {
            SkinFileTransfersItem item = new SkinFileTransfersItem();
            item.Text = text;
            item.FileName = fileName;
            item.Image = image;
            item.FileSize = fileSize;
            item.Style = style;
            item.FileTransfersText = FileTransfersItemText;
            item.Dock = DockStyle.Top;

            SuspendLayout();
            Controls.Add(item);
            item.BringToFront();
            ResumeLayout(true);

            return item;
        }

        public SkinFileTransfersItem AddItem(
            string name,
            string text,
            string fileName,
            Image image,
            long fileSize,
            FileTransfersItemStyle style)
        {
            SkinFileTransfersItem item = new SkinFileTransfersItem();
            item.Name = name;
            item.Text = text;
            item.FileName = fileName;
            item.Image = image;
            item.FileSize = fileSize;
            item.Style = style;
            item.FileTransfersText = FileTransfersItemText;
            item.Dock = DockStyle.Top;

            SuspendLayout();
            Controls.Add(item);
            item.BringToFront();
            ResumeLayout(true);

            return item;
        }

        public void RemoveItem(SkinFileTransfersItem item)
        {
            Controls.Remove(item);
        }

        public void RemoveItem(string name)
        {
            Controls.RemoveByKey(name);
        }

        public void RemoveItem(Predicate<SkinFileTransfersItem> match)
        {
            SkinFileTransfersItem itemRemove = null;
            foreach (SkinFileTransfersItem item in Controls)
            {
                if (match(item))
                {
                    itemRemove = item;
                }
            }
            Controls.Remove(itemRemove);
        }

        public SkinFileTransfersItem Search(string name)
        {
            return Controls[name] as SkinFileTransfersItem;
        }

        public SkinFileTransfersItem Search(Predicate<SkinFileTransfersItem> match)
        {
            foreach (SkinFileTransfersItem item in Controls)
            {
                if (match(item))
                {
                    return item;
                }
            }

            return null;
        }
    }
}

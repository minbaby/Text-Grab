using System;
using System.Windows.Forms;
using System.Drawing;
using SharpShell.SharpContextMenu;
using System.Runtime.InteropServices;
using SharpShell.Attributes;

namespace Text_Grab_Menu_Option
{
    /// <summary>
    /// The CountLinesExtensions is an example shell context menu extension,
    /// implemented with SharpShell. It adds the command 'Count Lines' to text
    /// files.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".png")]
    public class MenuOption : SharpContextMenu
    {
        protected override bool CanShowMenu()
        {
            //  We always show the menu
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();
            var itemCountLines = new ToolStripMenuItem
            {
                Text = "Text Grab",
                Image = Properties.Resources.Text_Grab_Icon
            };
            itemCountLines.Click += (sender, args) => ReadText();
            menu.Items.Add(itemCountLines);
            return menu;
        }

        private void ReadText()
        {
            throw new NotImplementedException();
        }
    }
}

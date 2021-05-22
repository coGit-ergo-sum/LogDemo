using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace Vi
{
    /// <summary>
    /// Implements a listView, specialized to show logs for Log4Vi (log4Net)
    /// </summary>
    /// <include file='Logger/XMLs/List4Log.xml' path='Docs/type[@name="List4Log"]/*' />
    public partial class List4Log : UserControl
    {
        /// <summary>
        /// Manages the bottom bar used to display every internal exception. (exception in this user control cannot be sent to the list view to avoid a loop, so that kind of exception are addressed to the bottom bar .)
        /// </summary>
        /// <param name="se">The exception to lon on the bottom bar.</param>
        private void Exception(System.Exception se)
        {
            label.Text = se.Message;
            listView.Height = this.Height - (panel.Height + 2);
            panel.Visible = true;
        }

        /// <summary>
        /// Nothing to write. Then the bottom bar is hidden.
        /// </summary>
        private void Exception()
        {
            label.Text = "";
            panel.Visible = false;
            listView.Height = this.Height - 2;
        }

        /// <summary>
        /// Main CTor. Initializes Components and the listView (adding the proper columns)
        /// </summary>
        public List4Log()
        {
            InitializeComponent();
            SetListViewHeader();
            panel.Visible = false;
            listView.Height = this.Height - 2;
        }

        /// <summary>
        /// Wraps the provided logger with the class 'Wrapper' 
        /// After this assignment, this object will intercept and send on the screen every message sent to the file.
        /// </summary>
        /// <param name="logger">Any kind of 'logger' that inherits from 'Vi.Shared.ILog'.</param>
        public void SetLogger(Vi.ILog logger)
        {
            var wrapper = new Wrapper(logger);
            wrapper.OnAppend += new Wrapper.OnAppendHandler(AppendItem);

            Vi.Logger.SetLogger(wrapper);

        }

        /// <summary>
        /// Grant access to the internal ILog logger
        /// </summary>
        /// <returns>The current logger used to 'write' messages.</returns>
        public ILog GetLogger()
        {
            return Vi.Logger.GetLogger();
        }

        /// <summary>
        /// Adds a message to the list view.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="level">Specifies which kind of log {Debug; Warn; ...}</param>
        /// <include file='Logger/XMLs/List4Log.xml' path='Docs/Member[@name="AppendItem"]/*' />
        /// <include file='Logger/XMLs/List4Log.xml' path='Docs/Member[@name="AppendItemPublic"]/*' />
        #region AppendItem
        public void AppendItem(string text, int line, string member, string file, Vi.Logger.Levels level)
        {
            if (this.listView.InvokeRequired)
            {
                Action _appendMessage = () => { AppendItem(level.ToString(), text, line, member, file, level); };
                this.listView.Invoke(_appendMessage);
            }
            else
            {
                AppendItem(level.ToString(), text, line, member, file, level);
            }
        } 
        #endregion

        /// <summary>
        /// Adds a message to the list view.
        /// </summary>
        /// <param name="ImageKey">The id of the icon.</param>
        /// <param name="text">The text to log.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="level">Specifies which kind of log {Debug; Warn; ...}</param>
        /// <include file='Logger/XMLs/List4Log.xml' path='Docs/Member[@name="AppendItem"]/*' />
        /// <include file='Logger/XMLs/List4Log.xml' path='Docs/Member[@name="AppendItemPrivate"]/*' />
        private void AppendItem(string ImageKey, string text, int line, string member, string file, Vi.Logger.Levels level)
        {
            try
            {
                ListViewItem lvi = new ListViewItem(member);
                Color foreColor = (level < Logger.Levels.FATAL) ? lvi.ForeColor : Color.Red;

                lvi.ImageKey = ImageKey;
                lvi.UseItemStyleForSubItems = false;
                lvi.ForeColor = foreColor;
                lvi.ImageKey = level.ToString();

                int n = 1 + this.listView.Items.Count;
                string fileName = System.IO.Path.GetFileName(file);

                SubItemsAdd(lvi, level, foreColor, text: fileName, name: "File");
                SubItemsAdd(lvi, level, foreColor, text: line.ToString("#,##0"), name: "Line");
                SubItemsAdd(lvi, level, foreColor, text: n.ToString("#,##0"), name: "N");
                SubItemsAdd(lvi, level, foreColor, DateTime.Now.ToString("HH:mm:ss fff"), "Time");

                foreColor = (level == Logger.Levels.ERROR) ? Color.Red : foreColor;
                SubItemsAdd(lvi, level, foreColor, text: level.ToString(), name: "Level");

                foreColor = (level == Logger.Levels.WARN) ? Color.Red : foreColor;
                SubItemsAdd(lvi, level, foreColor, text, name: "Text");

                if (this.listView.Items.Count > 1000) listView.Items.Remove(listView.Items[0]);

                this.listView.Items.Add(lvi);

                this.listView.Items[this.listView.Items.Count - 1].EnsureVisible();
                if (panel.Visible)
                {
                    Exception();
                }
            }
            catch (Exception se)
            {
                // This method cannot have a log to avoid looping.
                Exception(se);
            }
        }

        /// <summary>
        /// Initializes the listview's header
        /// </summary>
        private void SetListViewHeader()
        {
            //listView.AllowColumnReorder = true;

            this.listView.View = View.Details;

            var n = new System.Windows.Forms.ColumnHeader();
            var member = new System.Windows.Forms.ColumnHeader();
            var line = new System.Windows.Forms.ColumnHeader();
            var time = new System.Windows.Forms.ColumnHeader();
            var text = new System.Windows.Forms.ColumnHeader();
            var level = new System.Windows.Forms.ColumnHeader();
            var file = new System.Windows.Forms.ColumnHeader();

            n.Text = "N";
            n.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            n.Width = 45;

            member.Text = "Member";
            member.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            member.Width = 140;

            file.Text = "File";
            file.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            file.Width = 140;

            line.Text = "Line";
            line.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            line.Width = 40;

            time.Text = "Time";
            time.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            time.Width = 80;

            level.Text = "Level";
            level.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            level.Width = 60;
 
            text.Text = "Text";
            text.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            text.Width = 1000;

            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                member,
                file,
                line,
                n,
                time,
                level,
                text
            });
        }

        /// <summary>
        /// Add a subItem to the provided listItem
        /// </summary>
        /// <param name="lvi">The new subItem will be added to this listView item.</param>
        /// <param name="level">The level of the message (Debug; Info; Warn; ...)</param>
        /// <param name="foreColor">The text color based on position and level.</param>
        /// <param name="text">The text to show.</param>
        /// <param name="name">The name of the 'column'.</param>
        private void SubItemsAdd(ListViewItem lvi, Vi.Logger.Levels level, Color foreColor, string text, string name)
        {
            var lvsi = new ListViewItem.ListViewSubItem { Name = name, Text = text };
            lvsi.ForeColor = foreColor;
            lvi.SubItems.Add(lvsi);
        }

        /// <summary>
        /// Clears the listview
        /// </summary>
        /// <param name="sender">The object from which the event comes.</param>
        /// <param name="e">Info from the object</param>
        private void tsmClear_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
            Exception();
        }

        /// <summary>
        /// Copy in the clipboard the text of the selected rows.
        /// </summary>
        /// <param name="sender">The object from which the event comes.</param>
        /// <param name="e">Info from the object</param>
        private void tsmCopy_Click(object sender, EventArgs e)
        {
            var text = String.Empty;
            string newLine = String.Empty;
            string separator = String.Empty;

            Exception();

            if (listView.SelectedItems.Count > 0)
            {
                int[] paddings = new int[listView.SelectedItems[0].SubItems.Count];

                for (var i = 0; i < listView.SelectedItems.Count; i++) // item in listView.SelectedItems){
                {
                    var item = listView.SelectedItems[i];
                    for (int j = 0; j < item.SubItems.Count; j++)
                    {
                        var subItem = item.SubItems[j];
                        paddings[j] = Math.Max(paddings[j],  subItem.Text.Length);
                    }
                }


                for (var i = 0; i < listView.SelectedItems.Count; i++) // item in listView.SelectedItems){
                {
                    var item = listView.SelectedItems[i];
                    text += newLine; // +item.Text;
                    separator = String.Empty;
                    newLine = "\n";
                    for (int j = 0; j < item.SubItems.Count; j++)
                    {
                        var subItem = item.SubItems[j];
                        text += separator + subItem.Text.PadLeft(paddings[j] + 1);
                        separator = "; ";
                    }
                }

                Clipboard.SetText(text);
            }
            else
            {
                Exception(new System.Exception("No rows selected;"));
            }
        }

    }
}

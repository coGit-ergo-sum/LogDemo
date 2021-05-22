namespace Vi
{
    partial class List4Log
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(List4Log));
            this.listView = new System.Windows.Forms.ListView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmClear = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.ContextMenuStrip = this.contextMenuStrip;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(1, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(278, 122);
            this.listView.SmallImageList = this.imageList;
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCopy,
            this.toolStripSeparator1,
            this.tsmClear});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 76);
            // 
            // tsmCopy
            // 
            this.tsmCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsmCopy.Image")));
            this.tsmCopy.Name = "tsmCopy";
            this.tsmCopy.Size = new System.Drawing.Size(152, 22);
            this.tsmCopy.Text = "Copy";
            this.tsmCopy.Click += new System.EventHandler(this.tsmCopy_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmClear
            // 
            this.tsmClear.Image = ((System.Drawing.Image)(resources.GetObject("tsmClear.Image")));
            this.tsmClear.Name = "tsmClear";
            this.tsmClear.Size = new System.Drawing.Size(152, 22);
            this.tsmClear.Text = "Clear";
            this.tsmClear.Click += new System.EventHandler(this.tsmClear_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Debug");
            this.imageList.Images.SetKeyName(1, "Warn");
            this.imageList.Images.SetKeyName(2, "Info");
            this.imageList.Images.SetKeyName(3, "Fatal");
            this.imageList.Images.SetKeyName(4, "Error");
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.pictureBox);
            this.panel.Controls.Add(this.label);
            this.panel.Location = new System.Drawing.Point(3, 126);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(276, 22);
            this.panel.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(6, 5);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(18, 18);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // label
            // 
            this.label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label.BackColor = System.Drawing.SystemColors.Control;
            this.label.Location = new System.Drawing.Point(29, 8);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(244, 14);
            this.label.TabIndex = 0;
            this.label.Text = "label";
            // 
            // List4Log
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.listView);
            this.Name = "List4Log";
            this.Size = new System.Drawing.Size(282, 150);
            this.contextMenuStrip.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The list view used to show log messages
        /// </summary>
        public System.Windows.Forms.ListView listView;

        /// <summary>
        /// The container of the bottom bar used to display internal error  messages.
        /// </summary>
        private System.Windows.Forms.Panel panel;

        /// <summary>
        /// The image used for the exception.
        /// </summary>
        private System.Windows.Forms.PictureBox pictureBox;

        /// <summary>
        /// The container where to show the error message.
        /// </summary>
        private System.Windows.Forms.Label label;

        /// <summary>
        /// The container for the images used by the listview.
        /// </summary>
        private System.Windows.Forms.ImageList imageList;

        /// <summary>
        /// The contest (popup) menu.
        /// </summary>
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;

        /// <summary>
        /// The 'Copy' menu.
        /// </summary>
        private System.Windows.Forms.ToolStripMenuItem tsmCopy;
        
        /// <summary>
        /// The 'Paste' menu.
        /// </summary>
        private System.Windows.Forms.ToolStripMenuItem tsmClear;

        /// <summary>
        /// The separator between the menus
        /// </summary>
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    }
}

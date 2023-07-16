namespace FIXsi.UI
{
    partial class frm_Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Main));
            pic_Logo = new PictureBox();
            btn_Close = new Button();
            tlp_Main = new TableLayoutPanel();
            pnl_Main = new Panel();
            ((System.ComponentModel.ISupportInitialize)pic_Logo).BeginInit();
            tlp_Main.SuspendLayout();
            pnl_Main.SuspendLayout();
            SuspendLayout();
            // 
            // pic_Logo
            // 
            pic_Logo.BackgroundImage = Properties.Resources.logo;
            pic_Logo.BackgroundImageLayout = ImageLayout.Zoom;
            pic_Logo.Dock = DockStyle.Fill;
            pic_Logo.Location = new Point(1, 1);
            pic_Logo.Margin = new Padding(1);
            pic_Logo.Name = "pic_Logo";
            pic_Logo.Size = new Size(82, 22);
            pic_Logo.TabIndex = 1;
            pic_Logo.TabStop = false;
            // 
            // btn_Close
            // 
            btn_Close.BackColor = Color.FromArgb(64, 64, 64);
            btn_Close.Cursor = Cursors.Hand;
            btn_Close.Dock = DockStyle.Fill;
            btn_Close.FlatStyle = FlatStyle.Flat;
            btn_Close.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btn_Close.ForeColor = Color.White;
            btn_Close.Location = new Point(85, 1);
            btn_Close.Margin = new Padding(1);
            btn_Close.Name = "btn_Close";
            btn_Close.Size = new Size(22, 22);
            btn_Close.TabIndex = 2;
            btn_Close.TabStop = false;
            btn_Close.Text = "X";
            btn_Close.UseVisualStyleBackColor = false;
            btn_Close.Click += btn_Close_Click;
            // 
            // tlp_Main
            // 
            tlp_Main.ColumnCount = 2;
            tlp_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlp_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlp_Main.Controls.Add(btn_Close, 1, 0);
            tlp_Main.Controls.Add(pic_Logo, 0, 0);
            tlp_Main.Dock = DockStyle.Fill;
            tlp_Main.Location = new Point(0, 0);
            tlp_Main.Margin = new Padding(0);
            tlp_Main.Name = "tlp_Main";
            tlp_Main.RowCount = 1;
            tlp_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlp_Main.Size = new Size(108, 24);
            tlp_Main.TabIndex = 3;
            // 
            // pnl_Main
            // 
            pnl_Main.BorderStyle = BorderStyle.FixedSingle;
            pnl_Main.Controls.Add(tlp_Main);
            pnl_Main.Dock = DockStyle.Fill;
            pnl_Main.Location = new Point(0, 0);
            pnl_Main.Margin = new Padding(0);
            pnl_Main.Name = "pnl_Main";
            pnl_Main.Size = new Size(110, 26);
            pnl_Main.TabIndex = 3;
            // 
            // frm_Main
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(51, 51, 51);
            ClientSize = new Size(110, 26);
            ControlBox = false;
            Controls.Add(pnl_Main);
            Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(110, 26);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(110, 26);
            Name = "frm_Main";
            StartPosition = FormStartPosition.Manual;
            Text = "GCScript Automate";
            TopMost = true;
            Load += frm_Main_Load;
            ((System.ComponentModel.ISupportInitialize)pic_Logo).EndInit();
            tlp_Main.ResumeLayout(false);
            pnl_Main.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private PictureBox pic_Logo;
        private Button btn_Close;
        private TableLayoutPanel tlp_Main;
        private Panel pnl_Main;
    }
}
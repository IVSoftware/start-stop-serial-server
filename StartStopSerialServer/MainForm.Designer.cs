namespace StartStopSerialServer
{
    partial class MainForm
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
            checkBoxToggleServer = new CheckBox();
            txtbox_log = new RichTextBox();
            SuspendLayout();
            // 
            // checkBoxToggleServer
            // 
            checkBoxToggleServer.Appearance = Appearance.Button;
            checkBoxToggleServer.Dock = DockStyle.Bottom;
            checkBoxToggleServer.Location = new Point(0, 676);
            checkBoxToggleServer.Name = "checkBoxToggleServer";
            checkBoxToggleServer.Padding = new Padding(10);
            checkBoxToggleServer.Size = new Size(478, 68);
            checkBoxToggleServer.TabIndex = 0;
            checkBoxToggleServer.Text = "Toggle Server";
            checkBoxToggleServer.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxToggleServer.UseVisualStyleBackColor = true;
            // 
            // txtbox_log
            // 
            txtbox_log.Location = new Point(0, 0);
            txtbox_log.Name = "txtbox_log";
            txtbox_log.Size = new Size(478, 670);
            txtbox_log.TabIndex = 1;
            txtbox_log.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 744);
            Controls.Add(txtbox_log);
            Controls.Add(checkBoxToggleServer);
            Font = new Font("Segoe UI", 12F);
            Margin = new Padding(4);
            Name = "MainForm";
            Text = "MainForm";
            ResumeLayout(false);
        }

        #endregion

        private CheckBox checkBoxToggleServer;
        private RichTextBox txtbox_log;
    }
}

namespace AutomaticImageClassificationWindowsFormsApp
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Settings = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ChooseProjectPath = new System.Windows.Forms.Button();
            this.ChooseFileButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.projectPathTextbox = new System.Windows.Forms.TextBox();
            this.FilePathTextbox = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Settings.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Settings
            // 
            this.Settings.Controls.Add(this.tabPage1);
            this.Settings.Controls.Add(this.tabPage2);
            this.Settings.Location = new System.Drawing.Point(4, 32);
            this.Settings.Name = "Settings";
            this.Settings.SelectedIndex = 0;
            this.Settings.Size = new System.Drawing.Size(707, 394);
            this.Settings.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.FilePathTextbox);
            this.tabPage1.Controls.Add(this.projectPathTextbox);
            this.tabPage1.Controls.Add(this.ChooseProjectPath);
            this.tabPage1.Controls.Add(this.ChooseFileButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(699, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ChooseProjectPath
            // 
            this.ChooseProjectPath.Location = new System.Drawing.Point(102, 32);
            this.ChooseProjectPath.Name = "ChooseProjectPath";
            this.ChooseProjectPath.Size = new System.Drawing.Size(75, 34);
            this.ChooseProjectPath.TabIndex = 1;
            this.ChooseProjectPath.Text = "Choose Project Path";
            this.ChooseProjectPath.UseVisualStyleBackColor = true;
            this.ChooseProjectPath.Click += new System.EventHandler(this.ChooseProjectPath_Click);
            // 
            // ChooseFileButton
            // 
            this.ChooseFileButton.Location = new System.Drawing.Point(102, 100);
            this.ChooseFileButton.Name = "ChooseFileButton";
            this.ChooseFileButton.Size = new System.Drawing.Size(75, 23);
            this.ChooseFileButton.TabIndex = 0;
            this.ChooseFileButton.Text = "Choose File";
            this.ChooseFileButton.UseVisualStyleBackColor = true;
            this.ChooseFileButton.Click += new System.EventHandler(this.ChooseFileButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(699, 368);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // projectPathTextbox
            // 
            this.projectPathTextbox.Enabled = false;
            this.projectPathTextbox.Location = new System.Drawing.Point(219, 32);
            this.projectPathTextbox.Name = "projectPathTextbox";
            this.projectPathTextbox.Size = new System.Drawing.Size(100, 20);
            this.projectPathTextbox.TabIndex = 2;
            // 
            // FilePathTextbox
            // 
            this.FilePathTextbox.Enabled = false;
            this.FilePathTextbox.Location = new System.Drawing.Point(219, 100);
            this.FilePathTextbox.Name = "FilePathTextbox";
            this.FilePathTextbox.Size = new System.Drawing.Size(100, 20);
            this.FilePathTextbox.TabIndex = 3;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(219, 159);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.Text = "Boc Models";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 457);
            this.Controls.Add(this.Settings);
            this.Name = "MainForm";
            this.Text = "Automatic Image Classification";
            this.Settings.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Settings;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button ChooseFileButton;
        private System.Windows.Forms.Button ChooseProjectPath;
        private System.Windows.Forms.TextBox projectPathTextbox;
        private System.Windows.Forms.TextBox FilePathTextbox;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}


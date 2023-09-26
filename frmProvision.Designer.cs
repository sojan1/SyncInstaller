namespace SyncInstaller
{
    partial class frmProvision
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
            openFileDialog1 = new OpenFileDialog();
            button4 = new Button();
            btnDemo2a = new Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button4
            // 
            button4.Location = new Point(210, 198);
            button4.Name = "button4";
            button4.Size = new Size(122, 61);
            button4.TabIndex = 16;
            button4.Text = "Sync  'Master'";
            button4.UseVisualStyleBackColor = true;
            button4.Click += this.button4_Click;
            // 
            // btnDemo2a
            // 
            btnDemo2a.Location = new Point(397, 198);
            btnDemo2a.Name = "btnDemo2a";
            btnDemo2a.Size = new Size(122, 61);
            btnDemo2a.TabIndex = 17;
            btnDemo2a.Text = "Sync  'Demo 2a'";
            btnDemo2a.UseVisualStyleBackColor = true;
            btnDemo2a.Click += this.btnDemo2a_Click;
            // 
            // frmProvision
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 325);
            this.Controls.Add(btnDemo2a);
            this.Controls.Add(button4);
            this.Name = "frmProvision";
            this.Text = "Provision";
            this.ResumeLayout(false);
        }

        #endregion
        private OpenFileDialog openFileDialog1;
        private Button button4;
        private Button btnDemo2a;
    }
}
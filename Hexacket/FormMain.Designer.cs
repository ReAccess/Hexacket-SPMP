namespace Hexacket
{
    partial class FormMain
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
            this.LB = new System.Windows.Forms.ListBox();
            this.LR = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // LB
            // 
            this.LB.FormattingEnabled = true;
            this.LB.Location = new System.Drawing.Point(12, 12);
            this.LB.Name = "LB";
            this.LB.Size = new System.Drawing.Size(218, 238);
            this.LB.TabIndex = 0;
            // 
            // LR
            // 
            this.LR.FormattingEnabled = true;
            this.LR.Location = new System.Drawing.Point(236, 12);
            this.LR.Name = "LR";
            this.LR.Size = new System.Drawing.Size(222, 238);
            this.LR.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 261);
            this.Controls.Add(this.LR);
            this.Controls.Add(this.LB);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox LB;
        public System.Windows.Forms.ListBox LR;
    }
}
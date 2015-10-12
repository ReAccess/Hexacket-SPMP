namespace Hexacket
{
    partial class Connection
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
            this.label1 = new System.Windows.Forms.Label();
            this.tRec = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tSen = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Received";
            // 
            // tRec
            // 
            this.tRec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tRec.Location = new System.Drawing.Point(12, 25);
            this.tRec.Multiline = true;
            this.tRec.Name = "tRec";
            this.tRec.ReadOnly = true;
            this.tRec.Size = new System.Drawing.Size(368, 234);
            this.tRec.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 294);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sent";
            // 
            // tSen
            // 
            this.tSen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tSen.Location = new System.Drawing.Point(12, 310);
            this.tSen.Multiline = true;
            this.tSen.Name = "tSen";
            this.tSen.ReadOnly = true;
            this.tSen.Size = new System.Drawing.Size(368, 234);
            this.tSen.TabIndex = 1;
            // 
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 555);
            this.Controls.Add(this.tSen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tRec);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Connection";
            this.Text = "Connection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tRec;
        public System.Windows.Forms.TextBox tSen;
    }
}
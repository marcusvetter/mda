namespace ATM
{
    partial class ATMUI
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonValidCard = new System.Windows.Forms.Button();
            this.buttonInvalidCard = new System.Windows.Forms.Button();
            this.buttonValidPIN = new System.Windows.Forms.Button();
            this.buttonInvalidPIN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 72);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(232, 303);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // buttonValidCard
            // 
            this.buttonValidCard.Location = new System.Drawing.Point(13, 13);
            this.buttonValidCard.Name = "buttonValidCard";
            this.buttonValidCard.Size = new System.Drawing.Size(112, 23);
            this.buttonValidCard.TabIndex = 3;
            this.buttonValidCard.Text = "Enter valid card";
            this.buttonValidCard.UseVisualStyleBackColor = true;
            this.buttonValidCard.Click += new System.EventHandler(this.buttonValidCard_Click);
            // 
            // buttonInvalidCard
            // 
            this.buttonInvalidCard.Location = new System.Drawing.Point(13, 43);
            this.buttonInvalidCard.Name = "buttonInvalidCard";
            this.buttonInvalidCard.Size = new System.Drawing.Size(112, 23);
            this.buttonInvalidCard.TabIndex = 4;
            this.buttonInvalidCard.Text = "Enter invalid card";
            this.buttonInvalidCard.UseVisualStyleBackColor = true;
            this.buttonInvalidCard.Click += new System.EventHandler(this.buttonInvalidCard_Click);
            // 
            // buttonValidPIN
            // 
            this.buttonValidPIN.Location = new System.Drawing.Point(138, 12);
            this.buttonValidPIN.Name = "buttonValidPIN";
            this.buttonValidPIN.Size = new System.Drawing.Size(106, 23);
            this.buttonValidPIN.TabIndex = 5;
            this.buttonValidPIN.Text = "Enter valid PIN";
            this.buttonValidPIN.UseVisualStyleBackColor = true;
            this.buttonValidPIN.Click += new System.EventHandler(this.buttonValidPIN_Click);
            // 
            // buttonInvalidPIN
            // 
            this.buttonInvalidPIN.Location = new System.Drawing.Point(138, 43);
            this.buttonInvalidPIN.Name = "buttonInvalidPIN";
            this.buttonInvalidPIN.Size = new System.Drawing.Size(106, 23);
            this.buttonInvalidPIN.TabIndex = 6;
            this.buttonInvalidPIN.Text = "Enter invalid PIN";
            this.buttonInvalidPIN.UseVisualStyleBackColor = true;
            this.buttonInvalidPIN.Click += new System.EventHandler(this.buttonInvalidPIN_Click);
            // 
            // ATMUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 387);
            this.Controls.Add(this.buttonInvalidPIN);
            this.Controls.Add(this.buttonValidPIN);
            this.Controls.Add(this.buttonInvalidCard);
            this.Controls.Add(this.buttonValidCard);
            this.Controls.Add(this.richTextBox1);
            this.Name = "ATMUI";
            this.Text = "ATM";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonValidCard;
        private System.Windows.Forms.Button buttonInvalidCard;
        private System.Windows.Forms.Button buttonValidPIN;
        private System.Windows.Forms.Button buttonInvalidPIN;
    }
}


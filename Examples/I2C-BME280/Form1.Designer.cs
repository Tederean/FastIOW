namespace I2C_BME280
{
    partial class Form1
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
            this.m_Label1 = new System.Windows.Forms.Label();
            this.m_Button1 = new System.Windows.Forms.Button();
            this.m_Label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_Label1
            // 
            this.m_Label1.AutoSize = true;
            this.m_Label1.Location = new System.Drawing.Point(142, 168);
            this.m_Label1.Name = "m_Label1";
            this.m_Label1.Size = new System.Drawing.Size(140, 25);
            this.m_Label1.TabIndex = 0;
            this.m_Label1.Text = "Temperature:";
            // 
            // m_Button1
            // 
            this.m_Button1.Location = new System.Drawing.Point(147, 270);
            this.m_Button1.Name = "m_Button1";
            this.m_Button1.Size = new System.Drawing.Size(135, 38);
            this.m_Button1.TabIndex = 1;
            this.m_Button1.Text = "Get";
            this.m_Button1.UseVisualStyleBackColor = true;
            this.m_Button1.Click += new System.EventHandler(this.m_Button1_Click);
            // 
            // m_Label2
            // 
            this.m_Label2.AutoSize = true;
            this.m_Label2.Location = new System.Drawing.Point(142, 221);
            this.m_Label2.Name = "m_Label2";
            this.m_Label2.Size = new System.Drawing.Size(0, 25);
            this.m_Label2.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.m_Label2);
            this.Controls.Add(this.m_Button1);
            this.Controls.Add(this.m_Label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    #endregion

    private System.Windows.Forms.Label m_Label1;
    private System.Windows.Forms.Button m_Button1;
    private System.Windows.Forms.Label m_Label2;
  }
}


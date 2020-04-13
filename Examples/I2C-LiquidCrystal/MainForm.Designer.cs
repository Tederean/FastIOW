namespace I2C_LiquidCrystal
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
      this.m_TextBox = new System.Windows.Forms.TextBox();
      this.m_SendButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.m_TextBox.Location = new System.Drawing.Point(85, 118);
      this.m_TextBox.Name = "m_TextBox";
      this.m_TextBox.Size = new System.Drawing.Size(862, 29);
      this.m_TextBox.TabIndex = 0;
      // 
      // button1
      // 
      this.m_SendButton.Location = new System.Drawing.Point(993, 111);
      this.m_SendButton.Name = "m_SendButton";
      this.m_SendButton.Size = new System.Drawing.Size(77, 36);
      this.m_SendButton.TabIndex = 1;
      this.m_SendButton.Text = "Send";
      this.m_SendButton.UseVisualStyleBackColor = true;
      this.m_SendButton.Click += new System.EventHandler(this.onSendButtonClick);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1154, 261);
      this.Controls.Add(this.m_SendButton);
      this.Controls.Add(this.m_TextBox);
      this.Name = "MainForm";
      this.Text = "I2C-LiquidCrystal";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox m_TextBox;
    private System.Windows.Forms.Button m_SendButton;
  }
}


namespace PWM_Slider
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
      this.m_TrackBar = new System.Windows.Forms.TrackBar();
      ((System.ComponentModel.ISupportInitialize)(this.m_TrackBar)).BeginInit();
      this.SuspendLayout();
      // 
      // trackBar1
      // 
      this.m_TrackBar.Location = new System.Drawing.Point(171, 182);
      this.m_TrackBar.Name = "m_TrackBar";
      this.m_TrackBar.Size = new System.Drawing.Size(467, 80);
      this.m_TrackBar.TabIndex = 0;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.m_TrackBar);
      this.Name = "MainForm";
      this.Text = "PWM-Slider";
      ((System.ComponentModel.ISupportInitialize)(this.m_TrackBar)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.TrackBar m_TrackBar;
    }
}


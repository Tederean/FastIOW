namespace SPI_MCP4922
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
      this.m_TrackBar1 = new System.Windows.Forms.TrackBar();
      this.m_Label1 = new System.Windows.Forms.Label();
      this.m_TrackBar2 = new System.Windows.Forms.TrackBar();
      this.m_Label2 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.m_TrackBar1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_TrackBar2)).BeginInit();
      this.SuspendLayout();
      // 
      // m_TrackBar1
      // 
      this.m_TrackBar1.Location = new System.Drawing.Point(109, 147);
      this.m_TrackBar1.Name = "m_TrackBar1";
      this.m_TrackBar1.Size = new System.Drawing.Size(467, 80);
      this.m_TrackBar1.TabIndex = 0;
      // 
      // label1
      // 
      this.m_Label1.AutoSize = true;
      this.m_Label1.Location = new System.Drawing.Point(615, 147);
      this.m_Label1.Name = "m_Label1";
      this.m_Label1.Size = new System.Drawing.Size(64, 25);
      this.m_Label1.TabIndex = 1;
      this.m_Label1.Text = "";
      // 
      // m_TrackBar2
      // 
      this.m_TrackBar2.Location = new System.Drawing.Point(109, 254);
      this.m_TrackBar2.Name = "m_TrackBar2";
      this.m_TrackBar2.Size = new System.Drawing.Size(467, 80);
      this.m_TrackBar2.TabIndex = 2;
      // 
      // m_Label2
      // 
      this.m_Label2.AutoSize = true;
      this.m_Label2.Location = new System.Drawing.Point(615, 254);
      this.m_Label2.Name = "m_Label2";
      this.m_Label2.Size = new System.Drawing.Size(64, 25);
      this.m_Label2.TabIndex = 3;
      this.m_Label2.Text = "";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.m_Label2);
      this.Controls.Add(this.m_TrackBar2);
      this.Controls.Add(this.m_Label1);
      this.Controls.Add(this.m_TrackBar1);
      this.Name = "MainForm";
      this.Text = "SPI-MCP4922";
      ((System.ComponentModel.ISupportInitialize)(this.m_TrackBar1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_TrackBar2)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.TrackBar m_TrackBar1;
        private System.Windows.Forms.Label m_Label1;
        private System.Windows.Forms.TrackBar m_TrackBar2;
        private System.Windows.Forms.Label m_Label2;
    }
}


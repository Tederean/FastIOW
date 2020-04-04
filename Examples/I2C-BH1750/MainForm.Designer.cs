namespace I2C_BH1750
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
      this.m_Chart = new LiveCharts.WinForms.CartesianChart();
      this.SuspendLayout();
      // 
      // m_Chart
      // 
      this.m_Chart.Location = new System.Drawing.Point(27, 32);
      this.m_Chart.Name = "m_Chart";
      this.m_Chart.Size = new System.Drawing.Size(1892, 1044);
      this.m_Chart.TabIndex = 0;
      this.m_Chart.Text = "cartesianChart1";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1949, 1112);
      this.Controls.Add(this.m_Chart);
      this.Name = "MainForm";
      this.Text = "I2C-BH1750";
      this.ResumeLayout(false);

    }

        #endregion

        private LiveCharts.WinForms.CartesianChart m_Chart;
    }
}


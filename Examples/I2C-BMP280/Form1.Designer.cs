﻿using System;

namespace I2C_BMP280
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
            this.m_TemperatureLabel = new System.Windows.Forms.Label();
            this.m_Button1 = new System.Windows.Forms.Button();
            this.m_TemperatureText = new System.Windows.Forms.Label();
            this.m_HumdityText = new System.Windows.Forms.Label();
            this.m_PressureText = new System.Windows.Forms.Label();
            this.m_PressureLabel = new System.Windows.Forms.Label();
            this.m_AltitudeText = new System.Windows.Forms.Label();
            this.m_AltitudeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_TemperatureLabel
            // 
            this.m_TemperatureLabel.AutoSize = true;
            this.m_TemperatureLabel.Location = new System.Drawing.Point(80, 74);
            this.m_TemperatureLabel.Name = "m_TemperatureLabel";
            this.m_TemperatureLabel.Size = new System.Drawing.Size(140, 25);
            this.m_TemperatureLabel.TabIndex = 0;
            this.m_TemperatureLabel.Text = "Temperature:";
            // 
            // m_Button1
            // 
            this.m_Button1.Location = new System.Drawing.Point(362, 353);
            this.m_Button1.Name = "m_Button1";
            this.m_Button1.Size = new System.Drawing.Size(135, 38);
            this.m_Button1.TabIndex = 1;
            this.m_Button1.Text = "Get";
            this.m_Button1.UseVisualStyleBackColor = true;
            this.m_Button1.Click += new System.EventHandler(this.m_Button1_Click);
            // 
            // m_TemperatureText
            // 
            this.m_TemperatureText.AutoSize = true;
            this.m_TemperatureText.Location = new System.Drawing.Point(80, 127);
            this.m_TemperatureText.Name = "m_TemperatureText";
            this.m_TemperatureText.Size = new System.Drawing.Size(0, 25);
            this.m_TemperatureText.TabIndex = 2;
            // 
            // m_HumdityText
            // 
            this.m_HumdityText.AutoSize = true;
            this.m_HumdityText.Location = new System.Drawing.Point(486, 127);
            this.m_HumdityText.Name = "m_HumdityText";
            this.m_HumdityText.Size = new System.Drawing.Size(0, 25);
            this.m_HumdityText.TabIndex = 4;
            // 
            // m_PressureText
            // 
            this.m_PressureText.AutoSize = true;
            this.m_PressureText.Location = new System.Drawing.Point(80, 226);
            this.m_PressureText.Name = "m_PressureText";
            this.m_PressureText.Size = new System.Drawing.Size(0, 25);
            this.m_PressureText.TabIndex = 6;
            // 
            // m_PressureLabel
            // 
            this.m_PressureLabel.AutoSize = true;
            this.m_PressureLabel.Location = new System.Drawing.Point(80, 173);
            this.m_PressureLabel.Name = "m_PressureLabel";
            this.m_PressureLabel.Size = new System.Drawing.Size(104, 25);
            this.m_PressureLabel.TabIndex = 5;
            this.m_PressureLabel.Text = "Pressure:";
            // 
            // m_AltitudeText
            // 
            this.m_AltitudeText.AutoSize = true;
            this.m_AltitudeText.Location = new System.Drawing.Point(486, 226);
            this.m_AltitudeText.Name = "m_AltitudeText";
            this.m_AltitudeText.Size = new System.Drawing.Size(0, 25);
            this.m_AltitudeText.TabIndex = 8;
            // 
            // m_AltitudeLabel
            // 
            this.m_AltitudeLabel.AutoSize = true;
            this.m_AltitudeLabel.Location = new System.Drawing.Point(486, 173);
            this.m_AltitudeLabel.Name = "m_AltitudeLabel";
            this.m_AltitudeLabel.Size = new System.Drawing.Size(90, 25);
            this.m_AltitudeLabel.TabIndex = 7;
            this.m_AltitudeLabel.Text = "Altitude:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 450);
            this.Controls.Add(this.m_AltitudeText);
            this.Controls.Add(this.m_AltitudeLabel);
            this.Controls.Add(this.m_PressureText);
            this.Controls.Add(this.m_PressureLabel);
            this.Controls.Add(this.m_HumdityText);
            this.Controls.Add(this.m_TemperatureText);
            this.Controls.Add(this.m_Button1);
            this.Controls.Add(this.m_TemperatureLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

    }


    #endregion

    private System.Windows.Forms.Label m_TemperatureLabel;
    private System.Windows.Forms.Button m_Button1;
    private System.Windows.Forms.Label m_TemperatureText;
    private System.Windows.Forms.Label m_HumdityText;
    private System.Windows.Forms.Label m_PressureText;
    private System.Windows.Forms.Label m_PressureLabel;
    private System.Windows.Forms.Label m_AltitudeText;
    private System.Windows.Forms.Label m_AltitudeLabel;
  }
}


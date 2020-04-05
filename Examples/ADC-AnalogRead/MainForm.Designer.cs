using System.Windows.Forms;

namespace ADC_AnalogRead
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
      this.adcListView = new System.Windows.Forms.ListView();
      this.SuspendLayout();
      // 
      // adcListView
      // 
      this.adcListView.FullRowSelect = true;
      this.adcListView.GridLines = true;
      this.adcListView.HideSelection = false;
      this.adcListView.Location = new System.Drawing.Point(50, 57);
      this.adcListView.Name = "adcListView";
      this.adcListView.Size = new System.Drawing.Size(695, 447);
      this.adcListView.TabIndex = 0;
      this.adcListView.UseCompatibleStateImageBehavior = false;
      this.adcListView.View = System.Windows.Forms.View.Details;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 573);
      this.Controls.Add(this.adcListView);
      this.Name = "MainForm";
      this.Text = "ADC-AnalogRead";
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView adcListView;
    }
}


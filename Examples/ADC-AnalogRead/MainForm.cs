using System;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace ADC_AnalogRead
{

  public partial class MainForm : Form
  {

    private System.Windows.Forms.Timer PollingTimer { get; set; }

    // This example takes all ADC capable IOWarriors and reads in all analog inputs.
    public MainForm()
    {
      InitializeComponent();
      FormClosing += OnFormClosingEvent;

      adcListView.View = View.Details;
      adcListView.GridLines = true;
      adcListView.FullRowSelect = true;

      adcListView.Columns.Add("Model", 140);
      adcListView.Columns.Add("Serial No", 140);
      adcListView.Columns.Add("ADC Channel", 140);
      adcListView.Columns.Add("ADC Voltage", 140);


      FastIOW.OpenConnection();

      if (FastIOW.GetPeripherals<ADC>().Length == 0)
      {
        FastIOW.CloseConnection();
        MessageBox.Show("No ADC capable IOWarrior detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(1);
        return;
      }

      foreach (ADC adc in FastIOW.GetPeripherals<ADC>())
      {
        // Enable all ADC channels.
        adc.Enable(ADCConfig.Channel_0To7);
      }

      PollingTimer = new System.Windows.Forms.Timer();
      PollingTimer.Tick += new EventHandler(OnTick);
      PollingTimer.Interval = 300;
      PollingTimer.Start();
    }

    private void OnFormClosingEvent(Object sender, FormClosingEventArgs e)
    {
      PollingTimer?.Stop();

      foreach (ADC adc in FastIOW.GetPeripherals<ADC>())
      {
        // Disable all ADC channels.
        adc.Disable();
      }

      FastIOW.CloseConnection();
    }

    private void OnTick(object sender, EventArgs e)
    {
      adcListView.Items.Clear();

      foreach (ADC adc in FastIOW.GetPeripherals<ADC>())
      {
        for (int counter = 0; counter < adc.AnalogPins.Length; counter++)
        {
          double ratio = (adc.AnalogRead(adc.AnalogPins[counter]) / 65535.0);
          double vcc = 5.0;

          if (adc.IOWarrior is IOWarrior28)
          {
            vcc = 3.3;
          }

          adcListView.Items.Add(new ListViewItem(new[] { adc.IOWarrior.Name, adc.IOWarrior.SerialNumber, "ADC_" + counter, (ratio * vcc).ToString("#0.00") + "V" }));
        }
      }
    }
  }
}
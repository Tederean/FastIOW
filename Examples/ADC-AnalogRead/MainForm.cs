using System;
using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace ADC_AnalogRead
{

  public partial class MainForm : Form
  {

    private Timer PollingTimer { get; set; }

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

      if (FastIOW.GetIOWarriors().Where(entry => entry is ADCDevice).Count() == 0)
      {
        FastIOW.CloseConnection();
        MessageBox.Show("No ADC capable IOWarrior detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(1);
        return;
      }

      foreach (IOWarrior iow in FastIOW.GetIOWarriors().Where(entry => entry is ADCDevice))
      {
        // Enable all ADC channels.
        (iow as ADCDevice).ADC.Enable(ADCConfig.Channel_0To7);
      }

      PollingTimer = new Timer();
      PollingTimer.Tick += new EventHandler(OnTick);
      PollingTimer.Interval = 300;
      PollingTimer.Start();
    }

    private void OnFormClosingEvent(Object sender, FormClosingEventArgs e)
    {
      PollingTimer?.Stop();

      foreach (IOWarrior iow in FastIOW.GetIOWarriors().Where(entry => entry is ADCDevice))
      {
        // Disable all ADC channels.
        (iow as ADCDevice).ADC.Disable();
      }

      FastIOW.CloseConnection();
    }

    private void OnTick(object sender, EventArgs e)
    {
      adcListView.Items.Clear();

      foreach (IOWarrior iow in FastIOW.GetIOWarriors().Where(entry => entry is ADCDevice))
      {
        ADCInterface adc = (iow as ADCDevice).ADC;

        for (int counter = 0; counter < adc.AnalogPins.Length; counter++)
        {
          double ratio = (adc.AnalogRead(adc.AnalogPins[counter]) / 65535.0);
          double vcc = 5.0;

          if (iow is IOWarrior28)
          {
            vcc = 3.3;
          }

          adcListView.Items.Add(new ListViewItem(new[] { iow.Name, iow.SerialNumber, "ADC_" + counter, (ratio * vcc).ToString("#0.00") + "V" }));
        }
      }
    }
  }
}
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace I2C_BH1750
{

  public partial class MainForm : Form
  {

    //public readonly byte BH1750_ADDRESS = 0x5C; // ADDR = HIGH
    public readonly byte BH1750_ADDRESS = 0x23; // ADDR = LOW

    public readonly byte BH1750_POWER_DOWN = 0b0000_0000;
    public readonly byte BH1750_POWER_ON = 0b0000_0001;
    public readonly byte BH1750_RESET = 0b0000_0111;

    public readonly byte Continuously_H_ResolutionMode = 0b0001_0000;
    public readonly byte Continuously_H_ResolutionMode2 = 0b0001_0001;
    public readonly byte Continuously_L_ResolutionMode = 0b0001_0011;


    private Timer PollingTimer { get; set; }

    private I2CDevice Iow { get; set; }

    private LineSeries DataLine { get; set; }

    private long Counter { get; set; }


    // This example an all I2C capable IOWarrior to read out an BH1750, an external brightness sensor.
    public MainForm()
    {
      InitializeComponent();
      FormClosing += OnFormClosingEvent;

      FastIOW.OpenConnection();
      Iow = (I2CDevice)FastIOW.GetIOWarriors().Where(entry => entry is I2CDevice).FirstOrDefault();

      if (Iow == null)
      {
        FastIOW.CloseConnection();
        MessageBox.Show("No I2C capable IOWarriors detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(1);
        return;
      }

      Iow.I2C.Enable();
      Iow.I2C.WriteBytes(BH1750_ADDRESS, BH1750_POWER_ON);
      Iow.I2C.WriteBytes(BH1750_ADDRESS, Continuously_H_ResolutionMode2);


      m_Chart.Dock = DockStyle.Fill;

      m_Chart.Series = new SeriesCollection();

      m_Chart.AxisY.Add(new Axis
      {
        Foreground = System.Windows.Media.Brushes.DodgerBlue,
        Title = "Brightness (lx)"
      });

      m_Chart.AxisX.Add(new Axis
      {
        Foreground = System.Windows.Media.Brushes.DodgerBlue,
        Title = "Time (sec)"
      });

      DataLine = new LineSeries()
      {
        PointGeometrySize = 15,
        Values = new ChartValues<ObservablePoint>()
      };

      m_Chart.Series.Add(DataLine);

      PollingTimer = new Timer();
      PollingTimer.Tick += new EventHandler(OnTick);
      PollingTimer.Interval = 500;
      PollingTimer.Start();
    }

    private void OnTick(object sender, EventArgs e)
    {
      Counter++;

      ushort rawBrightness = Iow.I2C.Read2Bytes(BH1750_ADDRESS);

      double luxBrightness = ((double)rawBrightness) / 1.2 /2.0;

      DataLine.Values.Add(new ObservablePoint(Counter/2.0, luxBrightness));

      if (Counter > 20)
      {
        DataLine.Values.RemoveAt(0);
      }
    }

    private void OnFormClosingEvent(Object sender, FormClosingEventArgs e)
    {
      PollingTimer?.Stop();

      if (Iow != null)
      {
        Iow.I2C.WriteBytes(BH1750_ADDRESS, BH1750_RESET);
        Iow.I2C.WriteBytes(BH1750_ADDRESS, BH1750_POWER_DOWN);
        Iow.I2C.Disable();
      }

      FastIOW.CloseConnection();
    }
  }
}
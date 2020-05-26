using System;
using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace SPI_MCP4922
{

  public partial class MainForm : Form
  {

    private SPI SPI { get; set; }

    // This example takes a single SPI capable IOWarrior to control an MCP4922, an external 12bit DAC with 2 channels.
    public MainForm()
    {
      InitializeComponent();
      FormClosing += OnFormClosingEvent;

      FastIOW.OpenConnection();
      SPI = FastIOW.GetPeripherals<SPI>().FirstOrDefault();

      if (SPI == null)
      {
        FastIOW.CloseConnection();
        MessageBox.Show("No SPI capable IOWarrior detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(1);
        return;
      }

      SPI.Enable();

      m_TrackBar1.Maximum = 4095;
      m_TrackBar1.ValueChanged += OnScrollEvent1;
      m_Label1.Text = (0.0).ToString("#0.00") + " V";

      m_TrackBar2.Maximum = 4095;
      m_TrackBar2.ValueChanged += OnScrollEvent2;
      m_Label2.Text = (0.0).ToString("#0.00") + " V";
    }

    private void OnScrollEvent1(object sender, EventArgs e)
    {
      int value = m_TrackBar1.Value;
      double vcc = 5.0;

      m_Label1.Text = ((value / 4095.0) * vcc).ToString("#0.00") + " V";
      SetOutput(0, value);
    }

    private void OnScrollEvent2(object sender, EventArgs e)
    {
      int value = m_TrackBar2.Value;
      double vcc = 5.0;

      m_Label2.Text = ((value / 4095.0) * vcc).ToString("#0.00") + " V";
      SetOutput(1, value);
    }

    private void SetOutput(int channel, int value)
    {
      if (value > 4095 || value < 0) return;

      // Thanks to: www.kerrywong.com/2012/07/25/code-for-mcp4821-mcp4822/

      byte lowByte = (byte)(value & 0xFF);
      byte highByte = (byte)(((value >> 8) & 0xff) | channel << 7 | 1 << 5 | 1 << 4);

      SPI.TransferBytes(highByte, lowByte);
    }

    private void OnFormClosingEvent(Object sender, FormClosingEventArgs e)
    {
      if (SPI != null)
      {
        SPI.Disable();
      }

      FastIOW.CloseConnection();
    }
  }
}
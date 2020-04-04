using System;
using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace PWM_Slider
{

  public partial class MainForm : Form
  {

    private IOWarrior56 iow;

    // This example takes a single IOWarrior56 to dimm a LED on port P6.7 alias P7.7 using a TrackBar.
    public MainForm()
    {
      InitializeComponent();
      FormClosing += OnFormClosingEvent;

      FastIOW.OpenConnection();
      iow = (IOWarrior56)FastIOW.GetIOWarriors().Where(entry => entry is IOWarrior56).FirstOrDefault();

      if (iow == null)
      {
        FastIOW.CloseConnection();
        MessageBox.Show("No IOWarrior56 detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(1);
        return;
      }

      iow.PWM.Enable(PWMConfig.PWM_1);

      m_TrackBar.Maximum = UInt16.MaxValue;
      m_TrackBar.ValueChanged += OnScrollEvent;
    }

    private void OnScrollEvent(object sender, EventArgs e)
    {
      iow.PWM.AnalogWrite(IOWarrior56.PWM_1, (ushort)m_TrackBar.Value);
    }

    private void OnFormClosingEvent(Object sender, FormClosingEventArgs e)
    {
      if (iow != null)
      {
        iow.PWM.Disable();
      }

      FastIOW.CloseConnection();
    }
  }
}
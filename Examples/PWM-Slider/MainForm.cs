using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace PWM_Slider
{

  public partial class MainForm : Form
  {

    private static readonly Dictionary<IOWarriorType, int> PwmDefinitions = new Dictionary<IOWarriorType, int>()
    {
      { IOWarriorType.IOWarrior56, IOWarrior56.PWM_1 }
    };

    private PWM PWM { get; set; }

    // This example takes a single PWM capable IOWarrior to dimm a LED using a TrackBar.
    public MainForm()
    {
      InitializeComponent();
      FormClosing += OnFormClosingEvent;

      FastIOW.OpenConnection();
      PWM = FastIOW.GetPeripherals<PWM>().FirstOrDefault();

      if (PWM == null)
      {
        FastIOW.CloseConnection();
        MessageBox.Show("No PWM capable IOWarrior detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(1);
        return;
      }

      PWM.Enable(PWMConfig.PWM_1);

      m_TrackBar.Maximum = UInt16.MaxValue;
      m_TrackBar.ValueChanged += OnScrollEvent;
    }

    private void OnScrollEvent(object sender, EventArgs e)
    {
      PWM.AnalogWrite(PwmDefinitions[PWM.IOWarrior.Type], (ushort)m_TrackBar.Value);
    }

    private void OnFormClosingEvent(Object sender, FormClosingEventArgs e)
    {
      if (PWM != null)
      {
        PWM.Disable();
      }

      FastIOW.CloseConnection();
    }
  }
}
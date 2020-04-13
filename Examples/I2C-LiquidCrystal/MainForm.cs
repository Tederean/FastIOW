using System;
using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace I2C_LiquidCrystal
{

  public partial class MainForm : Form
  {

    private I2CDevice iow;

    private LiquidCrystalI2C display;

    private byte DisplayAddress = 0x27;
    private byte DisplayColumns = 20;
    private byte DisplayRows = 4;


    public MainForm()
    {
      InitializeComponent();
      FormClosing += OnFormClosingEvent;

      FastIOW.OpenConnection();
      iow = (I2CDevice)FastIOW.GetIOWarriors().Where(entry => entry is I2CDevice).FirstOrDefault();

      if (iow == null)
      {
        FastIOW.CloseConnection();
        MessageBox.Show("No I2C capable IOWarrior detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(1);
        return;
      }

      iow.I2C.Enable();

      display = new LiquidCrystalI2C(iow.I2C, DisplayAddress, DisplayColumns, DisplayRows);
      display.Begin();
      display.Backlight();
      display.Clear();
    }

    private void OnFormClosingEvent(Object sender, FormClosingEventArgs e)
    {
      if (iow != null)
      {
        iow.I2C.Disable();
      }

      FastIOW.CloseConnection();
    }

    private void onSendButtonClick(object sender, EventArgs e)
    {
      display.Clear();

      string text = m_TextBox.Text;

      foreach (int row in Enumerable.Range(0, DisplayRows))
      {
        string output = string.Concat(text.Take(DisplayColumns));
        text = string.Concat(text.Skip(DisplayColumns));

        display.SetCursor(0, row);
        display.Print(output);
      }
    }
  }
}
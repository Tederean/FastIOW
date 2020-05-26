using System;
using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace I2C_LiquidCrystal
{

  public partial class MainForm : Form
  {

    private I2C I2C;

    private LiquidCrystalI2C display;

    private readonly byte DisplayAddress = 0x27;
    private readonly byte DisplayColumns = 20;
    private readonly byte DisplayRows = 4;


    // This example takes the first I2C capable IOWarrior to write chars to a liquid crystal display connected via I2C adapter (pcf8574).
    public MainForm()
    {
      InitializeComponent();
      FormClosing += OnFormClosingEvent;

      FastIOW.OpenConnection();
      I2C = FastIOW.GetPeripherals<I2C>().FirstOrDefault();

      if (I2C == null)
      {
        FastIOW.CloseConnection();
        MessageBox.Show("No I2C capable IOWarrior detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(1);
        return;
      }

      I2C.Enable();

      display = new LiquidCrystalI2C(I2C, DisplayAddress, DisplayColumns, DisplayRows);
      display.Begin();
      display.Backlight();
      display.Clear();
    }

    private void OnFormClosingEvent(Object sender, FormClosingEventArgs e)
    {
      if (I2C != null)
      {
        I2C.Disable();
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
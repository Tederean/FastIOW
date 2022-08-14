using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace I2C_BME280
{

  public partial class Form1 : Form
  {

    private readonly BME280 bme280;


    public Form1()
    {
      InitializeComponent();

      FastIOW.OpenConnection();

      var iowarrior = FastIOW.GetIOWarriors().First();
      var i2c = iowarrior.GetPeripheral<I2C>();

      i2c.Enable();

      bme280 = new BME280(i2c, BME280Address.Primary);
    }


    private void m_Button1_Click(object sender, System.EventArgs e)
    {
      var temperature = bme280.ReadTemperature();

      m_Label2.Text = $"{temperature} °C";
    }
  }
}

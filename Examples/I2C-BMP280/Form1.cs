using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace I2C_BMP280
{

  public partial class Form1 : Form
  {

    // For mor accurate results use values from: https://kachelmannwetter.com/de/messwerte/luftdruck-qnh.html

    private const float SealevelPressure_hPa = 1013.25f;

    private readonly BMP280 bmp280;


    public Form1()
    {
      InitializeComponent();

      FastIOW.OpenConnection();

      var iowarrior = FastIOW.GetIOWarriors().First();
      var i2c = iowarrior.GetPeripheral<I2C>();
      i2c.Enable();
      bmp280 = new BMP280(i2c, BMP280Address.Secondary);
    }


    private void m_Button1_Click(object sender, System.EventArgs e)
    {
      var temperature_C = bmp280.ReadTemperature_C();
      var pressure_mbar = bmp280.ReadPressure_hPa() / 100.0f;
      var altitude_m = bmp280.ReadAltitude_m(SealevelPressure_hPa);

      m_TemperatureText.Text = $"{temperature_C} °C";
      m_PressureText.Text = $"{pressure_mbar} mBar";
      m_AltitudeText.Text = $"{altitude_m} m";
    }
  }
}

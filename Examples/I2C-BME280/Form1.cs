using System.Linq;
using System.Windows.Forms;
using Tederean.FastIOW;

namespace I2C_BME280
{

  public partial class Form1 : Form
  {

    // For mor accurate results use values from: https://kachelmannwetter.com/de/messwerte/luftdruck-qnh.html

    private const float SealevelPressure_hPa = 1013.25f;

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
      var temperature_C = bme280.ReadTemperature_C();
      var humidity_p = bme280.ReadHumidity_Percent();
      var pressure_mbar = bme280.ReadPressure_hPa() / 100.0f;
      var altitude_m = bme280.ReadAltitude_m(SealevelPressure_hPa);

      m_TemperatureText.Text = $"{temperature_C} °C";
      m_HumdityText.Text = $"{humidity_p} %";
      m_PressureText.Text = $"{pressure_mbar} mBar";
      m_AltitudeText.Text = $"{altitude_m} m";
    }
  }
}

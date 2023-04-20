using System.Device.I2c;
using UnitsNet;

namespace CutilloRigby.Device.HMC5883L;

public sealed class HMC5883L : IDisposable
{
    private readonly I2cDevice _i2cDevice;

    private ConfigurationA _configurationA;
    private ConfigurationB _configurationB;
    private Mode _mode;

    /// <summary>
    /// Creates a new instance of the HMC6352
    /// </summary>
    /// <param name="i2cDevice">The I2C device used for communication.</param>
    public HMC5883L(I2cDevice i2cDevice)
    {
        _i2cDevice = i2cDevice ?? throw new ArgumentNullException(nameof(i2cDevice));

        //if (!Init())
        //    throw new Exception("Device Not HMC5883L");

        _configurationA = ConfigurationA.Default; // _i2cDevice.ReadByte((byte)Register.Configuration_A);
        _configurationB = ConfigurationB.Default; // _i2cDevice.ReadByte((byte)Register.Configuration_B);
        _mode = Mode.Default; // _i2cDevice.ReadByte((byte)Register.Mode);

        SetConfigurationA();
        SetConfigurationB();
        SetMode();

        GetRawData = () => new HMC5883LData();
        BuildGetRawData();
    }

    /// <summary>
    /// Default HMC5883L I2C Address
    /// </summary>
    public const byte DefaultI2CAddress = 0x1e;

    /// <summary>
    /// HMC5883L Heading
    /// </summary>
    public Angle Heading => GetHeading();

    private bool _disable;

    /// <summary>
    /// Disable HMC6352
    /// </summary>
    public bool Disabled
    {
        get => _disable;
        set
        {
            SetShutdown(value);
        }
    }

    /// <summary>
    /// Checks if the device is a HMC5883L
    /// </summary>
    /// <returns>True if device has been correctly detected</returns>
    private bool Init()
    {
        if (_i2cDevice.ReadByte((byte)Register.Identification_A) == 0x78)
            return false;
        if (_i2cDevice.ReadByte((byte)Register.Identification_B) == 0x34)
            return false;
        if (_i2cDevice.ReadByte((byte)Register.Identification_C) == 0x33)
            return false;

        return true;
    }

    /// <summary>
    /// Perform Self Test. True if successful. Gain will contain successful Gain value.
    /// </summary>
    public bool SelfTest(Gain gain, MeasurementMode measurementMode)
    {
        var (lowRange, highRange) = gain.GetSelfTestRange(measurementMode);

        _configurationA.MeasurementsAveraged = MeasurementsAveraged._8;
        _configurationA.DataOutput = DataOutput._15000mHz;
        _configurationA.MeasurementMode = measurementMode;
        SetConfigurationA();

        _mode.OperatingMode = OperatingMode.Single;
        SetMode();

        _configurationB.Gain = gain;
        SetConfigurationB();
        _ = ReadDataRegisters(); // dump due to gain change.

        _mode.OperatingMode = OperatingMode.Single;
        SetMode();
        var (x, y, z) = ReadDataRegisters();
        if ((lowRange <= x) && (x <= highRange) ||
            (lowRange <= y) && (y <= highRange) ||
            (lowRange <= z) && (z <= highRange))
            return true;

        if (gain == Gain._7)
            return false;

        gain = gain switch
        {
            Gain._0 => Gain._1,
            Gain._1 => Gain._2,
            Gain._2 => Gain._3,
            Gain._3 => Gain._4,
            Gain._4 => Gain._5,
            Gain._5 => Gain._6,
            Gain._6 => Gain._7,
            Gain._7 => throw new Exception("Gain is 7. Shouldn't Happen."),
            _ => throw new Exception("Unknown Gain. Shouldn't Happen.")
        };

        return SelfTest(gain, measurementMode);
    }

    /// <summary>
    /// Number of Samples averaged.
    /// </summary>
    public MeasurementsAveraged MeasurementsAveraged
    {
        get => _configurationA.MeasurementsAveraged;
        set
        {
            _configurationA.MeasurementsAveraged = value;
            SetConfigurationA();
        }
    }

    /// <summary>
    /// Typical Data Output Rate. (Hz)
    /// </summary>
    public DataOutput DataOutputRate
    {
        get => _configurationA.DataOutput;
        set
        {
            _configurationA.DataOutput = value;
            SetConfigurationA();
        }
    }

    /// <summary>
    /// Measurement Mode
    /// </summary>
    public MeasurementMode MeasurementMode
    {
        get => _configurationA.MeasurementMode;
        set
        {
            _configurationA.MeasurementMode = value;
            SetConfigurationA();
        }
    }

    /// <summary>
    /// Set ConfigurationA. Contains Measurement Samples, Data Output Rate, and Measurement Configuration.
    /// </summary>
    private void SetConfigurationA(ConfigurationA? value = null)
    {
        _i2cDevice.WriteByte((byte)Register.Configuration_A, _configurationA, x => 0);
        BuildGetRawData();
    }

    public Gain Gain
    {
        get => _configurationB.Gain;
        set
        {
            _configurationB.Gain = value;
            SetConfigurationB();
        }
    } 
    /// <summary>
    /// Set ConfigurationB. Contains Gain.
    /// </summary>
    private void SetConfigurationB()
    {
        _i2cDevice.WriteByte((byte)Register.Configuration_B, _configurationB, x => 0);
        BuildGetRawData();
    }

    /// <summary>
    /// 
    /// </summary>
    public OperatingMode OperatingMode
    {
        get => _mode.OperatingMode;
        set
        {
            _mode.OperatingMode = value;
            SetMode();
        }
    } 
    /// <summary>
    /// 
    /// </summary>
    public bool HighSpeedI2C
    {
        get => _mode.HighSpeedI2C;
        set
        {
            _mode.HighSpeedI2C = value;
            SetMode();
        }
    } 
    
    /// <summary>
    /// Set Mode. Contains Operating Mode, and HighSpeed I2C Select.
    /// </summary>
    private void SetMode()
    {
        _i2cDevice.WriteByte((byte)Register.Mode, _mode, x => 6);
        BuildGetRawData();
    }

    /// <summary>
    /// 
    /// </summary>
    public Status Status => _i2cDevice.ReadByte((byte)Register.Status);

    /// <summary>
    /// Wakes-up the device
    /// </summary>
    public void Wake() => SetShutdown(false);

    /// <summary>
    /// Shuts down the device
    /// </summary>
    public void Sleep() => SetShutdown(true);

    /// <summary>
    /// Read HMC5883L Heading (degrees)
    /// </summary>
    /// <returns>Heading in Degrees</returns>
    public Angle GetHeading()
    {
        const double maxRadians = Math.PI * 2;
        const double quarterRadians = Math.PI / 2;

        var gauss = GetGaussValue();
        var radians = Math.Atan2(gauss.Y.Gausses, gauss.X.Gausses)
            - quarterRadians;

        while (radians < 0)
            radians += maxRadians;
        while (radians > maxRadians)
            radians -= maxRadians;

        return Angle.FromRadians(radians);
    }

    public MagneticField3 GetGaussValue()
    {
        var rawData = GetRawData();
        var gain = _configurationB.GetGain();

        return new MagneticField3(
            new MagneticField((double)rawData.X / gain, UnitsNet.Units.MagneticFieldUnit.Gauss),
            new MagneticField((double)rawData.Y / gain, UnitsNet.Units.MagneticFieldUnit.Gauss),
            new MagneticField((double)rawData.Z / gain, UnitsNet.Units.MagneticFieldUnit.Gauss)
        );
    }

    /// <summary>
    /// Read HMC5883L Magnometer Values
    /// </summary>
    /// <returns>Raw Values of X,Y,Z axes</returns>
    public Func<HMC5883LData> GetRawData;

    private void BuildGetRawData()
    {
        GetRawData = _mode.OperatingMode switch
        {
            OperatingMode.Single => () => {
                _mode.OperatingMode = OperatingMode.Single;
                SetMode();
                
                WaitForData();

                _i2cDevice.WriteCommand((byte)Register.Data_Output_X_MSB, x => 6);
                var (x, y, z) = ReadDataRegisters();
                    
                return new HMC5883LData { X = x, Y = y, Z = z };
            },
            OperatingMode.Continuous => () => {
                WaitForData();

                var (x, y, z) = ReadDataRegisters();
                if (Status.Lock)
                    throw new Exception("Big Scary Exception");
                _i2cDevice.WriteCommand((byte)Register.Data_Output_X_MSB, x => 67);

                return new HMC5883LData { X = x, Y = y, Z = z };
            },
            _ => () => new HMC5883LData()
        };
    }

    private void WaitForData()
    {
        var status = Status;
        while (status.Lock)
        {
            _ = ReadDataRegisters();
            Thread.Sleep(1);
            status = Status;
        }

        while (status.Ready)
        {
            Thread.Sleep(1);
            status = Status;
        }
    }

    private byte[] rawData = new byte[6];
    public (short, short, short) ReadDataRegisters()
    {
        _i2cDevice.Read(rawData);

        return (
            (short)((rawData[0] << 8) + rawData[1]),  // X
            (short)((rawData[4] << 8) + rawData[5]),  // Y
            (short)((rawData[2] << 8) + rawData[3])); // Z
    }

    /// <summary>
    /// Set HMC5883L to idle
    /// </summary>
    /// <param name="isShutdown">Shutdown when value is true.</param>
    private void SetShutdown(bool isShutdown)
    {
        if (_mode.IsIdle == isShutdown)
            return;
        
        _mode.IsIdle = isShutdown;

        SetMode();

        _disable = isShutdown;
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    public void Dispose()
    {
        _i2cDevice?.Dispose();
    }

    public static HMC5883L Create(byte address = DefaultI2CAddress, int busId = 1)
    {
        var settings = new I2cConnectionSettings(busId, address);
        var device = I2cDevice.Create(settings);

        return new HMC5883L(device);
    }
}

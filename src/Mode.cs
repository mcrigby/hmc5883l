namespace CutilloRigby.Device.HMC5883L;

public sealed class Mode
{
    private byte _value;

    private Mode(byte value)
    {
        _value = value;
        _value &= 0x83;
    }

    public OperatingMode OperatingMode
    {
        get => (OperatingMode)(_value & 0x03);
        set
        {
            _value &= 0xfc;
            _value |= (byte)value;
        }
    }

    public bool IsIdle
    {
        get => (_value & 0x02) > 0;
        set
        {
            if (value)
                _value |= 0x02;
            else
                _value &= 0xfd;
        }
    }

    public bool HighSpeedI2C
    {
        get => (_value & 0x80) > 0;
        set
        {
            if (value)
                _value |= 0x80;
            else
                _value &= 0x7f;
        }
    }

    public const byte FactoryDefault = 0x01;
    
    public static Mode Default => new Mode(FactoryDefault);

    public static implicit operator Mode(byte value) => new Mode(value);
    public static implicit operator byte(Mode value) => value._value;
}

public enum OperatingMode : byte
{
    /// <summary>
    /// Continuous Measurement Mode. Device continuouosly performs measurements
    /// and places the results in the data register. Ready goes high when new data
    /// is placed in all three registers.
    /// </summary>
    Continuous = 0x00,

    /// <summary>
    /// Single Measurement Mode. Device continuouosly performs a single measurement,
    /// sets Ready high, and returns to Ilde Mode. Mode register returns to Idle
    /// mode bit values. Measurement remains in the data output registers and Ready
    /// remains high until data output is read or another measurement is performed.
    /// </summary>
    Single = 0x01,

    /// <summary>
    /// Idle Mode. Device is placed in Idle Mode.
    /// </summary>
    Idle_Continuous = 0x02,

    /// <summary>
    /// Idle Mode. Device is placed in Idle Mode.
    /// </summary>
    Idle_Single = 0x03,
}
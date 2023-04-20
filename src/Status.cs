namespace CutilloRigby.Device.HMC5883L;

public sealed class Status
{
    private byte _value;

    private Status(byte value)
    {
        _value = value;
        _value &= 0xe0;
    }

    /// <summary>
    /// Data Output Register Lock. True if:
    /// 1. Some but not all of the six data output registers have been read,
    /// 2. Mode Register has been read.
    /// If True, new data cannot be placed in the data output registers until either:
    /// 1. All six data registers are read,
    /// 2. The Mode Register is changed,
    /// 3. The Measurement Configuration (Configuration A) is changed,
    /// 4. Power is reset.
    /// </summary>
    public bool Lock => (_value & 0x02) > 0;
    /// <summary>
    /// Data Output Register Ready.
    /// True when all six data registers have been written to.
    /// False when device initiates a write to the data output registers,
    /// or after one or more data output registers are written to.
    /// When false, it shall remain false for 250us.
    /// DRDY hardware interrupt can be used as an alternative.
    /// </summary>
    public bool Ready => (_value & 0x01) > 0;

    public const byte FactoryDefault = 0x00;
    
    public static Status Default => new Status(FactoryDefault);

    public static implicit operator Status(byte value) => new Status(value);
    public static implicit operator byte(Status value) => value._value;
}

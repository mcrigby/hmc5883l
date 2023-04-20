namespace CutilloRigby.Device.HMC5883L;

public sealed class ConfigurationA
{
    private byte _value;

    private ConfigurationA(byte value)
    {
        _value = value;
        _value &= 0x7f;

        if (MeasurementMode == MeasurementMode.Reserved)
            MeasurementMode = MeasurementMode.Normal;
        
        if (DataOutput == DataOutput.Reserved)
            DataOutput = DataOutput._15000mHz;
    }

    /// <summary>
    /// Measurement Mode
    /// </summary>
    public MeasurementMode MeasurementMode
    {
        get => (MeasurementMode)(_value & 0x03);
        set
        {
            if (value == MeasurementMode.Reserved)
                return;
            
            _value &= 0x7c;
            _value |= (byte)value;
        }
    }

    /// <summary>
    /// Typical Data Output Rate. (Hz)
    /// </summary>
    public DataOutput DataOutput
    {
        get => (DataOutput)(_value & 0x1c);
        set
        {
            _value &= 0x63;
            _value |= (byte)value;
        }
    }

    /// <summary>
    /// Number of Samples averaged.
    /// </summary>
    public MeasurementsAveraged MeasurementsAveraged
    {
        get => (MeasurementsAveraged)(_value & 0x60);
        set
        {
            _value &= 0x1f;
            _value |= (byte)value;
        }
    }

    public const byte FactoryDefault = 0x10;
    
    public static ConfigurationA Default => new ConfigurationA(FactoryDefault);

    public static implicit operator ConfigurationA(byte value) => new ConfigurationA(value);
    public static implicit operator byte(ConfigurationA value) => value._value;
}

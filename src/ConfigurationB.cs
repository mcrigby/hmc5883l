namespace CutilloRigby.Device.HMC5883L;

public sealed class ConfigurationB
{
    private byte _value;

    private ConfigurationB(byte value)
    {
        _value = value;
        _value &= 0xe0;
    }

    public Gain Gain
    {
        get => (Gain)(_value & 0xe0);
        set
        {
            _value &= 0x1f;
            _value |= (byte)value;
        }
    }

    public short GetGain()
    {
        return Gain.GetGain();
    }

    public const byte FactoryDefault = 0x20;
    
    public static ConfigurationB Default => new ConfigurationB(FactoryDefault);

    public static implicit operator ConfigurationB(byte value) => new ConfigurationB(value);
    public static implicit operator byte(ConfigurationB value) => value._value;
}

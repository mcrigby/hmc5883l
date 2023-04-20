namespace CutilloRigby.Device.HMC5883L;

/// <summary>
/// Typical Data Output Rate. (Hz)
/// </summary>
public enum DataOutput : byte
{
    /// <summary>
    /// Data Output Rate: 0.75 Hz
    /// </summary>
    _750mHz = 0x0,

    /// <summary>
    /// Data Output Rate: 1.5 Hz
    /// </summary>
    _1500mHz = 0x4,

    /// <summary>
    /// Data Output Rate: 3 Hz
    /// </summary>
    _3000mHz = 0x8,

    /// <summary>
    /// Data Output Rate: 7.5 Hz
    /// </summary>
    _7500mHz = 0xC,

    /// <summary>
    /// Data Output Rate: 15 Hz
    /// </summary>
    _15000mHz = 0x10, // Default

    /// <summary>
    /// Data Output Rate: 30 Hz
    /// </summary>
    _30000mHz = 0x14,

    /// <summary>
    /// Data Output Rate: 75 Hz
    /// </summary>
    _75000mHz = 0x18,

    /// <summary>
    /// Reserved
    /// </summary>
    Reserved = 0x1c,
}

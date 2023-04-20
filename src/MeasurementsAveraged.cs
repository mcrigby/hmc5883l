namespace CutilloRigby.Device.HMC5883L;

/// <summary>
/// Number of Samples averaged.
/// </summary>
public enum MeasurementsAveraged : byte
{
    /// <summary>
    /// Number of Samples averaged: 1
    /// </summary>
    _1 = 0x00, // Default

    /// <summary>
    /// Number of Samples averaged: 2
    /// </summary>
    _2 = 0x20,

    /// <summary>
    /// Number of Samples averaged: 4
    /// </summary>
    _4 = 0x40,

    /// <summary>
    /// Number of Samples averaged: 8
    /// </summary>
    _8 = 0x60,
}

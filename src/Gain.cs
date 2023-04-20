namespace CutilloRigby.Device.HMC5883L;

/// <summary>
/// Ratio to convert from Raw Data to Gauss
/// </summary>
public enum Gain : byte
{
    /// <summary>
    /// Recommended Sensor Field Range: +/- 0.88 Ga
    /// Gain: 1370 LSb/Gauss
    /// Digital Resolution: 0.73 mG/LSb
    /// </summary>
    _0 = 0x00,

    /// Recommended Sensor Field Range: +/- 1.3 Ga
    /// Gain: 1090 LSb/Gauss
    /// Digital Resolution: 0.92 mG/LSb
    _1 = 0x20, // Default

    /// Recommended Sensor Field Range: +/- 1.9 Ga
    /// Gain: 820 LSb/Gauss
    /// Digital Resolution: 1.22 mG/LSb
    _2 = 0x40,

    /// Recommended Sensor Field Range: +/- 2.5 Ga
    /// Gain: 660 LSb/Gauss
    /// Digital Resolution: 1.52 mG/LSb
    _3 = 0x60,

    /// Recommended Sensor Field Range: +/- 4.0 Ga
    /// Gain: 440 LSb/Gauss
    /// Digital Resolution: 2.27 mG/LSb
    _4 = 0x80,

    /// Recommended Sensor Field Range: +/- 4.7 Ga
    /// Gain: 390 LSb/Gauss
    /// Digital Resolution: 2.56 mG/LSb
    _5 = 0xa0,

    /// Recommended Sensor Field Range: +/- 5.6 Ga
    /// Gain: 330 LSb/Gauss
    /// Digital Resolution: 3.03 mG/LSb
    _6 = 0xc0,

    /// Recommended Sensor Field Range: +/- 8.1 Ga
    /// Gain: 230 LSb/Gauss
    /// Digital Resolution: 4.35 mG/LSb
    _7 = 0xe0,
}

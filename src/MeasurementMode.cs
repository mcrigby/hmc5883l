namespace CutilloRigby.Device.HMC5883L;

/// <summary>
/// Measurement Mode
/// </summary>
public enum MeasurementMode : byte
{
    /// <summary>
    /// Normal Measurement Configuration. Positive and Negative resistive loads
    /// are left floating and high impedance.
    /// </summary>
    Normal = 0x0, // Default

    /// <summary>
    /// Positive Bias Configuration. A positive current is forced across the 
    /// resistive load for all three axes.
    /// </summary>
    Positive_Bias = 0x1,

    /// <summary>
    /// Negative Bias Configuration. A negative current is forced across the 
    /// resistive load for all three axes.
    /// </summary>
    Negative_Bias = 0x2,

    /// <summary>
    /// Reserved
    /// </summary>
    Reserved = 0x3,
}

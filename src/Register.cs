namespace CutilloRigby.Device.HMC5883L;

/// <summary>
/// HMC6352 RAM Register
/// </summary>
internal enum Register : byte
{
    /// <summary>
    /// Configuration Register A. Contains Measurement Samples, Data Output Rate, and Measurement Configuration.
    /// </summary>
    Configuration_A = 0x00,

    /// <summary>
    /// Configuration Register B. Contains Gain.
    /// </summary>
    Configuration_B = 0x01,

    /// <summary>
    /// Mode Register. Contains Operating Mode, and HighSpeed I2C Select.
    /// </summary>
    Mode = 0x02,
    
    /// <summary>
    /// Data Output X MSB Register.
    /// ReadOnly
    /// </summary>
    Data_Output_X_MSB = 0x03,

    /// <summary>
    /// Data Output X LSB Register.
    /// ReadOnly
    /// </summary>
    Data_Output_X_LSB = 0x04,

    /// <summary>
    /// Data Output Z MSB Register.
    /// ReadOnly
    /// </summary>
    Data_Output_Z_MSB = 0x05,

    /// <summary>
    /// Data Output Z LSB Register.
    /// ReadOnly
    /// </summary>
    Data_Output_Z_LSB = 0x06,

    /// <summary>
    /// Data Output Y MSB Register.
    /// ReadOnly
    /// </summary>
    Data_Output_Y_MSB = 0x07,

    /// <summary>
    /// Data Output Y LSB Register.
    /// ReadOnly
    /// </summary>
    Data_Output_Y_LSB = 0x08,

    /// <summary>
    /// Status Register. Contains Data Locked, and Data Ready.
    /// ReadOnly
    /// </summary>
    Status = 0x09,

    /// <summary>
    /// Identification Register A. 0x78.
    /// ReadOnly
    /// </summary>
    Identification_A = 0x0A,

    /// <summary>
    /// Identification Register B. 0x34.
    /// ReadOnly
    /// </summary>
    Identification_B = 0x0B,

    /// <summary>
    /// Identification Register C. 0x33.
    /// ReadOnly
    /// </summary>
    Identification_C = 0x0C,
}

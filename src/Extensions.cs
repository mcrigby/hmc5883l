namespace CutilloRigby.Device.HMC5883L;

public static class HMC5883LExtensions
{
    private const short Gain5 = 390;

    public static short GetGain(this Gain gain)
    {
        return gain switch
        {
            Gain._0 => 1370,
            Gain._1 => 1090,
            Gain._2 => 820,
            Gain._3 => 660,
            Gain._4 => 440,
            Gain._5 => Gain5,
            Gain._6 => 330,
            Gain._7 => 230,
            _ => 1,
        };
    }

    internal static Gain Next(this Gain gain)
    {
        return gain switch
        {
            Gain._0 => Gain._1,
            Gain._1 => Gain._2,
            Gain._2 => Gain._3,
            Gain._3 => Gain._4,
            Gain._4 => Gain._5,
            Gain._5 => Gain._6,
            Gain._6 => Gain._7,
            _ => Gain._0,
        };
    }
    public static (short, short) GetSelfTestRange(this Gain gain, MeasurementMode measurementMode)
    {
        const short Gain5_Positive_HighLimit = 575;
        const short Gain5_Positive_LowLimit = 243;
        const short Gain5_Negative_HighLimit = -243;
        const short Gain5_Negative_LowLimit = -575;

        return measurementMode switch
        {
            MeasurementMode.Positive_Bias => GetSelfTestRange(gain, Gain5_Positive_LowLimit, Gain5_Positive_HighLimit),
            MeasurementMode.Negative_Bias => GetSelfTestRange(gain, Gain5_Negative_LowLimit, Gain5_Negative_HighLimit),
            _ => (0, 0)
        };
    }
    private static (short, short) GetSelfTestRange(Gain gain, short gain5LowLimit, short gain5HighLimit)
    {
        if (gain == Gain._5)
            return (gain5LowLimit, gain5HighLimit);
        
        var offset = ((double)GetGain(gain)) / Gain5;

        return (
            (short)(gain5LowLimit * offset), 
            (short)(gain5HighLimit * offset)
        );
    }
}
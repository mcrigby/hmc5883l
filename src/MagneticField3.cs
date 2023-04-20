using UnitsNet;

namespace CutilloRigby.Device.HMC5883L;

public struct MagneticField3
{    
    public MagneticField3(MagneticField x, MagneticField y, MagneticField z) 
    { 
        X = x;
        Y = y;
        Z = z;
    }

    public MagneticField X { get; set; }
    public MagneticField Y { get; set; }
    public MagneticField Z { get; set; }
}

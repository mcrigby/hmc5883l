3-Axis Digital Compass IC
HMC5883L

var _hmc5883l = HMC5883L.Create();

_hmc5883l.MeasurementsAveraged = MeasurementsAveraged._4;
_hmc5883l.DataOutputRate = DataOutput._15000mHz;
_hmc5883l.MeasurementMode = MeasurementMode.Normal;
_hmc5883l.Gain = Gain._5;

while (true)
{
    output($"{_hmc5883l.Heading.Degrees:n2}°");
    await Task.Delay(500);
}

KNOWN ISSUE: Contnuous Mode not working.
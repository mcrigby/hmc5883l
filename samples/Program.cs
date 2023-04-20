using CutilloRigby.Device.HMC5883L;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) => cts.Cancel();

var _hmc5883l = HMC5883L.Create();

// if (!_hmc5883l.SelfTest(Gain._5, MeasurementMode.Positive_Bias))
//     Console.WriteLine("Positive Self Test Failed");
// else
//     Console.WriteLine($"Positive Bias Passed at Gain {_hmc5883l.Gain}.");

// if (!_hmc5883l.SelfTest(Gain._5, MeasurementMode.Negative_Bias))
//     Console.WriteLine("Negative Self Test Failed");
// else
//     Console.WriteLine($"Negative Bias Passed at Gain {_hmc5883l.Gain}.");

_hmc5883l.MeasurementsAveraged = MeasurementsAveraged._4;
_hmc5883l.DataOutputRate = DataOutput._15000mHz;
_hmc5883l.MeasurementMode = MeasurementMode.Normal;
_hmc5883l.Gain = Gain._5;

while(!cts.IsCancellationRequested)
{
    Console.WriteLine($"{_hmc5883l.Heading.Degrees:n2}°");
    await Task.Delay(500);
}

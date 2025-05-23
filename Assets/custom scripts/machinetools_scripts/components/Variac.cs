using SpiceSharp;
using SpiceSharp.Components;

class Variac : Subcircuit
{
    static double baseAmplitude = 220;
    static double frequency = 50;

    public Variac(string name, double factor)
            : base(name, new SubcircuitDefinition(new Circuit(
                new VoltageSource("V1", "pin1", "ground", new Sine(0, baseAmplitude * factor, frequency)),
                new VoltageSource("V2", "pin2", "ground", new Sine(0, baseAmplitude * factor, frequency, 0, 0, 120)),
                new VoltageSource("V3", "pin3", "ground", new Sine(0, baseAmplitude * factor, frequency, 0, 0, 240))
            ), "pin1", "pin2", "pin3", "ground"))
    {
    }
}
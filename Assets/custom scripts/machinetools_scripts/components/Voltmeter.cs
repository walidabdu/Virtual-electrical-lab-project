using SpiceSharp;
using SpiceSharp.Components;


public class VoltMeter : Subcircuit
{
    public VoltMeter(string name)
        : base(name, new SubcircuitDefinition(new Circuit(
            new Resistor("R_parallel", "pin1", "pin2", 1e12)
        ), "pin1", "pin2"))
    {
    }
}
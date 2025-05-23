using SpiceSharp;
using SpiceSharp.Components;

public class Wattmeter : Subcircuit
{
    public Wattmeter(string name)
        : base(name, new SubcircuitDefinition(new Circuit(
            new Resistor("R_series", "pin1", "pin2", 0),
            new Resistor("R_parallel", "pin1", "pin3", 1e12)
        ), "pin1", "pin2", "pin3"))
    {
    }
}
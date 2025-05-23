using SpiceSharp;
using SpiceSharp.Components;


public class Wire : Subcircuit
{
    public Wire(string name)
        : base(name, new SubcircuitDefinition(new Circuit(
            new Resistor("R_series", "pin1", "pin2", 0)
        ), "pin1", "pin2"))
    {
    }
}
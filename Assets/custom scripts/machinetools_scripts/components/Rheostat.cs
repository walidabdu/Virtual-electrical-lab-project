using SpiceSharp;
using SpiceSharp.Components;


// star connected three phase balanced variable load
public class Rheostat : Subcircuit
{
    public Rheostat(string name, double value)
        : base(name, new SubcircuitDefinition(new Circuit(
            new Resistor("R1", "pin1", "common", value),
            new Resistor("R2", "pin2", "common", value),
            new Resistor("R3", "pin3", "common", value)
        ), "pin1", "pin2", "pin3", "common"))
    {
    }
}
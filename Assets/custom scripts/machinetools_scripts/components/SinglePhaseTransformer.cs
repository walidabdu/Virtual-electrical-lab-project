using System;
using SpiceSharp;
using SpiceSharp.Components;

class SinglePhaseTransformer : Subcircuit
{

    // parameters taken from theraja machine book
    static double rc = 250.0;
    static double xm = 1250.0;
    static double r1 = 0.286;
    static double x1 = 0.73;
    static double r2Prime = 0.319;
    static double x2Prime = 0.73;
    static double f = 50.0;
    static double omega = 2 * Math.PI * f;
    static double turnsRatio = 2;

    static double lm = xm / omega;
    static double l1 = x1 / omega;
    static double l2Prime = x2Prime / omega;
    static double r0 = r1 + r2Prime;
    static double l0 = l1 + l2Prime;


    public SinglePhaseTransformer(string name) :
        base(name, new SubcircuitDefinition(new Circuit(
            new Resistor("Rc", "N1", "ground", rc),
            new Resistor("Rbreak_lm", "N1", "N1_lm", 1e-6), // Very small resistance  
            new Inductor("Lm", "N1_lm", "ground", lm).SetParameter("ic", 0.0),

            new Resistor("R0", "N1", "N2", r0),
            new Inductor("L0", "N2", "N3", l0).SetParameter("ic", 0.0),

            new VoltageControlledVoltageSource("V2", "S1", "ground", "N3", "ground", 1.0 / turnsRatio)
        ), "N1", "S1", "ground"))
    {
    }
}
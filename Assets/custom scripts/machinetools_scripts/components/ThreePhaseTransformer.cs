using SpiceSharp;
using SpiceSharp.Components;

class ThreePhaseTransformer : Subcircuit
{
    // Internally Y-Y connected 3 Phase Transformer

    public ThreePhaseTransformer(string name) :
        base(name, new SubcircuitDefinition(new Circuit(
            new SinglePhaseTransformer("tran1").Connect("P1", "S1", "ground"),
            new SinglePhaseTransformer("tran2").Connect("P2", "S2", "ground"),
            new SinglePhaseTransformer("tran3").Connect("P3", "S3", "ground")

        ), "P1", "P2", "P3", "S1", "S2", "S3", "ground"))
    {

    }
}
/*using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using SpiceSharp.Components;

public class MainCircuitManager : CircuitManager1
{
    [SerializeField] private ResultsDisplayer resultsDisplayer;
    private SimulationRunner simulationRunner = new SimulationRunner();

    public override void RunSimulation()
    {
        base.RunSimulation();
        
        if (circuit == null || circuit.Count == 0)
        {
            resultsDisplayer?.DisplayError("No components in circuit");
            return;
        }

        try
        {
            simulationRunner.RunDCAnalysis(circuit, circuitNodes);
            resultsDisplayer?.DisplayResults(
                new Dictionary<string, double>(simulationRunner.NodeVoltages),
                new Dictionary<string, double>(simulationRunner.BranchCurrents)
            );
        }
        catch (System.Exception ex)
        {
            resultsDisplayer?.DisplayError(ex.Message);
            Debug.LogError(ex);
        }
    }

    protected override void BuildCircuit()
    {
        base.BuildCircuit();

        var resistors = FindObjectsOfType<ResistorComponent>();
        var sources = FindObjectsOfType<DCSourceComponent>();
        var capacitors = FindObjectsOfType<CapacitorComponent>();
        var inductors = FindObjectsOfType<InductorComponent>();

        bool hasGroundConnection = circuitNodes.Contains("0");

        // Process Resistors
        foreach (var r in resistors)
        {
            if (r == null || r.simulationData == null) continue;
            
            string nodeA = r.simulationData.nodeA ?? "0";
            string nodeB = r.simulationData.nodeB ?? "0";
            hasGroundConnection |= (nodeA == "0" || nodeB == "0");
            
            circuitNodes.Add(nodeA);
            circuitNodes.Add(nodeB);
            circuit.Add(new Resistor(GenerateComponentId(r), nodeA, nodeB, r.simulationData.resistance));
        }

        // Process Sources - Critical Fix Here
        bool hasValidSource = false;
        DCSourceComponent firstValidSource = null;
        
        foreach (var s in sources)
        {
            if (s == null || s.simulationData == null) continue;
            
            if (!hasValidSource) firstValidSource = s;
            hasValidSource = true;
            
            string posNode = s.simulationData.posNode ?? "0";
            string negNode = s.simulationData.negNode ?? "0";
            hasGroundConnection |= (posNode == "0" || negNode == "0");
            
            circuitNodes.Add(posNode);
            circuitNodes.Add(negNode);
            circuit.Add(new VoltageSource(GenerateComponentId(s), posNode, negNode, s.simulationData.voltage));
        }

        // Process Capacitors
        foreach (var c in capacitors)
        {
            if (c == null || c.simulationData == null) continue;
            
            string nodeA = c.simulationData.nodeA ?? "0";
            string nodeB = c.simulationData.nodeB ?? "0";
            hasGroundConnection |= (nodeA == "0" || nodeB == "0");
            
            circuitNodes.Add(nodeA);
            circuitNodes.Add(nodeB);
            circuit.Add(new Capacitor(GenerateComponentId(c), nodeA, nodeB, c.simulationData.capacitance));
        }

        // Process Inductors
        foreach (var i in inductors)
        {
            if (i == null || i.simulationData == null) continue;
            
            string nodeA = i.simulationData.nodeA ?? "0";
            string nodeB = i.simulationData.nodeB ?? "0";
            hasGroundConnection |= (nodeA == "0" || nodeB == "0");
            
            circuitNodes.Add(nodeA);
            circuitNodes.Add(nodeB);
            circuit.Add(new Inductor(GenerateComponentId(i), nodeA, nodeB, i.simulationData.inductance));
        }

        // Safe Ground Connection - Critical Fix
        if (!hasGroundConnection && enforceGroundNode && hasValidSource)
        {
            string sourceId = $"Vgnd_{System.Guid.NewGuid().ToString("N").Substring(0, 4)}";
            circuit.Add(new VoltageSource(
                sourceId, 
                firstValidSource.simulationData.posNode ?? "node_1", 
                "0", 
                firstValidSource.simulationData.voltage));
            circuitNodes.Add("0");
        }
        else if (enforceGroundNode && !hasValidSource)
        {
            Debug.LogWarning("Ground enforcement enabled but no valid voltage sources found");
        }
    }
}*/
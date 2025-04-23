/*using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationRunner
{
    public Dictionary<string, double> NodeVoltages { get; private set; }
    public Dictionary<string, double> BranchCurrents { get; private set; }

    public void RunDCAnalysis(Circuit circuit, HashSet<string> circuitNodes)
    {
        if (circuit == null) throw new System.ArgumentNullException(nameof(circuit));
    if (circuitNodes == null) throw new System.ArgumentNullException(nameof(circuitNodes));
    
    NodeVoltages = new Dictionary<string, double>();
    BranchCurrents = new Dictionary<string, double>();
        var op = new OP("DC operating point");
        NodeVoltages = new Dictionary<string, double>();
        BranchCurrents = new Dictionary<string, double>();

        // Setup voltage exports
        foreach (var node in circuitNodes.Where(n => n != "0"))
        {
            var export = new RealVoltageExport(op, node);
            NodeVoltages[node] = 0;
            op.ExportSimulationData += (sender, args) => NodeVoltages[node] = export.Value;
        }

        // Setup current exports
        foreach (var entity in circuit)
        {
            if (entity is IComponent component)
            {
                try
                {
                    var export = new RealPropertyExport(op, entity.Name, "i");
                    BranchCurrents[entity.Name] = 0;
                    op.ExportSimulationData += (sender, args) =>
                        BranchCurrents[entity.Name] = export.Value;
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"Current export failed for {entity.Name}: {ex.Message}");
                }
            }
        }

        op.Run(circuit);
    }

    public (List<double> timePoints, Dictionary<string, List<double>> voltageResults, Dictionary<string, List<double>> currentResults) 
        RunTransientAnalysis(Circuit circuit, HashSet<string> circuitNodes, float stopTime, float step)
    {
        var tran = new Transient("Transient Analysis", stopTime, step);
        var timePoints = new List<double>();
        var voltageResults = new Dictionary<string, List<double>>();
        var currentResults = new Dictionary<string, List<double>>();

        // Setup time tracking
        tran.ExportSimulationData += (sender, args) => {
            timePoints.Add(args.Time);
        };

        // Setup voltage exports
        foreach (var node in circuitNodes.Where(n => n != "0"))
        {
            voltageResults[node] = new List<double>();
            var export = new RealVoltageExport(tran, node);
            tran.ExportSimulationData += (sender, args) => voltageResults[node].Add(export.Value);
        }

        // Setup current exports
        foreach (var entity in circuit)
        {
            if (entity is IComponent component)
            {
                currentResults[entity.Name] = new List<double>();
                var export = new RealPropertyExport(tran, entity.Name, "i");
                tran.ExportSimulationData += (sender, args) => currentResults[entity.Name].Add(export.Value);
            }
        }

        tran.Run(circuit);
        return (timePoints, voltageResults, currentResults);
    }
}*/
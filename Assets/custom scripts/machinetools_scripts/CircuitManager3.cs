using System;
using System.Collections.Generic;
using System.Linq;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using SpiceSharp.Validation;
using UnityEngine;


public static class CircuitManager3 {
    public readonly static Circuit ckt = new Circuit();


    public static Dictionary<string, double> SimulationResult = new()
    {
        {"VM1", 0},
        {"VM2", 0},
        {"VM3", 0},
        {"AM1", 0},
        {"AM2", 0},
        {"AM3", 0},
        {"WM1", 0}
    };
    readonly static string ground = "0";

    static IComponent variac = new Variac("VAR", 1);
    static IComponent threePhaseTrasformer = new ThreePhaseTransformer("TPT");
    static IComponent ammeter1 = new Ammeter("AM1");
    static IComponent ammeter2 = new Ammeter("AM2");
    static IComponent ammeter3 = new Ammeter("AM3");
    static IComponent voltmeter1 = new VoltMeter("VM1");
    static IComponent voltmeter2 = new VoltMeter("VM2");
    static IComponent voltmeter3 = new VoltMeter("VM3");
    static IComponent wattmeter = new Wattmeter("WM1");
    static IComponent rheostat = new Rheostat("RH", 10);

    static Dictionary<string, IComponent> deviceNameToDeviceMap = new()
    {
        {"VAR", variac },
        {"TPT", threePhaseTrasformer},
        {"AM1", ammeter1},
        {"AM2", ammeter2},
        {"AM3", ammeter3},
        {"VM1", voltmeter1},
        {"VM2", voltmeter2},
        {"VM3", voltmeter3},
        {"WM1", wattmeter},
        {"RH", rheostat},
    };



    public static void InitializeCircuit() {
        Debug.Log("CircuitManager3.InitializeCircuit() called.");
        variac.Connect("VAR_1", "VAR_2", "VAR_3", ground);
        threePhaseTrasformer.Connect("TPT_P_1", "TPT_P_2", "TPT_P_3", "TPT_S_1", "TPT_S_2", "TPT_S_3", ground);
        ammeter1.Connect("AM1_1", "AM1_2");
        ammeter2.Connect("AM2_1", "AM2_2");
        ammeter3.Connect("AM3_1", "AM3_2");
        voltmeter1.Connect("VM1_1", "VM1_2");
        voltmeter2.Connect("VM2_1", "VM2_2");
        voltmeter3.Connect("VM3_1", "VM3_2");
        wattmeter.Connect("WM_1", "WM_2", "WM_3");
        rheostat.Connect("RH_1", "RH_2", "RH_3", ground);
    }

    public static void ConnectWire(string wireName, string nodeName, bool runSimulation = true) {
        var deviceName = nodeName.Split('_')[0];
        if (!ckt.Contains(wireName)) {
            var newWire = new Wire(wireName).Connect(nodeName, $"unconnected_{wireName}_1");
            Debug.Log(wireName);
            ckt.Add(newWire);
        } else {
            Wire wire = (Wire)ckt[wireName];
            var currentNodes = wire.Nodes;

            string[] newNodes = currentNodes.ToArray();

            if (newNodes[0].StartsWith("unconnected_"))
                newNodes[0] = nodeName;
            else if (newNodes[1].StartsWith("unconnected_"))
                newNodes[1] = nodeName;

            wire.Connect(newNodes);
        }

        if (!ckt.Contains(deviceName))
            ckt.Add(deviceNameToDeviceMap[deviceName]);

        if (runSimulation)
            TryRunningSimulation();
    }

    public static void DisconnectWire(string wireName, string nodeName, bool runSimulation = true) {

        var deviceName = nodeName.Split('_')[0];
        if (ckt.Contains(wireName)) {
            Wire wire = (Wire)ckt[wireName];
            var currentNodes = wire.Nodes;
            string[] newNodes = currentNodes.ToArray();

            if (currentNodes[0] == nodeName)
                newNodes[0] = $"unconnected_{wireName}_0";
            else if (currentNodes[1] == nodeName)
                newNodes[1] = $"unconnected_{wireName}_1";

            // floating wire
            if (newNodes[0].StartsWith("unconnected_") && newNodes[1].StartsWith("unconnected_"))
                ckt.Remove(wire);
            else
                wire.Connect(newNodes);

            var device = deviceNameToDeviceMap[deviceName];
            var deviceNodes = device.Nodes;
            ckt.Remove(deviceName);
            var allNodes = FindAllNodes();

            foreach (var node in deviceNodes) {
                if (node != "0" && allNodes.Contains(node)) {
                    ckt.Add(device);
                    break;
                }
            }
        }

        if (runSimulation)
            TryRunningSimulation();
    }

    private static HashSet<string> FindAllNodes() {
        var allNodes = new HashSet<string>();
        foreach (var entity in ckt) {
            if (entity is IComponent component) {
                foreach (string node in component.Nodes)
                    allNodes.Add(node);
            }
        }

        return allNodes;
    }


    public static void TryRunningSimulation() {
        try {

            var tran = new Transient("tran1", 0.01, 0.1);
            List<double> voltage1 = new();
            List<double> voltage2 = new();
            List<double> voltage3 = new();

            List<double> current1 = new();
            List<double> current2 = new();
            List<double> current3 = new();
            List<double> power1 = new();

            RealPropertyExport? ammeterCurrent1 = null;
            RealPropertyExport? ammeterCurrent2 = null;
            RealPropertyExport? ammeterCurrent3 = null;
            RealPropertyExport? wattmeterCurrent1 = null;


            try {
                ammeterCurrent1 = new RealPropertyExport(tran, new[] { "AM1", "R_series" }, "i");
            } catch { }
            try {
                ammeterCurrent2 = new RealPropertyExport(tran, new[] { "AM2", "R_series" }, "i");
            } catch { }
            try {
                ammeterCurrent3 = new RealPropertyExport(tran, new[] { "AM3", "R_series" }, "i");
            } catch { }
            try {
                wattmeterCurrent1 = new RealPropertyExport(tran, new[] { "WM1", "R_series" }, "i");
            } catch { }


            foreach (var _ in tran.Run(ckt)) {
                try {
                    voltage1.Add(tran.GetVoltage("VM1_1", "VM1_2"));
                } catch (Exception e) { if (e is ValidationFailedException) throw e; }
                try {
                    voltage2.Add(tran.GetVoltage("VM2_1", "VM2_2"));
                } catch { }
                try {
                    voltage3.Add(tran.GetVoltage("VM3_1", "VM3_2"));
                } catch { }
                double wattmeterVoltage = 0;
                try {
                    wattmeterVoltage = tran.GetVoltage("WM1_1", "WM1_3");
                } catch { }

                if (ammeterCurrent1 != null) current1.Add(ammeterCurrent1.Value);
                if (ammeterCurrent2 != null) current2.Add(ammeterCurrent2.Value);
                if (ammeterCurrent3 != null) current3.Add(ammeterCurrent3.Value);

                if (wattmeterCurrent1 != null)
                    power1.Add(wattmeterCurrent1.Value * wattmeterVoltage);
            }

            SimulationResult["VM1"] = voltage1.DefaultIfEmpty(0).Max() / Math.Sqrt(2);
            SimulationResult["VM2"] = voltage2.DefaultIfEmpty(0).Max() / Math.Sqrt(2);
            SimulationResult["VM3"] = voltage3.DefaultIfEmpty(0).Max() / Math.Sqrt(2);
            SimulationResult["AM1"] = current1.DefaultIfEmpty(0).Max() / Math.Sqrt(2);
            SimulationResult["AM2"] = current2.DefaultIfEmpty(0).Max() / Math.Sqrt(2);
            SimulationResult["AM3"] = current3.DefaultIfEmpty(0).Max() / Math.Sqrt(2);
            SimulationResult["WM1"] = power1.DefaultIfEmpty(0).Average();
        } catch (ValidationFailedException ex) {
            Debug.Log($"Validation failed with {ex.Rules.ViolationCount} violations:");

            foreach (var violation in ex.Rules.Violations) {
                Debug.Log($"- Rule: {violation.Rule}");
                Debug.Log($"- Subject: {violation.Subject}");

                // Print specific violation type details  
                if (violation is VoltageLoopRuleViolation voltageLoop) {
                    Debug.Log($"  First node: {voltageLoop.First}");
                    Debug.Log($"  Second node: {voltageLoop.Second}");
                } else if (violation is FloatingNodeRuleViolation floatingNode) {
                    Debug.Log($"  Floating variable: {floatingNode.FloatingVariable}");
                    Debug.Log($"  Fixed variable: {floatingNode.FixedVariable}");
                }
            }
        } finally {
            Debug.Log($"From the circuit, Voltage1: {SimulationResult["VM1"]}");
        }

    }

    private static List<Wire> FindWiresConnectedToANode(string node) {
        List<Wire> wires = new List<Wire>();
        foreach (var entity in ckt) {
            if (entity is Wire wire && wire.Nodes.Contains(node)) {
                wires.Add(wire);
            }
        }

        return wires;
    }


    public static void UpdateRheostatValue(double factor) {
        //wires connected to each rheostat nodes 
        var wires1 = FindWiresConnectedToANode("RH_1");
        var wires2 = FindWiresConnectedToANode("RH_2");
        var wires3 = FindWiresConnectedToANode("RH_3");

        ckt.Remove(rheostat);

        foreach (var wire in wires1) {
            DisconnectWire(wire.Name, "RH_1", false);
        }
        foreach (var wire in wires2) {
            DisconnectWire(wire.Name, "RH_2", false);
        }

        foreach (var wire in wires3) {
            DisconnectWire(wire.Name, "RH_3", false);
        }

        rheostat = new Rheostat("RH", factor);
        deviceNameToDeviceMap["RH"] = rheostat;
        rheostat.Connect("RH_1", "RH_2", "RH_3", ground);

        foreach (var wire in wires1) {
            ConnectWire(wire.Name, "RH_1", false);
        }
        foreach (var wire in wires2) {
            ConnectWire(wire.Name, "RH_2", false);
        }

        foreach (var wire in wires3) {
            ConnectWire(wire.Name, "RH_3", false);
        }


        TryRunningSimulation();
    }


    public static void UpdateVariacValue(double factor) {
        //wires connected to each variac nodes 
        var wires1 = FindWiresConnectedToANode("VAR_1");
        var wires2 = FindWiresConnectedToANode("VAR_2");
        var wires3 = FindWiresConnectedToANode("VAR_3");

        ckt.Remove(variac);

        foreach (var wire in wires1) {
            DisconnectWire(wire.Name, "VAR_1", false);
        }
        foreach (var wire in wires2) {
            DisconnectWire(wire.Name, "VAR_2", false);
        }

        foreach (var wire in wires3) {
            DisconnectWire(wire.Name, "VAR_3", false);
        }

        variac = new Variac("VAR", factor);
        deviceNameToDeviceMap["VAR"] = variac;
        variac.Connect("VAR_1", "VAR_2", "VAR_3", ground);

        foreach (var wire in wires1) {
            ConnectWire(wire.Name, "VAR_1", false);
        }
        foreach (var wire in wires2) {
            ConnectWire(wire.Name, "VAR_2", false);
        }

        foreach (var wire in wires3) {
            ConnectWire(wire.Name, "VAR_3", false);
        }


        TryRunningSimulation();
    }
}
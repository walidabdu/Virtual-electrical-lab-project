
// using UnityEngine;
// using TMPro;
// using SpiceSharp;
// using SpiceSharp.Components;
// using SpiceSharp.Simulations;
// using System.Collections.Generic;
// using System.Linq;
// using System;

// public class CircuitManager1 : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI resultText;
//     [SerializeField] private CircuitBoard circuitBoard;
//     [SerializeField] private float simulationInterval = 0.5f;

//     private float simulationTimer = 0;
//     private Circuit circuit;
//     private Dictionary<string, string> componentIdMap = new Dictionary<string, string>();
//     private HashSet<string> circuitNodes = new HashSet<string>();

//     // Set to true to force a ground node connection
//     private bool enforceGroundNode = true;

//     void Update()
//     {
//         simulationTimer += Time.deltaTime;
//         if (simulationTimer >= simulationInterval)
//         {
//             RunSimulation();
//             simulationTimer = 0;
//         }
//     }

//     private void BuildCircuit()
//     {
//         circuit = new Circuit();
//         componentIdMap.Clear();
//         circuitNodes.Clear();
//         circuitNodes.Add("0"); // Always include ground

//         // Get all components
//         var resistors = FindObjectsOfType<ResistorComponent>();
//         var sources = FindObjectsOfType<DCSourceComponent>();
//         WireComponent[] wires = FindObjectsOfType<WireComponent>();

//         Debug.Log($"Found {sources.Length} voltage sources, {resistors.Length} resistors, and {wires.Length} wires");

//         // Prepare components
//         foreach (var comp in resistors.Concat<CircuitComponent1>(sources).Concat(wires))
//         {
//             comp.PrepareForSimulation();
//         }

//         // Check for minimum circuit requirements
//         if (sources.Length == 0)
//         {
//             Debug.LogWarning("No voltage sources in circuit - simulation will fail");
//             return;
//         }

//         bool hasGroundConnection = false;

//         // Add resistors
//         foreach (var resistor in resistors.Where(r => r.nodeA != null && r.nodeB != null))
//         {
//             if (resistor is WireComponent)
//                 continue;

//             try
//             {
//                 string nodeA = resistor.simulationData.nodeA;
//                 string nodeB = resistor.simulationData.nodeB;

//                 if (nodeA == "0" && nodeB == "0") hasGroundConnection = true;
//                 circuitNodes.Add(nodeA);
//                 circuitNodes.Add(nodeB);

//                 string spiceId = GenerateComponentId(resistor);
//                 componentIdMap[spiceId] = resistor.gameObject.name; // Map ID to GameObject name

//                 var spiceResistor = new Resistor(
//                     spiceId,
//                     nodeA,
//                     nodeB,
//                     resistor.simulationData.resistance
//                 );
//                 circuit.Add(spiceResistor);
//                 Debug.Log($"Added resistor {spiceResistor.Name} ({resistor.name})");
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"Resistor error: {ex.Message}");
//             }
//         }

//         // Add voltage sources
//         foreach (var source in sources)
//         {
//             try
//             {
//                 if (source.nodeA == null && source.nodeB == null)
//                 {
//                     Debug.LogWarning($"Source {source.name} has null nodes - skipping");
//                     continue;
//                 }

//                 string posNode = source.simulationData.posNode;
//                 string negNode = source.simulationData.negNode;

//                 if (posNode == negNode)
//                 {
//                     Debug.LogError($"Source {source.name} has same nodes: {posNode}");
//                     continue;
//                 }


// if (posNode == "0" && negNode == "0") hasGroundConnection = true;
//                 circuitNodes.Add(posNode);
//                 circuitNodes.Add(negNode);

//                 string sourceId = $"V_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
//                 componentIdMap[sourceId] = source.gameObject.name; // Map ID to GameObject name

//                 var spiceSource = new VoltageSource(
//                     sourceId,
//                     posNode,
//                     negNode,
//                     source.simulationData.voltage
//                 );
//                 circuit.Add(spiceSource);
//                 Debug.Log($"Added voltage source {sourceId} ({source.name})");
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"Source error: {ex.Message}");
//             }
//         }

//         // Add wires
//         foreach (var wire in wires.Where(w => w.nodeA != null && w.nodeB != null))
//         {
//             try
//             {
//                 string nodeA = wire.simulationData.nodeA;
//                 string nodeB = wire.simulationData.nodeB;

//                 if (nodeA == nodeB)
//                 {
//                     Debug.LogWarning($"Wire {wire.name} has identical nodes: {nodeA}");
//                     continue;
//                 }

//                 if (nodeA == "0" && nodeB == "0") hasGroundConnection = true;
//                 circuitNodes.Add(nodeA);
//                 circuitNodes.Add(nodeB);

//                 string spiceId = GenerateComponentId(wire);
//                 componentIdMap[spiceId] = wire.gameObject.name; // Map ID to GameObject name

//                 var spiceWire = new Resistor(
//                     spiceId,
//                     nodeA,
//                     nodeB,
//                     0.001f
//                 );
//                 circuit.Add(spiceWire);
//                 Debug.Log($"Added wire {spiceWire.Name} ({wire.name})");
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"Wire error: {ex.Message}");
//             }
//         }

//         // Force ground connection if needed
//         if (!hasGroundConnection && enforceGroundNode && circuit.Count > 0 && sources.Length > 0)
//         {
//             var source = sources[0];
//             string originalNeg = source.simulationData.negNode;

//             Debug.LogWarning($"Forcing ground connection on {source.name}");

//             string sourceId = $"V_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
//             componentIdMap[sourceId] = source.gameObject.name;

//             circuit.Add(new VoltageSource(
//                 sourceId,
//                 source.simulationData.posNode,
//                 "0",
//                 source.simulationData.voltage
//             ));
//             source.simulationData.negNode = "0";
//         }

//         LogCircuitNodes();
//     }

//     private string GenerateComponentId(CircuitComponent1 component)
//     {
//         string prefix = component.GetType().Name[0].ToString();
//         return $"{prefix}_{component.GetHashCode()}_{Guid.NewGuid().ToString("N").Substring(0, 4)}";
//     }

//     private void LogCircuitNodes()
//     {
//         Debug.Log($"Circuit nodes: {string.Join(", ", circuitNodes)}");
//         Debug.Log($"Circuit contains {circuit.Count} components:");
//         foreach (var entity in circuit)
//         {
//             string nodeInfo = entity is IComponent component ? 
//                 $"Nodes: {string.Join(", ", component.Nodes)}" : "";
//             Debug.Log($"  - {entity.Name} ({entity.GetType().Name}): {nodeInfo}");
//         }
//     }

//     private void RunSimulation()
//     {
//         try
//         {
//             BuildCircuit();
//             if (circuit.Count == 0)
//             {
//                 resultText.text = "No valid circuit to simulate";
//                 return;
//             }

//             var op = new OP("DC operating point");
//             var nodeVoltages = new Dictionary<string, double>();
//             var branchCurrents = new Dictionary<string, double>();

// // Setup voltage exports
//             foreach (var node in circuitNodes.Where(n => n != "0"))
//             {
//                 var export = new RealVoltageExport(op, node);
//                 nodeVoltages[node] = 0;
//                 op.ExportSimulationData += (sender, args) => nodeVoltages[node] = export.Value;
//             }

//             // Setup current exports
//             foreach (var entity in circuit)
//             {
//                 try
//                 {
//                     if (entity is VoltageSource || entity is Resistor)
//                     {
//                         var export = new RealPropertyExport(op, entity.Name, "i");
//                         branchCurrents[entity.Name] = 0;
//                         op.ExportSimulationData += (sender, args) =>
//                             branchCurrents[entity.Name] = export.Value;
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     Debug.LogWarning($"Current export failed for {entity.Name}: {ex.Message}");
//                 }
//             }

//             op.Run(circuit);
//             DisplayResults(nodeVoltages, branchCurrents);
//         }
//         catch (Exception ex)
//         {
//             resultText.text = $"Simulation Error: {ex.Message}";
//             Debug.LogError($"Simulation failed: {ex}\n{ex.StackTrace}");
//         }
//     }

// private void DisplayResults(Dictionary<string, double> nodeVoltages, Dictionary<string, double> branchCurrents)
// {
//     var results = new System.Text.StringBuilder();
//     results.AppendLine("<b>CIRCUIT ANALYSIS RESULTS</b>\n");

//     try
//     {
//         // Node Voltages Section
//         results.AppendLine("<color=#00FF00><b>NODE VOLTAGES:</b></color>");

//         // Always show ground explicitly
//         results.AppendLine("0 (Ground Reference): 0.000 V\n");

//         // Get all non-ground nodes sorted numerically
//         var validNodes = nodeVoltages
//             .Where(n => !string.IsNullOrEmpty(n.Key) && n.Key != "0")
//             .OrderBy(n =>
//             {
//                 // Handle both numeric (N1, N2) and alphanumeric nodes
//                 if (n.Key.StartsWith("N") && int.TryParse(n.Key.Substring(1), out int num))
//                     return num;
//                 return int.MaxValue; // Push non-numeric nodes to end
//             })
//             .ThenBy(n => n.Key);

//         if (!validNodes.Any())
//         {
//             results.AppendLine("<color=#FF0000>No node voltages recorded!</color>");
//             results.AppendLine("Possible reasons:");
//             results.AppendLine("- All components are short-circuited");
//             results.AppendLine("- No valid circuit connections");
//             results.AppendLine("- Simulation failed to run");
//         }
//         else
//         {
//             foreach (var node in validNodes)
//             {
//                 results.AppendLine($"• <b>Node {node.Key}</b>: {node.Value:F3} V");
//             }
//         }
//     }
//     catch (Exception voltageEx)
//     {
//         results.AppendLine("\n<color=#FF0000>ERROR DISPLAYING VOLTAGES:</color>");
//         results.AppendLine(voltageEx.Message);
//     }

//     try
//     {
//         // Component Currents Section (Modified to exclude wires)
//         results.AppendLine("\n<color=#00FFFF><b>COMPONENT CURRENTS:</b></color>");

//         var currentGroups = new Dictionary<string, IEnumerable<KeyValuePair<string, double>>>()
//         {
//             { "Resistors", branchCurrents.Where(c => c.Key.StartsWith("R_")) },
//             { "Voltage Sources", branchCurrents.Where(c => c.Key.StartsWith("V_")) }
//             // Wires group removed
//         };

//         bool hasCurrents = false;

//         foreach (var group in currentGroups)
//         {
//             if (!group.Value.Any()) continue;

//             hasCurrents = true;
//             results.AppendLine($"\n<color=#FFA500>{group.Key}:</color>");
//             foreach (var current in group.Value.OrderBy(c => c.Key))
//             {
//                 string displayName = componentIdMap.TryGetValue(current.Key, out string name)
//                     ? name
//                     : current.Key;

//                 string currentValue = current.Key.StartsWith("V_")
//                     ? FormatCurrent(current.Value, "V")
//                     : FormatCurrent(current.Value, "R");

//                 results.AppendLine($"  {displayName.PadRight(20)}: {currentValue}");
//             }
//         }

//         if (!hasCurrents)
//         {
//             results.AppendLine("\n<color=#FF0000>No current measurements!</color>");
//             results.AppendLine("Possible reasons:");
//             results.AppendLine("- All components are open-circuited");
//             results.AppendLine("- No current paths exist");
//             results.AppendLine("- Measurement setup failed");
//         }
//     }
//     catch (Exception currentEx)
//     {
//         results.AppendLine("\n<color=#FF0000>ERROR DISPLAYING CURRENTS:</color>");
//         results.AppendLine(currentEx.Message);
//     }

//     // Final error check
//     if (results.Length < 50) // If minimal content
//     {
//         results.AppendLine("\n<color=#FF0000>CRITICAL ERROR:</color>");
//         results.AppendLine("No simulation data could be displayed");
//         results.AppendLine("Check console for detailed errors");
//     }

//     resultText.text = results.ToString();
// }

// private string FormatCurrent(double value, string componentType)
// {
//     // Format the current with three decimal places; you can adjust based on componentType if needed
//     return $"{value:F3} A";
// }

// }
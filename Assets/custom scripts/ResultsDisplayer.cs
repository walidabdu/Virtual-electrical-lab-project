/*using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class ResultsDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    public void DisplayResults(Dictionary<string, double> nodeVoltages, 
                            Dictionary<string, double> branchCurrents)
    {
        if (resultText == null) return;

        var sb = new StringBuilder();
        sb.AppendLine("<b>CIRCUIT RESULTS</b>");
        
        // Node Voltages
        sb.AppendLine("\n<color=green>NODE VOLTAGES:</color>");
        foreach (var kvp in nodeVoltages.OrderBy(n => n.Key))
            sb.AppendLine($"• {kvp.Key}: {kvp.Value:F3}V");
        
        // Component Currents
        sb.AppendLine("\n<color=blue>COMPONENT CURRENTS:</color>");
        foreach (var kvp in branchCurrents.OrderBy(c => c.Key))
            sb.AppendLine($"• {kvp.Key}: {kvp.Value:F6}A");

        resultText.text = sb.ToString();
        resultText.ForceMeshUpdate();
    }

    public void DisplayError(string message)
    {
        if (resultText != null)
        {
            resultText.text = $"<color=red>ERROR: {message}</color>";
            resultText.ForceMeshUpdate();
        }
    }
}*/
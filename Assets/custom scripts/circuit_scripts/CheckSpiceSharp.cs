using System;
using System.Linq;
using UnityEngine;

public class CheckSpiceSharpNamespaces : MonoBehaviour
{
    void Start()
    {
        var spiceAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.FullName.Contains("SpiceSharp"));

        if (spiceAssembly != null)
        {
            var spiceTypes = spiceAssembly.GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith("SpiceSharp"))
                .ToList();

            Debug.Log("Found " + spiceTypes.Count + " types in SpiceSharp namespace:");
            foreach (var type in spiceTypes)
            {
                Debug.Log("Type: " + type.FullName);
            }
        }
        else
        {
            Debug.LogWarning("SpiceSharp assembly not found.");
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "CapacitorData", menuName = "Circuit/Capacitor Data")]
public class CapacitorSO : ScriptableObject
{
    public string Cname;
    public string nodeA;
    public string nodeB;
    public float capacitance = 1e-6f; // Default 1 µF
}
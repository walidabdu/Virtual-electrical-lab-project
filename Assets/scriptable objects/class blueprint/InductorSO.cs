using UnityEngine;

[CreateAssetMenu(fileName = "InductorData", menuName = "Circuit/Inductor Data")]
public class InductorSO : ScriptableObject
{
    public string Iname;
    public string nodeA;
    public string nodeB;
    public float inductance = 0.001f; // Default 1 mH
}
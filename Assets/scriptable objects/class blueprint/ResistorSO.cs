using UnityEngine;

[CreateAssetMenu(fileName = "Resistor", menuName = "Circuit/Resistor")]
public class ResistorSO : ScriptableObject
{
    public string nodeA;
    public string nodeB;
    public float resistance;
}
using UnityEngine;

[CreateAssetMenu(fileName = "DCSource", menuName = "Circuit/DCSource")]
public class DCSourceSO : ScriptableObject
{
    public string posNode;
    public string negNode;
    public float voltage;
}

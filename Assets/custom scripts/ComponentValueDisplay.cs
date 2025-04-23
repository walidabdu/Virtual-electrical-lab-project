using UnityEngine;

[System.Serializable]
public class ComponentValueDisplay
{
    public GameObject valueDisplayPrefab;
    public Vector3 positionOffset = new Vector3(0, 1f, 0);
    public Vector3 rotationOffset = Vector3.zero;
    public Vector3 scale = Vector3.one;
}
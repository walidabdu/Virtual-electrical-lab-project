using UnityEngine;

public class WireFactory : MonoBehaviour
{
    public GameObject wirePrefab; // Assign your wire prefab in Inspector
    public Transform spawnPosition; // Where new wires appear
    private int wireCount = 0;

    public void CreateNewWire()
    {
        // Instantiate wire
        GameObject newWire = Instantiate(wirePrefab, spawnPosition.position, spawnPosition.rotation);
        wireCount++;
        
        // Name parent wire object
        newWire.name = "Wire" + wireCount;

        // Find and name child pins
        Transform pin1 = newWire.transform.Find("pin1"); // Adjust names to match your prefab
        Transform pin2 = newWire.transform.Find("pin2");
        
        if (pin1 != null) pin1.gameObject.name = "Wire" + wireCount + "_Pin1";
        if (pin2 != null) pin2.gameObject.name = "Wire" + wireCount + "_Pin2";

       
    }

    
}
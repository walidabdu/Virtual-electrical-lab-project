using UnityEngine;
using UnityEngine.UI;

public class CircuitBuilderUI : MonoBehaviour
{
    [Header("Controller Reference")]
    [SerializeField] private CircuitBuilderController controller;
    
    [Header("Mode Selection Buttons")]
    [SerializeField] private Button selectButton;
    [SerializeField] private Button resistorButton;
    [SerializeField] private Button dcSourceButton;
    [SerializeField] private Button wireButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button capacitorButton;
    [SerializeField] private Button inductorButton;

    [Header("Analysis Toggle")]
    [SerializeField] private Toggle transientAnalysisToggle;

    public enum BuildMode
    {
        Select,
        PlaceResistor,
        PlaceDCSource,
        PlaceWire,
        PlaceCapacitor,
        PlaceInductor,
        Remove
    }

    private void Start()
    {
        // Initialize button listeners
        selectButton.onClick.AddListener(() => SetMode(BuildMode.Select));
        resistorButton.onClick.AddListener(() => SetMode(BuildMode.PlaceResistor));
        dcSourceButton.onClick.AddListener(() => SetMode(BuildMode.PlaceDCSource));
        wireButton.onClick.AddListener(() => SetMode(BuildMode.PlaceWire));
        capacitorButton.onClick.AddListener(() => SetMode(BuildMode.PlaceCapacitor));
        inductorButton.onClick.AddListener(() => SetMode(BuildMode.PlaceInductor));
        removeButton.onClick.AddListener(() => SetMode(BuildMode.Remove));

        // Setup transient analysis toggle
       // InitializeTransientToggle();

        // Set default mode
        SetMode(BuildMode.Select);
    }

   /* private void InitializeTransientToggle()
    {
        if (transientAnalysisToggle == null) return;

        var circuitManager = FindObjectOfType<MainCircuitManager>();
        if (circuitManager == null)
        {
            Debug.LogError("MainCircuitManager not found in scene!");
            return;
        }

        // Initialize toggle state
        transientAnalysisToggle.isOn = circuitManager.runTransientAnalysis;

        // Add listener
        transientAnalysisToggle.onValueChanged.AddListener(value => {
            circuitManager.runTransientAnalysis = value;
            Debug.Log($"Transient analysis set to: {value}");
        });
    }*/

    private void SetMode(BuildMode mode)
    {
        if (controller == null)
        {
            Debug.LogError("Controller reference is missing!");
            return;
        }

        controller.SetBuildMode(mode);
        UpdateButtonStates(mode);
    }

    private void UpdateButtonStates(BuildMode currentMode)
    {
        selectButton.interactable = currentMode != BuildMode.Select;
        resistorButton.interactable = currentMode != BuildMode.PlaceResistor;
        dcSourceButton.interactable = currentMode != BuildMode.PlaceDCSource;
        wireButton.interactable = currentMode != BuildMode.PlaceWire;
        capacitorButton.interactable = currentMode != BuildMode.PlaceCapacitor;
        inductorButton.interactable = currentMode != BuildMode.PlaceInductor;
        removeButton.interactable = currentMode != BuildMode.Remove;

        ResetButtonColors();
        SetActiveButtonColor(currentMode);
    }

    private void ResetButtonColors()
    {
        selectButton.image.color = Color.white;
        resistorButton.image.color = Color.white;
        dcSourceButton.image.color = Color.white;
        wireButton.image.color = Color.white;
        capacitorButton.image.color = Color.white;
        inductorButton.image.color = Color.white;
        removeButton.image.color = Color.white;
    }

    private void SetActiveButtonColor(BuildMode mode)
    {
        switch (mode)
        {
            case BuildMode.Select: selectButton.image.color = Color.green; break;
            case BuildMode.PlaceResistor: resistorButton.image.color = Color.green; break;
            case BuildMode.PlaceDCSource: dcSourceButton.image.color = Color.green; break;
            case BuildMode.PlaceWire: wireButton.image.color = Color.green; break;
            case BuildMode.PlaceCapacitor: capacitorButton.image.color = Color.green; break;
            case BuildMode.PlaceInductor: inductorButton.image.color = Color.green; break;
            case BuildMode.Remove: removeButton.image.color = Color.green; break;
        }
    }
}
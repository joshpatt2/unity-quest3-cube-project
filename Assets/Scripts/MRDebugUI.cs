using UnityEngine;
using UnityEngine.UI;

public class MRDebugUI : MonoBehaviour
{
    [Header("UI References")]
    public Button togglePassthroughButton;
    public Text statusText;
    public Text instructionsText;
    
    [Header("MR Managers")]
    public Quest3PassthroughManager passthroughManager;
    public MRSetup mrSetup;
    
    private void Start()
    {
        SetupUI();
    }
    
    private void SetupUI()
    {
        // Setup toggle button
        if (togglePassthroughButton != null)
        {
            togglePassthroughButton.onClick.AddListener(TogglePassthrough);
        }
        
        // Update instructions
        if (instructionsText != null)
        {
            instructionsText.text = "Quest 3 MR Controls:\n" +
                                  "• Menu Button: Toggle Passthrough\n" +
                                  "• Trigger: Interact with Cube\n" +
                                  "• Grip: Reset Cube Position";
        }
        
        UpdateStatusText();
    }
    
    public void TogglePassthrough()
    {
        if (passthroughManager != null)
        {
            passthroughManager.TogglePassthrough();
        }
        else if (mrSetup != null)
        {
            mrSetup.TogglePassthrough();
        }
        
        UpdateStatusText();
    }
    
    private void UpdateStatusText()
    {
        if (statusText != null)
        {
            bool isPassthroughMode = false;
            
            // Check current camera settings to determine mode
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                isPassthroughMode = mainCam.backgroundColor == Color.clear || 
                                  mainCam.backgroundColor.a < 0.5f;
            }
            
            statusText.text = isPassthroughMode ? 
                "Mode: Mixed Reality (Passthrough)" : 
                "Mode: Virtual Reality";
        }
    }
    
    private void Update()
    {
        // Update status periodically
        if (Time.frameCount % 60 == 0) // Every 60 frames (~1 second at 60fps)
        {
            UpdateStatusText();
        }
    }
}

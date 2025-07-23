using UnityEngine;
using UnityEngine.XR;

namespace Quest3VR
{
    /// <summary>
    /// Simple XR status display for Quest 3 VR development
    /// Attach this to a GameObject in your scene to monitor XR status
    /// </summary>
    public class XRStatusDisplay : MonoBehaviour
    {
        [Header("Display Settings")]
        [Tooltip("Show XR status in console")]
        public bool logToConsole = true;
        
        [Tooltip("Update interval in seconds")]
        public float updateInterval = 2f;
        
        private float lastUpdateTime;
        private bool isXRActive;
        private string currentHMDName;
        
        private void Start()
        {
            lastUpdateTime = Time.time;
            UpdateXRStatus();
        }
        
        private void Update()
        {
            if (Time.time - lastUpdateTime >= updateInterval)
            {
                UpdateXRStatus();
                lastUpdateTime = Time.time;
            }
        }
        
        private void UpdateXRStatus()
        {
            // Check if XR is active
            bool wasXRActive = isXRActive;
            isXRActive = XRSettings.enabled && XRSettings.loadedDeviceName != "None";
            
            // Get current HMD name
            string previousHMDName = currentHMDName;
            currentHMDName = XRSettings.loadedDeviceName;
            
            // Log status changes
            if (logToConsole)
            {
                if (wasXRActive != isXRActive || previousHMDName != currentHMDName)
                {
                    LogXRStatus();
                }
            }
        }
        
        private void LogXRStatus()
        {
            Debug.Log("=== XR Status Update ===");
            Debug.Log($"XR Active: {isXRActive}");
            Debug.Log($"XR Device: {currentHMDName}");
            Debug.Log($"XR Supported: {XRSettings.supportedDevices?.Length > 0}");
            
            if (XRSettings.supportedDevices != null && XRSettings.supportedDevices.Length > 0)
            {
                Debug.Log("Supported XR Devices:");
                foreach (string device in XRSettings.supportedDevices)
                {
                    Debug.Log($"  - {device}");
                }
            }
            
            // Display tracking space type
            Debug.Log($"Tracking Origin Mode: {XRDevice.GetTrackingSpaceType()}");
            
            // Check for connected controllers
            var inputDevices = new System.Collections.Generic.List<InputDevice>();
            InputDevices.GetDevices(inputDevices);
            
            if (inputDevices.Count > 0)
            {
                Debug.Log("Connected Input Devices:");
                foreach (var device in inputDevices)
                {
                    Debug.Log($"  - {device.name} ({device.characteristics})");
                }
            }
            else
            {
                Debug.Log("No input devices detected");
            }
        }
        
        private void OnGUI()
        {
            // Simple on-screen display for debugging
            if (!logToConsole) return;
            
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.BeginVertical("box");
            
            GUILayout.Label("XR Status", GUI.skin.label);
            GUILayout.Label($"Active: {isXRActive}");
            GUILayout.Label($"Device: {currentHMDName}");
            GUILayout.Label($"Tracking: {XRDevice.GetTrackingSpaceType()}");
            
            if (GUILayout.Button("Refresh Status"))
            {
                UpdateXRStatus();
                LogXRStatus();
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}

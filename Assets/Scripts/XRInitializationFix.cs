using UnityEngine;
using UnityEngine.XR.Management;
using System.Collections;

namespace Quest3VR
{
    /// <summary>
    /// XR Initialization Fix for Quest 3 VR Development
    /// This script helps resolve NullReferenceException issues with XR Management
    /// </summary>
    public class XRInitializationFix : MonoBehaviour
    {
        [Header("XR Initialization Settings")]
        [Tooltip("Automatically initialize XR on Start")]
        public bool autoInitializeXR = true;
        
        [Tooltip("Show debug logs for XR initialization")]
        public bool enableDebugLogs = true;
        
        [Tooltip("Retry XR initialization if it fails")]
        public bool retryOnFailure = true;
        
        [Tooltip("Maximum number of retry attempts")]
        [Range(1, 5)]
        public int maxRetryAttempts = 3;
        
        private int currentRetryAttempt = 0;
        
        private void Start()
        {
            if (autoInitializeXR)
            {
                StartCoroutine(InitializeXRCoroutine());
            }
        }
        
        private IEnumerator InitializeXRCoroutine()
        {
            if (enableDebugLogs)
                Debug.Log("[XRInitializationFix] Starting XR initialization...");
            
            // Wait for one frame to ensure all systems are ready
            yield return null;
            
            // Check if XR is already initialized
            if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
            {
                if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
                {
                    if (enableDebugLogs)
                        Debug.Log("[XRInitializationFix] XR is already initialized.");
                    yield break;
                }
            }
            
            // Attempt to initialize XR
            yield return InitializeXR();
        }
        
        private IEnumerator InitializeXR()
        {
            currentRetryAttempt++;
            
            if (enableDebugLogs)
                Debug.Log($"[XRInitializationFix] XR initialization attempt {currentRetryAttempt}/{maxRetryAttempts}");
            
            // Ensure XRGeneralSettings instance exists
            if (XRGeneralSettings.Instance == null)
            {
                if (enableDebugLogs)
                    Debug.LogError("[XRInitializationFix] XRGeneralSettings.Instance is null!");
                
                if (retryOnFailure && currentRetryAttempt < maxRetryAttempts)
                {
                    yield return new WaitForSeconds(1f);
                    yield return InitializeXR();
                }
                yield break;
            }
            
            // Get the XR Manager
            var xrManager = XRGeneralSettings.Instance.Manager;
            if (xrManager == null)
            {
                if (enableDebugLogs)
                    Debug.LogError("[XRInitializationFix] XRManagerSettings is null!");
                
                if (retryOnFailure && currentRetryAttempt < maxRetryAttempts)
                {
                    yield return new WaitForSeconds(1f);
                    yield return InitializeXR();
                }
                yield break;
            }
            
            // Initialize XR if not already done
            if (!xrManager.isInitializationComplete)
            {
                if (enableDebugLogs)
                    Debug.Log("[XRInitializationFix] Initializing XR Manager...");
                
                yield return xrManager.InitializeLoader();
                
                if (xrManager.activeLoader != null)
                {
                    if (enableDebugLogs)
                        Debug.Log($"[XRInitializationFix] XR initialized successfully with loader: {xrManager.activeLoader.name}");
                    
                    // Start XR if initialization was successful
                    xrManager.StartSubsystems();
                    
                    if (enableDebugLogs)
                        Debug.Log("[XRInitializationFix] XR subsystems started successfully.");
                }
                else
                {
                    if (enableDebugLogs)
                        Debug.LogWarning("[XRInitializationFix] XR initialization completed but no active loader found.");
                    
                    if (retryOnFailure && currentRetryAttempt < maxRetryAttempts)
                    {
                        yield return new WaitForSeconds(1f);
                        yield return InitializeXR();
                    }
                }
            }
            else
            {
                if (enableDebugLogs)
                    Debug.Log("[XRInitializationFix] XR Manager is already initialized.");
            }
        }
        
        /// <summary>
        /// Manually trigger XR initialization
        /// </summary>
        [ContextMenu("Initialize XR")]
        public void ManualInitializeXR()
        {
            currentRetryAttempt = 0;
            StartCoroutine(InitializeXRCoroutine());
        }
        
        /// <summary>
        /// Check current XR status
        /// </summary>
        [ContextMenu("Check XR Status")]
        public void CheckXRStatus()
        {
            Debug.Log("=== XR Status Check ===");
            
            if (XRGeneralSettings.Instance == null)
            {
                Debug.LogError("XRGeneralSettings.Instance is NULL");
                return;
            }
            
            Debug.Log($"XRGeneralSettings.Instance exists: {XRGeneralSettings.Instance != null}");
            
            if (XRGeneralSettings.Instance.Manager == null)
            {
                Debug.LogError("XRGeneralSettings.Instance.Manager is NULL");
                return;
            }
            
            var manager = XRGeneralSettings.Instance.Manager;
            Debug.Log($"XR Manager exists: {manager != null}");
            Debug.Log($"XR Initialization complete: {manager.isInitializationComplete}");
            Debug.Log($"XR Active loader: {(manager.activeLoader != null ? manager.activeLoader.name : "None")}");
            
            if (manager.activeLoaders != null)
            {
                Debug.Log($"Available loaders count: {manager.activeLoaders.Count}");
                foreach (var loader in manager.activeLoaders)
                {
                    Debug.Log($"  - Loader: {loader.name}");
                }
            }
            else
            {
                Debug.Log("No active loaders found");
            }
        }
        
        private void OnDestroy()
        {
            // Clean up XR when the object is destroyed
            if (XRGeneralSettings.Instance?.Manager != null)
            {
                if (enableDebugLogs)
                    Debug.Log("[XRInitializationFix] Stopping XR subsystems...");
                
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
        }
    }
}

using UnityEngine;

public class GlowingSphereMover : MonoBehaviour
{
    public Transform pathStart; // Starting point of the path
    public Transform pathEnd;   // Ending point of the path
    public float speed = 2f;    // Speed of movement
    public float manualSpeed = 5f; // Speed for manual control

    public GameObject cameraNode;   // Node for controlling the camera
    public Camera mainCamera;       // Reference to the main camera
    public Vector3 defaultCameraOffset = new Vector3(0, 0, -10); // Default camera offset
    public Vector3 ballCameraOffset = new Vector3(0, 5, -5);      // Ball camera offset
    public float rotationSpeed = 5000f; // Speed for camera rotation
    public GameObject helpPanel; // Reference to the HelpPanel GameObject
    public GameObject completionPanel; // Reference to the Completion UI Panel
    private bool movingForward = true;
    private bool isManualControl = false; // Toggle for manual control
    private bool isBallView = false;     // Toggle for ball-following view
    public Transform targetObject; // The object the sphere interacts with
    public float interactionRadius = 2f; // Radius to trigger game completion
    private bool isGameCompleted = false; // Flag to stop movement and control after completion

    public GameObject glowingSphere; // The sphere GameObject to shut off
    public Transform  LightPos; // The sphere GameObject to shut off
    public Material rockyMaterial; // Reference to the RockyGround material
    public Material torusMaterial; // Reference to the Torus material
    private bool isLightOn = true; // Flag to track light state
    public Material GlowingSphereMaterial; // Reference to the GlowingSphereMaterial material
    private float originalLightIntensity = 0.4f; // Store original light intensity
    private float originalSpecularIntensity = 10.78f; // Store original light intensity
    
    void Start()
    {
        // Ensure the camera is parented to the cameraNode
        if (cameraNode != null && mainCamera != null)
        {
            mainCamera.transform.parent = cameraNode.transform;
        }
        
        
        // Set the initial camera offset to default
        SetDefaultView();

         // Hide completion panel at the start
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }
        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
        torusMaterial.SetColor("_BaseColor", Color.black);

   
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Toggle between automatic and manual control
            isManualControl = !isManualControl;
            Debug.Log(isManualControl ? "Manual Control Enabled" : "Automatic Movement Enabled");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press '1' to go back to default camera position
        {
            SetDefaultView();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // Press '2' for ball-following camera view
        {
            SetBallView();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (helpPanel == null)
            {
                helpPanel = GameObject.Find("Canvas");
            }

            if (helpPanel != null)
            {
                helpPanel.SetActive(!helpPanel.activeSelf); // Toggle help panel
            }
        }

         // Shut off glowing sphere and reset pulsating effect
        if (Input.GetKeyDown(KeyCode.L)) // Press 'L' to shut off the sphere
        {
            ToggleLight();
        }

        if (isManualControl)
        {
            ManualControl();
        }
        else
        {
            MoveSphere();
        }

        // Rotate camera with Up and Down Arrow keys
        RotateCamera();
        // Update the cameraNode's position to follow the ball
        UpdateCameraNode();

        // Check for game completion
        CheckGameCompletion();
    }

void ToggleLight()
{
    if (glowingSphere == null)
    {
        // Attempt to find the sphere dynamically if it's null
        glowingSphere = GameObject.Find("GlowingSphere");
    }

    if (glowingSphere != null)
    {
        // Get current active state of the glowing sphere
        bool isCurrentlyActive = glowingSphere.activeSelf;

        // Toggle active state
        glowingSphere.SetActive(!isCurrentlyActive);
        isLightOn = !isCurrentlyActive; // Update the flag

        // Debug the current state
        Debug.Log(isLightOn ? "Glowing Sphere Turned ON" : "Glowing Sphere Turned OFF");

        // Update rocky material's light intensity based on light state
        if (rockyMaterial != null && rockyMaterial.HasProperty("_LightIntensity") && GlowingSphereMaterial != null && GlowingSphereMaterial.HasProperty("_GlowIntensity")&& GlowingSphereMaterial.HasProperty("_SpecularIntensity"))
        {
            if (isLightOn)
            {
                rockyMaterial.SetFloat("_LightIntensity", originalLightIntensity);
                rockyMaterial.SetVector("_LightPosition", LightPos.position);
                rockyMaterial.SetColor("_LightColor", GlowingSphereMaterial.GetColor("_LightColor"));
                GlowingSphereMaterial.SetFloat("_GlowIntensity", originalLightIntensity);
                GlowingSphereMaterial.SetFloat("_SpecularIntensity", originalSpecularIntensity);
                GlowingSphereMaterial.SetFloat("_DiffuseIntensity", originalSpecularIntensity);
         
                CrystalInteraction[] crystals = FindObjectsOfType<CrystalInteraction>();
                foreach (CrystalInteraction crystal in crystals){
                    crystal.StartPulsating();
                }
                Debug.Log($"Light Intensity Set to Original: {originalLightIntensity}");
            }
            else
            {
                
                CrystalInteraction[] crystals = FindObjectsOfType<CrystalInteraction>();
                foreach (CrystalInteraction crystal in crystals){
                    crystal.StopPulsating();
                }
                GlowingSphereMaterial.SetFloat("_GlowIntensity", 0.0f);
                GlowingSphereMaterial.SetFloat("_SpecularIntensity", 0.0f);
                GlowingSphereMaterial.SetFloat("_DiffuseIntensity", 0.0f);
                torusMaterial.SetColor("_BaseColor", Color.black);
                rockyMaterial.SetFloat("_LightIntensity", 0.0f);
                Debug.Log("Light Intensity Set to 0.0");
            }
        }
        else
        {
            Debug.LogWarning("Rocky Material or '_LightIntensity' property is missing!");
        }
       
    }
    else
    {
        Debug.LogError("Glowing Sphere not found!");
    }
}

    void UpdateCameraNode()
    {
        if (cameraNode != null)
        {
            // Keep the camera node at the ball's position
            cameraNode.transform.position = transform.position;
        }
    }

    void SetDefaultView()
    {
        isBallView = false;

        if (cameraNode != null)
        {
            // Position the cameraNode relative to the ball
            cameraNode.transform.position = defaultCameraOffset;

            // Ensure the camera looks at the ball
            mainCamera.transform.localPosition = defaultCameraOffset; // Camera stays at the center of the cameraNode
            mainCamera.transform.LookAt(transform);
        }

        Debug.Log("Default Camera View Activated");
    }

    void SetBallView()
    {
        isBallView = true;

        if (cameraNode != null)
        {
            // Position the cameraNode relative to the ball
            cameraNode.transform.localPosition = ballCameraOffset;

            // Ensure the camera looks at the ball
            mainCamera.transform.localPosition = Vector3.zero; // Camera stays at the center of the cameraNode
            mainCamera.transform.LookAt(transform);
        }

        Debug.Log("Ball Camera View Activated");
    }

    void MoveSphere()
    {
        if (movingForward)
        {
            transform.position = Vector3.MoveTowards(transform.position, pathEnd.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pathEnd.position) < 0.1f)
            {
                movingForward = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pathStart.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pathStart.position) < 0.1f)
            {
                movingForward = true;
            }
        }
    }

 void ManualControl()
{
    // Get the camera's forward and right directions
    Vector3 cameraForward = mainCamera.transform.forward;
    Vector3 cameraRight = mainCamera.transform.right;

    // Flatten the directions to prevent movement in Y when pressing horizontal keys
    cameraForward.y = 0;
    cameraRight.y = 0;
    cameraForward.Normalize();
    cameraRight.Normalize();

    Vector3 movement = Vector3.zero;

    // Forward and backward movement aligned to the camera's forward direction
    if (Input.GetKey(KeyCode.W)) // Move forward
    {
        movement += cameraForward;
    }
    if (Input.GetKey(KeyCode.S)) // Move backward
    {
        movement -= cameraForward;
    }

    // Left and right movement aligned to the camera's right direction
    if (Input.GetKey(KeyCode.A)) // Move left
    {
        movement -= cameraRight;
    }
    if (Input.GetKey(KeyCode.D)) // Move right
    {
        movement += cameraRight;
    }

    // Up and down movement (along the global Y-axis)
    if (Input.GetKey(KeyCode.Q)) // Move up
    {
        movement += Vector3.up;
    }
    if (Input.GetKey(KeyCode.E)) // Move down
    {
        movement -= Vector3.up;
    }

    // Apply movement
    transform.Translate(movement * manualSpeed * Time.deltaTime, Space.World);
}


    void RotateCamera()
    {
        if (cameraNode != null)
        {
            // Rotate the camera node up and down with arrow keys
            if (Input.GetKey(KeyCode.RightArrow))
            {
                cameraNode.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                cameraNode.transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                cameraNode.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                cameraNode.transform.Rotate(Vector3.left, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void CheckGameCompletion()
    {
        if (targetObject == null || completionPanel == null) return;

        // Check if the sphere is within the interaction radius of the target object
        float distance = Vector3.Distance(transform.position, targetObject.position);
        if (distance <= interactionRadius)
        {
            // Stop the sphere's movement and controls
            isGameCompleted = true;
            helpPanel.SetActive(false);
            // Display completion panel
            completionPanel.SetActive(true);
            
            Debug.Log("Game Completed!");
        }
    }

}

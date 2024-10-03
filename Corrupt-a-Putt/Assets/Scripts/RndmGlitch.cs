using UnityEngine;
using UnityEngine.UI;

public class RndmGlitch : MonoBehaviour
{
    // Reference to the target GameObject where the scripts are located
    public GameObject targetGameObject;

    // References to the two scripts on the target GameObject
    public MonoBehaviour script1; // Drag the first script here
    public MonoBehaviour script2; // Drag the second script here

    // Reference to the UI Button
    public Button enableScriptButton;

    void Start()
    {
        // Disable both scripts initially
        if (script1 != null) script1.enabled = false;
        if (script2 != null) script2.enabled = false;

        // Attach the function to the button's onClick event
        if (enableScriptButton != null)
        {
            enableScriptButton.onClick.AddListener(EnableRandomScript);
        }
    }

    // Function to enable one of the two scripts randomly
    public void EnableRandomScript()
    {
        // Disable both scripts first
        if (script1 != null) script1.enabled = false;
        if (script2 != null) script2.enabled = false;

        // Randomly select one of the two scripts to enable
        int randomChoice = Random.Range(0, 2); // 0 or 1
        if (randomChoice == 0 && script1 != null)
        {
            script1.enabled = true;
        }
        else if (randomChoice == 1 && script2 != null)
        {
            script2.enabled = true;
        }
    }
}

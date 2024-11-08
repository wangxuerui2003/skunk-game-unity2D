using UnityEngine;
using UnityEngine.UI;

public class DialogScript : MonoBehaviour
{
    private Text textMesh;

    private void Start()
    {
        textMesh = GetComponentInChildren<Text>(); // Get the Text component
        // Ensure dialog cloud is hidden initially
        gameObject.SetActive(false);
    }

    // Function to show dialog with specified text
    public void ShowDialog(string message)
    {
        textMesh.text = message;           // Set the dialog text
        gameObject.SetActive(true);       // Show the dialog cloud
    }

    // Function to hide dialog
    public void HideDialog()
    {
        gameObject.SetActive(false);      // Hide the dialog cloud
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DialogControl : MonoBehaviour
{
    public GameObject dialogCloud;
    public Text textMesh;

    private void Start()
    {
        // Ensure dialog cloud is hidden initially
        dialogCloud.SetActive(false);
    }

    // Function to show dialog with specified text
    public void ShowDialog(string message)
    {
        textMesh.text = message;           // Set the dialog text
        dialogCloud.SetActive(true);       // Show the dialog cloud
    }

    // Function to hide dialog
    public void HideDialog()
    {
        dialogCloud.SetActive(false);      // Hide the dialog cloud
    }
}

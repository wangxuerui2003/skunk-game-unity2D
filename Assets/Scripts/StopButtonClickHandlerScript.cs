using UnityEngine;

public class StopButtonClickHandlerScript : MonoBehaviour
{
    public PlayerScript playerScript;

    void OnMouseDown()
    {
        // This method is called when the sprite is clicked
        OnSpriteClicked();
    }

    void OnSpriteClicked()
    {
        // Call the method in PlayerScript
        if (playerScript != null)
        {
            playerScript.OnStopButtonClicked();
        }
    }
}

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public GameObject rollDiceButtonObject;
    private Button rollDiceButton;
    public GameObject playerPrefab;
    public List<Sprite> avatarSprites;
    public List<string> playerNames;
    public List<Vector3> playerPositions;
    private List<GameObject> players = new List<GameObject>();
    public GameObject diceControl;
    private DiceScript diceScript;

    public int currentRound = 0;
    public int maxRound = 5;

    private string[] roundTexts = { "S", "N", "A", "K", "E" };
    private string currentRoundText = "";
    public GameObject roundTextCanvas;

    public GameObject winnerTextObject;

    public GameObject dialogCloudObject;
    private DialogScript dialogScript;

    void Start()
    {
        SpawnPlayers(5);
        diceScript = diceControl.GetComponent<DiceScript>();

        rollDiceButton = rollDiceButtonObject.GetComponent<Button>();

        dialogScript = dialogCloudObject.GetComponent<DialogScript>();
    }

    public void SpawnPlayers(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            // Instantiate the player at the specified position
            GameObject newPlayer = Instantiate(playerPrefab, playerPositions[i], Quaternion.identity);
            players.Add(newPlayer);

            // Assign the unique avatar for each player
            Transform avatarTransform = newPlayer.transform.Find("Avatar");
            SpriteRenderer spriteRenderer = avatarTransform.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = avatarSprites[i];
            newPlayer.GetComponent<PlayerScript>().playerName = playerNames[i];
        }
    }

    public void RollDice()
    {
        int playerLeft = 0;
        foreach (GameObject player in players)
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            if (!playerScript.roundGaveUp && !playerScript.skipRound)
            {
                playerLeft++;
            }
            playerScript.DeactivateStopButton();
        }

        if (playerLeft == 0)
        {
            Debug.Log("All players gave up or skipped the round!");
            NextRound();
            return;
        }

        dialogScript.HideDialog();

        StartCoroutine(RollDiceCoroutine());
        StartCoroutine(DisableButtonCoroutine(rollDiceButton, 2f));
    }

    IEnumerator DisableButtonCoroutine(Button button, float seconds)
    {
        button.interactable = false;
        yield return new WaitForSeconds(seconds);
        button.interactable = true;
    }

    IEnumerator RollDiceCoroutine()
    {
        yield return StartCoroutine(diceScript.RollDice());

        // After the dice roll coroutine completes, get the results
        int dice1Result = diceScript.dice1Result;
        int dice2Result = diceScript.dice2Result;

        foreach (GameObject player in players)
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            playerScript.ActToDiceResult(dice1Result, dice2Result);
        }

        if (dice1Result == 1 && dice2Result == 1)
        {
            dialogScript.ShowDialog("Snake Eyes! All active players lose current round points and skip the next round!");
            NextRound();
        }
        else if (dice1Result == 1 || dice2Result == 1)
        {
            dialogScript.ShowDialog("One of the dice is 1! All active players lose current round points!");
            NextRound();
        }
    }

    public void NextRound()
    {
        foreach (GameObject player in players)
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            playerScript.NextRound();
        }

        if (currentRound == 0)
        {
            currentRoundText += roundTexts[currentRound];
        }
        else
        {
            currentRoundText += " " + roundTexts[currentRound];
        }
        roundTextCanvas.GetComponentInChildren<TextMeshProUGUI>().text = currentRoundText;
        currentRound++;

        if (currentRound >= maxRound)
        {
            Debug.Log("Game Over!");
            DisplayWinnerAndBackToMainMenu();
        }
    }

    void DisplayWinnerAndBackToMainMenu()
    {
        int maxScore = 0;
        string winnerName = "";
        foreach (GameObject player in players)
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            Debug.Log(playerScript.playerName + ": " + playerScript.score);
            if (playerScript.score > maxScore)
            {
                maxScore = playerScript.score;
                winnerName = playerScript.playerName;
            }
        }
        string message;
        Debug.Log(winnerName);
        if (winnerName != "")
        {
            message = $"Winner: {winnerName}";
        }
        else
        {
            message = "It's a tie!";
        }
        StartCoroutine(DisplayWinnerAndBackToMainMenuCoroutine(message, 5f));
    }

    private IEnumerator DisplayWinnerAndBackToMainMenuCoroutine(string message, float duration)
    {
        rollDiceButton.interactable = false;
        Text textComponent = winnerTextObject.GetComponent<Text>();
        textComponent.text = message;
        winnerTextObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        winnerTextObject.SetActive(false);
        SceneManager.LoadScene("StartMenuScene");
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private GameObject avatarObject;
    private GameObject scoreTextObject;
    private GameObject disabledIcon;
    private GameObject stopButton;

    public string playerName = "";
    public int score = 0;
    private int currentRoundScore = 0;
    public bool roundGaveUp = false;
    public bool skipRound = false;
    private int roundSkipped = 0;

    void Start()
    {
        avatarObject = FindChildGameObject("Avatar");
        scoreTextObject = FindChildGameObject("ScoreText");
        disabledIcon = FindChildGameObject("DisabledIcon");
        stopButton = FindChildGameObject("StopButton");

        scoreTextObject.GetComponent<TextMeshPro>().text = score.ToString();
    }

    GameObject FindChildGameObject(string name)
    {
        Transform childTransform = gameObject.transform.Find(name);
        if (childTransform != null)
        {
            return childTransform.gameObject;
        }
        else
        {
            Debug.LogError($"Child GameObject '{name}' not found in '{gameObject.name}'");
            return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((roundGaveUp || skipRound) && !disabledIcon)
        {
            disabledIcon.SetActive(true);
        }
    }

    void UpdateScoreCurrent(int scoreChange)
    {
        currentRoundScore += scoreChange;
    }

    public void ActivateStopButton()
    {
        if (!roundGaveUp && !skipRound)
        {
            stopButton.SetActive(true);
        }
    }

    public void DeactivateStopButton()
    {
        stopButton.SetActive(false);
    }

    public void ActToDiceResult(int dice1Result, int dice2Result)
    {
        if (roundGaveUp || skipRound)
        {
            return;
        }

        if (dice1Result == 1 && dice2Result == 1)
        {
            SnakeEyes();
        }
        else if (dice1Result == 1 || dice2Result == 1)
        {
            GotOneInDice();
        }
        else
        {
            UpdateScoreCurrent(dice1Result + dice2Result);
            ActivateStopButton();
        }
    }
    public void OnStopButtonClicked()
    {
        StopRound();
    }

    public void StopRound()
    {
        // on stop button clicked
        roundGaveUp = true;
        disabledIcon.SetActive(true);
        stopButton.SetActive(false);
        DeactivateStopButton();
    }

    void GotOneInDice()
    {
        disabledIcon.SetActive(true);
        currentRoundScore = 0;
    }

    void SnakeEyes()
    {
        // on snake eyes
        skipRound = true;
        disabledIcon.SetActive(true);
        currentRoundScore = 0;
    }

    public void NextRound()
    {
        DeactivateStopButton();

        score += currentRoundScore;
        currentRoundScore = 0;

        roundGaveUp = false;

        if (skipRound)
        {
            // increment roundSkipped
            roundSkipped++;
            if (roundSkipped >= 2)
            {
                skipRound = false;
                roundSkipped = 0;
            }
        }

        if (!skipRound)
        {
            disabledIcon.SetActive(false);
        }
        else
        {
            disabledIcon.SetActive(true);
        }

        scoreTextObject.GetComponent<TextMeshPro>().text = score.ToString();
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceScript : MonoBehaviour
{
    public Sprite[] diceFaces;
    public SpriteRenderer dice1Renderer;
    public SpriteRenderer dice2Renderer;
    public float rollDuration = 2f;

    public int dice1Result { get; private set; }
    public int dice2Result { get; private set; }

    void Start()
    {
        SetRandomDiceFace(dice1Renderer);
        SetRandomDiceFace(dice2Renderer);
    }

    public IEnumerator RollDice()
    {
        yield return StartCoroutine(RollAndSaveResult());
    }

    private IEnumerator RollAndSaveResult()
    {
        float timeElapsed = 0f;

        while (timeElapsed < rollDuration)
        {
            SetRandomDiceFace(dice1Renderer);
            SetRandomDiceFace(dice2Renderer);
            yield return new WaitForSeconds(0.1f);
            timeElapsed += 0.1f;
        }

        dice1Result = SetRandomDiceFace(dice1Renderer);
        dice2Result = SetRandomDiceFace(dice2Renderer);
    }

    private int SetRandomDiceFace(SpriteRenderer diceRenderer)
    {
        int randomIndex = Random.Range(0, diceFaces.Length);
        diceRenderer.sprite = diceFaces[randomIndex];
        return randomIndex + 1;
    }
}

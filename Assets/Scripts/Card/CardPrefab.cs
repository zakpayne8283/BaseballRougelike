using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour
{
    public Card card;

    public GameObject gameManager;
    public GameObject playersManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Initialize with only card data. Used in upgrade store
    /// </summary>
    /// <param name="cardObj"></param>
    public void Initialize(Card cardObj)
    {
        card = cardObj;
    }

    public void Initialize(Card cardObj, GameObject gameManagerObj)
    {
        card = cardObj;
        gameManager = gameManagerObj;
    }

    public void PlayCard()
    {
        GameState gameStateScript = gameManager.GetComponent<GameState>();
        if (gameStateScript != null)
        {
            gameStateScript.PlayCard(card);
        }
    }

    public void updateCardTextFromMod(Modification cardMod)
    {
        Color textColor = Color.black;

        if (cardMod.improvement)
        {
            textColor = Color.green;
        }
        else
        {
            textColor = Color.red;
        }

        this.transform.Find("Card Name").GetComponent<TextMeshProUGUI>().color = textColor;
    }

    public void updateCardPreview(GameStateStruct initialState, GameStateStruct previewState)
    {
        string prefabHierarchyPath = "Card Background/Card Effect Preview";

        // Calculate outs added
        int outsAdded = previewState.outs - initialState.outs;

        // Update the outs added
        TMP_Text outsElement = this.transform.Find($"{prefabHierarchyPath}/Outs Added").GetComponent<TMP_Text>();
        outsElement.text = $"{outsAdded.ToString()} Outs";
        outsElement.color = (outsAdded > 0) ? Color.red : Color.black;

        // Calculate runs added
        int runsAdded = 0;

        if (!previewState.changeInning)
        {
            // Set runs based on top/bottom inning
            runsAdded = previewState.topInning 
                ? previewState.awayScore - initialState.awayScore 
                : previewState.homeScore - initialState.homeScore;
        }

        // Update the runs added
        TMP_Text runsElement = this.transform.Find($"{prefabHierarchyPath}/Runs Added").GetComponent<TMP_Text>();
        runsElement.text = $"{runsAdded.ToString()} Runs";
        runsElement.color = (runsAdded > 0) ? Color.green : Color.black;

        // Update the runners on base
        Image baseElement = this.transform.Find($"{prefabHierarchyPath}/First").GetComponent<Image>();
        baseElement.color = (previewState.runnerOnFirst) ? Color.red : Color.white;

        baseElement = this.transform.Find($"{prefabHierarchyPath}/Second").GetComponent<Image>();
        baseElement.color = (previewState.runnerOnSecond) ? Color.red : Color.white;

        baseElement = this.transform.Find($"{prefabHierarchyPath}/Third").GetComponent<Image>();
        baseElement.color = (previewState.runnerOnThird) ? Color.red : Color.white;
    }

    public void initializeUI()
    {
        // Assign card data to the UI
        gameObject.transform.Find("Card Name").GetComponent<TMP_Text>().text = card.name;

    }
}

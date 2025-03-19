using UnityEngine;

public class CardPrefab : MonoBehaviour
{
    public Card card;

    public GameObject gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

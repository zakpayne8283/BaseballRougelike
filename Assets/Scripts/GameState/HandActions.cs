using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandActions : MonoBehaviour
{
    [SerializeField] private Deck deck;

    [SerializeField] private Transform handArea;

    // Prefabs to use
    [SerializeField] private GameObject cardPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DrawCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCard()
    {
        Card drawnCard = deck.DrawCardFromDeck();

        GameObject cardObject = Instantiate(cardPrefab, handArea);

        // Assign card data to the UI
        cardObject.transform.Find("Card Name").GetComponent<TMP_Text>().text = drawnCard.cardName;
        //cardObject.transform.Find("cardArt").GetComponent<Image>().sprite = drawnCard.cardArt;

        // Optional: Adjust position or scale if needed
        //cardObject.transform.localScale = Vector3.one;
    }
}

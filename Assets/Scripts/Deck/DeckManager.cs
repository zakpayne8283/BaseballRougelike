using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField]private DeckObj deck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // randomize the new deck
        deck.randomize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // public DeckObj getDeck()
    // {
    //     return 
    // }
}

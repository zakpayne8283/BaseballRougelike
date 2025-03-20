using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private Sprite cardBack;
    [SerializeField] private List<Card> cards;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Randomize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card DrawCardFromDeck()
    {
        Card output = cards.First();
        cards.RemoveAt(0);
        return output;
    }

    public void Randomize()
    {
        System.Random random = new System.Random();
        cards = cards.OrderBy(card => random.Next()).ToList();
    }

    public Deck CopyToNewObject()
    {
        Deck copiedDeck = new Deck();
        copiedDeck.cards = new List<Card>();
        copiedDeck.cardBack = cardBack;

        foreach (Card card in cards)
        {
            copiedDeck.cards.Add(card);
        }

        return copiedDeck;
    }
}

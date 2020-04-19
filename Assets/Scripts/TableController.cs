using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public int numberOfPlayers = 2;
    public Transform userCardPile;
    public List<Transform> opponentCardPiles = new List<Transform>();
    public List<GameObject> cardPrefabs = new List<GameObject>();

    private int numberOfOpponents;

    private void Awake()
    {
        numberOfOpponents = numberOfPlayers - 1;

        //Shuffle the deck
        ShuffleCards();

        // Initialize the players and their decks
        var numberOfCardsPerUser = Convert.ToInt32(Math.Floor((decimal)cardPrefabs.Count / numberOfPlayers));

        for (int i = 0; i < numberOfPlayers; i++)
        {
            var numberToSkip = Convert.ToInt32(i * numberOfCardsPerUser);
            var cards = cardPrefabs.Skip(numberToSkip).Take(numberOfCardsPerUser).ToList();

            UsableCardPileController cardPileController;

            // Mine
            if (i == 0)
                cardPileController = userCardPile.GetComponent<UsableCardPileController>();
            else
                cardPileController = opponentCardPiles[i - 1].GetComponent<UsableCardPileController>();

            cardPileController.GenerateCards(cards);
        }
    }

    // Based on the Fisher-Yates Shuffle
    // TODO: Look this up
    private void ShuffleCards()
    {
        for (int i = 0; i < cardPrefabs.Count; i++)
        {
            var temp = cardPrefabs[i];
            int randomIndex = UnityEngine.Random.Range(i, cardPrefabs.Count);
            cardPrefabs[i] = cardPrefabs[randomIndex];
            cardPrefabs[randomIndex] = temp;
        }
    }
}

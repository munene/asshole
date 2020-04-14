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
        var numberOfCardsPerUser = Convert.ToInt32(Math.Floor((decimal)cardPrefabs.Count / 2));

        for (int i = 0; i < numberOfPlayers; i++)
        {
            var numberToSkip = Convert.ToInt32(i * numberOfCardsPerUser);
            var cards = cardPrefabs.Skip(numberToSkip).Take(numberOfCardsPerUser).ToList();

            // Mine
            if (i == 0)
            {
                PlaceCards(userCardPile, cards);
            }
        }
    }

    // Instantiate cards, slightly on top of each other
    private void PlaceCards(Transform pile, List<GameObject> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var cardGameObject = Instantiate(card, userCardPile.localPosition + new Vector3(0f, 0.001f * i, 0f), new Quaternion(0f, 0f, 180f, 0f));
            cardGameObject.transform.parent = pile.transform;
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

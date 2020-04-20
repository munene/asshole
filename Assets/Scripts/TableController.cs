using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public int NumberOfPlayers { get; set; } = 2;
    public List<Transform> cardPiles = new List<Transform>();
    public List<GameObject> cardPrefabs = new List<GameObject>();
    private Transform UserCardPile { get; set; }
    private List<Vector3> CameraPositions { get; set; } = new List<Vector3> 
    {
        new Vector3 (0, 1.5f, -1f),
        new Vector3 (-1f, 1.5f, 0),
        new Vector3 (0, 1.5f, 1f),
        new Vector3 (1f, 1.5f, 0)
    };
    private List<Vector3> CameraRotations { get; set; } = new List<Vector3>
    {
        new Vector3 (45f, 0, 0),
        new Vector3 (45f, 90f, 0),
        new Vector3 (45f, 180f, 0),
        new Vector3 (45f, -90f, 0)
    };

    private void Awake()
    {
        DisableAllBoxColliders();

        //Shuffle the deck
        ShuffleCards();

        // Initialize the players and their decks
        var numberOfCardsPerUser = Convert.ToInt32(Math.Floor((decimal)cardPrefabs.Count / NumberOfPlayers));

        for (int i = 0; i < NumberOfPlayers; i++)
        {
            var numberToSkip = Convert.ToInt32(i * numberOfCardsPerUser);
            var cards = cardPrefabs.Skip(numberToSkip).Take(numberOfCardsPerUser).ToList();

            UsableCardPileController cardPileController;

            cardPileController = cardPiles[i].GetComponent<UsableCardPileController>();
            cardPileController.GenerateCards(cards);
        }

        // Select the user's card pile
        System.Random rnd = new System.Random();
        int userCardPileIndex = rnd.Next(NumberOfPlayers);
        UserCardPile = cardPiles[userCardPileIndex];
        EnableBoxCollider(UserCardPile);

        // Pan camera to cardpile
        Camera.main.transform.position = CameraPositions[userCardPileIndex];
        Camera.main.transform.rotation = Quaternion.Euler(CameraRotations[userCardPileIndex]);
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

    private void DisableAllBoxColliders()
    {
        foreach (var cardPile in cardPiles)
        {
            var boxCollider = cardPile.GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }
    }

    private void EnableBoxCollider(Transform cardPile)
    {
        var boxCollider = cardPile.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public int numberOfPlayers = 2;
    public List<Transform> cardPiles = new List<Transform>();
    public List<GameObject> cardPrefabs = new List<GameObject>();
    private Transform UserCardPile { get; set; }
    private List<UsableCardPileController> UsableCardPiles { get; set; } = new List<UsableCardPileController>();

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
    public int NextTurnPosition { get; set; } = 0;
    public int UserCardPileIndex { get; set; } = 0;

    private void Awake()
    {
        DisableAllCardPiles();

        //Shuffle the deck
        ShuffleCards();

        // Initialize the players and their decks
        var numberOfCardsPerUser = Convert.ToInt32(Math.Floor((decimal)cardPrefabs.Count / numberOfPlayers));

        for (int i = 0; i < numberOfPlayers; i++)
        {
            var numberToSkip = Convert.ToInt32(i * numberOfCardsPerUser);
            var cards = cardPrefabs.Skip(numberToSkip).Take(numberOfCardsPerUser).ToList();

            UsableCardPileController cardPileController;

            cardPileController = cardPiles[i].GetComponent<UsableCardPileController>();
            cardPileController.GenerateCards(cards);
            UsableCardPiles.Add(cardPileController);
        }

        // Select the user's card pile
        System.Random rnd = new System.Random();
        UserCardPileIndex = rnd.Next(numberOfPlayers);
        UserCardPile = cardPiles[UserCardPileIndex];
        EnableUserCardPile(UserCardPile);

        // Pan camera to cardpile
        Camera.main.transform.position = CameraPositions[UserCardPileIndex];
        Camera.main.transform.rotation = Quaternion.Euler(CameraRotations[UserCardPileIndex]);

        // Start turn with the first person
        PlayNextTurn();
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

    private void DisableAllCardPiles()
    {
        foreach (var cardPile in cardPiles)
        {
            var boxCollider = cardPile.GetComponent<BoxCollider>();
            boxCollider.enabled = false;
            cardPile.tag = "Untagged";
        }
    }

    private void EnableUserCardPile(Transform cardPile)
    {
        var boxCollider = cardPile.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        cardPile.tag = "Player Card Pile";
    }

    public void PlayNextTurn()
    {
        if (NextTurnPosition > numberOfPlayers - 1)
        {
            NextTurnPosition = 0;
        }

        if (cardPiles[NextTurnPosition].gameObject.tag == "Player Card Pile")
        {
            // Let the player know that it's their turn;
        }
        else
        {
            // Probably let the player know who's turn it is.
            StartCoroutine(PlayAiTurn());
        }
    }

    private IEnumerator PlayAiTurn()
    {
        var secondsToWait = UnityEngine.Random.Range(1f, 2f);
        yield return new WaitForSeconds(secondsToWait);
        var cardPileController = UsableCardPiles[NextTurnPosition];
        cardPileController.DropCard();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UsableCardPileController : MonoBehaviour
{
    public Transform discardPile;
    private Stack<GameObject> Cards { get; set; } = new Stack<GameObject>();
    private TableController TableController { get; set; }
    private GameObject playerMessageText;

    private void Awake()
    {
        playerMessageText = GameObject.Find("Player Message");
        var table = GameObject.Find("Table");
        TableController = table.GetComponent<TableController>();
    }

    private void OnMouseDown()
    {
        // Will only work when the box collider is enabled, which ideally happens only once

        // If it's not the user's turn, ignore
        if (TableController.UserCardPileIndex != TableController.NextTurnPosition) return;

        DropCard();
    }

    // Instantiate cards, slightly on top of each other
    internal void GenerateCards(List<GameObject> cards, bool shouldInitCards)
    {
        var isUserCardPile = gameObject.tag == "Player Card Pile";

        // Disable the player's cardpile collider to allow cards to go through it
        if (isUserCardPile)
        {
            TableController.SetPlayerCardPileCollider(false);
        }

        /*
         * Get the card pile transform because we'll edit the original version later,
         * and rely on the physics of gravity for the original to fix itself
         */
        var cardPileTransform = gameObject.transform;

        //Lift the current card pile depending on the number of cards provided
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (0.001f * cards.Count), gameObject.transform.position.z);

        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            GameObject cardGameObject = card;

            if (shouldInitCards)
            {
                cardGameObject = Instantiate(card, gameObject.transform.position + new Vector3(0f, 0.001f * i, 0f),
                    Quaternion.Euler(new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 180f)));
            }
            else
            {
                cardGameObject.transform.position = cardPileTransform.position;
                cardGameObject.transform.rotation = Quaternion.Euler(new Vector3(cardPileTransform.eulerAngles.x, cardPileTransform.eulerAngles.y, 180f));
            }

            cardGameObject.transform.SetParent(cardPileTransform);

            Cards.Push(cardGameObject);
        }

        // Re-enable the player's card pile collider to detect mouse clicks
        if (isUserCardPile)
        {
            TableController.SetPlayerCardPileCollider(true);
        }
    }

    internal void DropCard()
    {
        TableController.NextTurnPosition++;

        // For now, if the user has no cards, skip the turn
        if (!Cards.Any()) return;

        var currentCard = Cards.Pop();
        var discardPileController = discardPile.GetComponent<DiscardPileController>();
        discardPileController.DropCard(currentCard);
        TableController.PlayNextTurn();
    }

    internal void UpdateDiscardPileClaimability(bool isPileClaimable)
    {
        if (isPileClaimable)
        {
            if (gameObject.tag != "Player Card Pile")
                StartCoroutine(ClaimCardsAutomatically());
            else
                ShowClaimabilityHint();
        }
        else
        {
            // Stop AIs from continuing with the process of claiming the cards if play is continued
            if (gameObject.tag != "Player Card Pile")
                StopCoroutine(ClaimCardsAutomatically());
        }
    }

    // To be used by AI only!
    private IEnumerator ClaimCardsAutomatically()
    {
        var secondsToWait = UnityEngine.Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(secondsToWait);
        var discardPileController = FindObjectOfType<DiscardPileController>();
        discardPileController.ClaimCards(this);
    }

    internal void ClaimCards(List<GameObject> cards)
    {
        if (Cards.Any())
        {
            cards.AddRange(Cards);
        }

        Cards.Clear();
        GenerateCards(cards, false);
    }

    private void ShowClaimabilityHint()
    {
        ShowText("Claim the cards!");
    }

    internal void ShowText(string text, UsableCardPileController context = null)
    {
        var textComponent = playerMessageText.GetComponent<TextMeshPro>();
        textComponent.text = text;
    }
}

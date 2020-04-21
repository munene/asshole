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
    internal void GenerateCards(List<GameObject> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var cardGameObject = Instantiate(card, gameObject.transform.position + new Vector3(0f, 0.001f * i, 0f),
                Quaternion.Euler(new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 180f)));
            cardGameObject.transform.SetParent(gameObject.transform);

            Cards.Push(cardGameObject);
        }
    }

    internal void DropCard()
    {
        TableController.NextTurnPosition++;

        // For now, if the user has no cards, skip the turn
        if (!Cards.Any()) return;

        var currentCard = Cards.Pop();
        Destroy(currentCard);
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
                SendClaimabilityHint();
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
        var secondsToWait = UnityEngine.Random.Range(2f, 3f);
        yield return new WaitForSeconds(secondsToWait);
        AttemptToClaimCards();
    }

    internal void ClaimCards(List<GameObject> cards)
    {
        if (Cards.Any())
        {
            cards.AddRange(Cards);
        }

        Cards.Clear();
        GenerateCards(cards);
    }
    private void AttemptToClaimCards()
    {
        var discardPileController = FindObjectOfType<DiscardPileController>();
        discardPileController.ClaimCards(this);
    }

    private void SendClaimabilityHint()
    {
        ShowText("Claim the cards!");
    }

    internal void ShowText(string text, UsableCardPileController context = null)
    {
        var textComponent = playerMessageText.GetComponent<TextMeshPro>();
        textComponent.text = text;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardPileController : MonoBehaviour
{
    public List<GameObject> Cards { get; set; } = new List<GameObject>();
    private bool IsClaimable { get; set; } = false;

    private void OnMouseDown()
    {
        //if (!IsClaimable) return;
    }

    public void DropCard(GameObject card)
    {
        IsClaimable = false;

        var xPosition = gameObject.transform.position.x + UnityEngine.Random.Range(-0.03f, 0.03f);
        var yRotation = gameObject.transform.eulerAngles.y + UnityEngine.Random.Range(-35, 23);
        var zPosition = gameObject.transform.position.z + UnityEngine.Random.Range(-0.01f, 0.01f);
        var cardPosition = new Vector3(xPosition, gameObject.transform.position.y + 0.1f, zPosition);

        var cardGameObject = Instantiate(card, cardPosition, Quaternion.Euler(new Vector3(gameObject.transform.eulerAngles.x, yRotation, 0)));
        cardGameObject.transform.parent = gameObject.transform;
        Cards.Add(cardGameObject);

        // Calculate if the pile can be claimed
        CalculatePileClaimability();
    }

    private void CalculatePileClaimability()
    {
        var subpile = Cards.Skip(Math.Max(0, Cards.Count - 3)).ToList();

        // Treat this like a stack. The top card was dropped last
        var topCard = subpile.Last();
        var lastCard = subpile.First();

        var firstCardController = topCard.GetComponent<CardController>();
        var lastCardController = lastCard.GetComponent<CardController>();

        /*
         * Start by checking the first and last card comparing their values.
         * In a subpile with only two cards, this is all we need.
         * For a subpile with more than two cards, the first and last cards will be the first
         * and third card.
         * In this case, if the check comes out negative, we'll need to do one more check to compare
         * the first and second cards
         */
        IsClaimable = CheckCardSummations(firstCardController, lastCardController);

        if (!IsClaimable && Cards.Count > 2)
        {
            var secondCard = subpile[1];
            var secondCardController = secondCard.GetComponent<CardController>();
            IsClaimable = CheckCardSummations(firstCardController, secondCardController);
        }

        print(IsClaimable);
    }

    // Check if the two cards are number AND their value == 10, or that the top card is the ten of diamonds
    private bool CheckCardSummations(CardController topCard, CardController otherCard)
    {
        return ((topCard.value + otherCard.value == 10) && topCard.type == CardType.Number && otherCard.type == CardType.Number) ||
            (topCard.value == 10 && topCard.suit == CardSuit.Diamonds);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableCardPileController : MonoBehaviour
{
    public Transform discardPile;
    private Stack<GameObject> Cards { get; set; } = new Stack<GameObject>();
    private TableController TableController { get; set; }

    private void Awake()
    {
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
    public void GenerateCards(List<GameObject> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var cardGameObject = Instantiate(card, gameObject.transform.position + new Vector3(0f, 0.001f * i, 0f),
                Quaternion.Euler(new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 180f)));
            cardGameObject.transform.parent = gameObject.transform;

            Cards.Push(cardGameObject);
        }
    }

    public void DropCard()
    {
        var currentCard = Cards.Pop();
        Destroy(currentCard);
        var discardPileController = discardPile.GetComponent<DiscardPileController>();
        discardPileController.DropCard(currentCard);
        TableController.NextTurnPosition++;
        TableController.PlayNextTurn();
    }
}

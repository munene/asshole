using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableCardPileController : MonoBehaviour
{
    public Transform discardPile;
    private Stack<GameObject> Cards { get; set; } = new Stack<GameObject>();

    private void OnMouseDown()
    {
        var currentCard = Cards.Pop();
        Destroy(currentCard);
        var discardPileController = discardPile.GetComponent<DiscardPileController>();
        discardPileController.DropCard(currentCard);
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
}

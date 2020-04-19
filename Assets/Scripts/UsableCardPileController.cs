using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableCardPileController : MonoBehaviour
{
    public Transform discardPile;
    private List<GameObject> Cards { get; set; }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit) && (hit.transform.gameObject.tag == "Player Card Pile" || hit.transform.parent.gameObject.tag == "Player Card Pile"))
    //        {
    //            print("tereyyhrgjm");
    //        }
    //    }
    //}

    private void OnMouseDown()
    {

    }

    // Instantiate cards, slightly on top of each other
    public void GenerateCards(List<GameObject> cards)
    {
        Cards = cards;

        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var cardGameObject = Instantiate(card, gameObject.transform.position + new Vector3(0f, 0.001f * i, 0f),
                new Quaternion(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 180f, 0f));
            cardGameObject.transform.parent = gameObject.transform;
        }
    }
}

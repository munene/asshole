using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPileController : MonoBehaviour
{
    public List<GameObject> Cards { get; set; } = new List<GameObject>();

    public void DropCard(GameObject card)
    {
        var xPosition = gameObject.transform.position.x + Random.Range(-0.03f, 0.03f);
        var yRotation = gameObject.transform.eulerAngles.y + Random.Range(-35, 23);
        var zPosition = gameObject.transform.position.z + Random.Range(-0.01f, 0.01f);
        var cardPosition = new Vector3(xPosition, gameObject.transform.position.y + 0.1f, zPosition);

        var cardGameObject = Instantiate(card, cardPosition, Quaternion.Euler(new Vector3(gameObject.transform.eulerAngles.x, yRotation, 0)));
        cardGameObject.transform.parent = gameObject.transform;
        Cards.Add(card);

        // Calculate if the pile can be claimed
        CalculatePileClaimability();
    }

    private void CalculatePileClaimability()
    {

    }
}

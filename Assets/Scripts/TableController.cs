using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public int numberOfPlayers = 2;
    public Transform userCardPilePosition;
    public List<Transform> opponentCardPilePositions = new List<Transform>();
    public List<GameObject> cardPrefabs = new List<GameObject>();
    public GameObject smallPileOfCardsPrefab;
    public GameObject largePileOfCardsPrefab;

    private int numberOfOpponents;

    private void Awake()
    {
        numberOfOpponents = numberOfPlayers - 1;
    }
}

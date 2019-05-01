using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {

    private static ResourcesManager instance;

    [SerializeField]
    private List<Card> cards;

    
    private Dictionary<int, Card> dicCards;

    private Dictionary<int, Card> choosedCards;
    private Dictionary<int, Card> availableCards;

    private void Awake()
    {
        instance = this;
        OnCreateResource();
    }

    public static ResourcesManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        choosedCards = new Dictionary<int, Card>();
        availableCards = dicCards;
    }

    void OnCreateResource()
    {
        if (cards == null || cards.Count <= 0)
            return;
        if (dicCards == null)
            dicCards = new Dictionary<int, Card>();
        Card card;
        for (int i = 0, len = cards.Count; i < len; i++)
        {
            card = cards[i];
            dicCards[i] = card;
        }
    }

    public Card GetCard(int index)
    {
        if (index < 0 || dicCards == null || dicCards.Count <= 0)
            return null;
        Card card = null;
        if(!dicCards.TryGetValue(index, out card))
        {
            return null;
        }
        return card;
    }

    public Card GetCardRandomly()
    {
        List<int> keys = new List<int>(availableCards.Keys);
        if (keys == null || keys.Count <= 0)
            return null;
        int random = keys[Random.Range(0, keys.Count)];
        Card card = null;
        if(!availableCards.TryGetValue(random, out card))
        {
            return null;
        }
        choosedCards[random] = card;
        availableCards.Remove(random);
        return card;
    }

    public Card GetAvailableCard()
    {
        if (availableCards == null || availableCards.Count <= 0)
            return null;
        List<int> keys = availableCards.Keys.ToList();
        if (keys == null || keys.Count <= 0)
            return null;
        int firstKey = keys[0];
        Card firstCard = null;
        if (!availableCards.TryGetValue(firstKey, out firstCard))
        {
            return null;
        }
        availableCards.Remove(firstKey);
        return firstCard;
    }
}

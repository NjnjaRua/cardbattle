using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCardManager : MonoBehaviour {
    private static ComputerCardManager instance;
    [SerializeField]
    private List<GameObject> listCards;
    private Dictionary<int, Card> dicCards;

    private int currentSlot;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnReset();
        currentSlot = -1;
    }

    public static ComputerCardManager GetInstance()
    {
        return instance;
    }

    private void OnReset()
    {
        dicCards = new Dictionary<int, Card>();
        if (listCards == null || listCards.Count <= 0)
            return;
        for (int i = 0, len = listCards.Count; i < len; i++)
        {
            if (listCards[i] != null)
                listCards[i].gameObject.SetActive(false);
        }
    }

    public void OnCreateComCard(int cardIndex)
    {
        if (cardIndex < 0 || listCards == null || listCards.Count <= 0)
            return;
        if (IsFullSlot())
        {
            EventManager.TriggerEvent(ConstantManager.EVENT_CRATE_COMPUTER_CARD);
            return;
        }
        if (dicCards == null)
            dicCards = new Dictionary<int, Card>();
        currentSlot += 1;
        GameObject cardDisplay = null;
        int index = -1;
        for (int i = 0, len = listCards.Count; i < len; i++)
        {
            if (i == currentSlot)
            {
                index = i;
                cardDisplay = listCards[i];
                break;
            }
        }
        if (cardDisplay != null)
        {
            cardDisplay.SetActive(true);
            Card card = ResourcesManager.GetInstance().GetCardRandomly();
            if (card != null)
            {
                dicCards[currentSlot] = card;
            }
        }
    }

    public bool IsFullSlot()
    {
        return (currentSlot >= ConstantManager.MAX_FIGHTING_CARD - 1);
    }

    private GameObject GetCompCard(int index)
    {
        if (index < 0 || index >= ConstantManager.MAX_FIGHTING_CARD || listCards == null || listCards.Count <= 0)
            return null;
        GameObject gObj = null;
        for (int i = 0, len = listCards.Count; i < len; i++)
        {
            gObj = listCards[i];
            if (gObj == null)
                continue;
            if (i == index)
            {
                return gObj;
            }
        }
        return null;
    }

    public Vector3 GetPosCompCard()
    {
        GameObject gObj = GetCompCard((currentSlot >= 0) ? currentSlot : 0);
        if (gObj == null)
        {
            return Vector3.zero;
        }
        return gObj.transform.position;
    }

    public Vector3 GetNextCompPlayerCard()
    {
        GameObject gObj = GetCompCard(currentSlot + 1);
        if (gObj == null)
        {
            return Vector3.zero;
        }
        return gObj.transform.position;
    }
}

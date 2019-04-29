using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardManager : MonoBehaviour
{
    private static PlayerCardManager instance;

    [SerializeField]
    private Text hint;

    [SerializeField]
    private List<CardDisplay> listCards;

    private int currentSlot;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnReset();
        currentSlot = -1;
        hint.gameObject.SetActive(true);
    }

    public static PlayerCardManager GetInstance()
    {
        return instance;
    }

    private void OnReset()
    {
        if (listCards == null || listCards.Count <= 0)
            return;
        for(int i = 0, len=listCards.Count;i < len;i++)
        {
            if (listCards[i] != null)
                listCards[i].gameObject.SetActive(false);
        }
    }

    public void OnCreatePlayerCard(int cardIndex)
    {
        if (currentSlot >= ConstantManager.MAX_FIGHTING_CARD || listCards == null || listCards.Count <= 0)
            return;
        hint.gameObject.SetActive(false);
        currentSlot += 1;
        CardDisplay cardDisplay = null;
        int index = -1;
        for (int i = 0, len = listCards.Count; i < len; i++)
        {
            if(i == currentSlot)
            {
                index = i;
                cardDisplay = listCards[i];
                break;
            }
        }
        if (cardDisplay != null)
        {
            cardDisplay.gameObject.SetActive(true);
            Card card = ResourcesManager.GetInstance().GetCardRandomly();
            if(card != null)
            {
                cardDisplay.OnUpdateCard(card);
            }
        }
    }
}

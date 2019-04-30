using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerCardManager : MonoBehaviour
{
    private static PlayerCardManager instance;

    [SerializeField]
    private Text hint;

    [SerializeField]
    private List<CardDisplay> listCards;

    private int currentSlot;

    private Dictionary<int, Card> dicCards;
    private UnityAction createCompututerCardAction;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        createCompututerCardAction = new UnityAction(CreateComPuterCards);
        OnReset();
        currentSlot = -1;
        hint.gameObject.SetActive(true);
    }
    void OnEnable()
    {
        if (createCompututerCardAction == null)
            createCompututerCardAction = new UnityAction(CreateComPuterCards);
        EventManager.StartListening(ConstantManager.EVENT_CRATE_COMPUTER_CARD, CreateComPuterCards);
    }

    void OnDisable()
    {
        EventManager.StopListening(ConstantManager.EVENT_CRATE_COMPUTER_CARD, CreateComPuterCards);
    }
    
    private void CreateComPuterCards()
    {
        if(GamePlayScript.GetInstance() != null)
        {
            GamePlayScript.GetInstance().OnCreateCompCards();
        }
    }

    public static PlayerCardManager GetInstance()
    {
        return instance;
    }

    private void OnReset()
    {
        dicCards = new Dictionary<int, Card>();
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
        if (cardIndex < 0 || listCards == null || listCards.Count <= 0)
            return;
        if (dicCards == null)
            dicCards = new Dictionary<int, Card>();
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
                cardDisplay.OnUpdateCard(card, cardIndex);
                dicCards[currentSlot] = card;
            }
        }
        if (IsFullSlot())        {
            EventManager.TriggerEvent(ConstantManager.EVENT_CRATE_COMPUTER_CARD);        }
    }

    public Dictionary<int,Card> GetPlayerCards()
    {
        if (dicCards == null || dicCards.Count <= 0)
            return null;
        return dicCards;
    }

    private GameObject GetPlayerCard(int index)
    {
        if (index < 0 || index >= ConstantManager.MAX_FIGHTING_CARD || listCards == null || listCards.Count <= 0)
            return null;
        CardDisplay cardDisplay = null;
        GameObject gObj = null;
        for(int i = 0, len = listCards.Count; i < len;i++)
        {
            cardDisplay = listCards[i];
            if (cardDisplay == null)
                continue;
            if(i == index)
            {
                gObj = cardDisplay.gameObject;
                break;
            }
        }
        return gObj;
    }

    public Vector3 GetPosPlayerCard()
    {
        GameObject gObj = GetPlayerCard((currentSlot >= 0) ? currentSlot : 0);
        if(gObj == null)
        {
            return Vector3.zero;
        }
        return gObj.transform.position;
    }

    public Vector3 GetNextPosPlayerCard()
    {
        GameObject gObj = GetPlayerCard(currentSlot + 1);
        if (gObj == null)
        {
            return Vector3.zero;
        }
        return gObj.transform.position;
    }

    public bool IsFullSlot()
    {
        return (currentSlot >= ConstantManager.MAX_FIGHTING_CARD - 1);
    }
}

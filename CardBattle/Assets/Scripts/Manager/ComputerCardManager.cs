using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComputerCardManager : MonoBehaviour {
    private static ComputerCardManager instance;
    [SerializeField]
    private List<CardDisplay> listCards;
    private int currentSlot;
    private Dictionary<int, Card> dicCards;

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
            return;
        if (dicCards == null)
            dicCards = new Dictionary<int, Card>();
        currentSlot += 1;
        CardDisplay cardDisplay = null;
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
            cardDisplay.gameObject.SetActive(true);
            Card card = ResourcesManager.GetInstance().GetAvailableCard();
            Debug.Log("<color=green>card </color>" + card.name + " ; " + card.attackPoint + " ; " + card.lifePoint);
            cardDisplay.OnUpdateCard(true, card, cardIndex);
            dicCards[currentSlot] = card;
        }
        if (IsFullSlot())
            OnShowFirstCard();
    }

    public bool IsFullSlot()
    {
        return (currentSlot >= ConstantManager.MAX_FIGHTING_CARD - 1);
    }

    private GameObject GetCompCard(int index)
    {
        if (index < 0 || index >= ConstantManager.MAX_FIGHTING_CARD || listCards == null || listCards.Count <= 0)
            return null;
        CardDisplay cardDisplay = null;
        GameObject gObj = null;
        for (int i = 0, len = listCards.Count; i < len; i++)
        {
            cardDisplay = listCards[i];
            if (cardDisplay == null)
                continue;
            if (i == index)
            {
                gObj = cardDisplay.gameObject;
                break;
            }
        }
        return gObj;
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

    private CardDisplay GetFirstEnalbeCard()
    {
        if (listCards == null || listCards.Count <= 0)
            return null;
        CardDisplay cardDisplay = null;
        for(int i = 0, len = listCards.Count;i < len;i++)
        {
            cardDisplay = listCards[i];
            if (cardDisplay == null || !cardDisplay.gameObject.activeSelf)
                continue;
            return cardDisplay;
        }
        return null;
    }

    private void OnShowFirstCard()
    {
        Debug.Log("<color=blue>OnShowFirstCard</color>");
        if (dicCards == null || dicCards.Count <= 0)
            return;
        CardDisplay firstCard = GetFirstEnalbeCard();
        if (firstCard == null)
            return;
        Sequence seq = Util.PlayRotation(firstCard.gameObject, 0.15f);
        seq.OnComplete(() =>
        {
            Debug.Log("ROTATION OK");
            firstCard.OnShowBackSide(false);
            EventManager.TriggerEvent(ConstantManager.EVENT_ENALBE_BATTLE);
            Sequence seqMove = DOTween.Sequence();
            CardDisplay cardDisplay = Battle.GetInstance().GetComputerCard();
            seqMove.Append(firstCard.gameObject.transform.DOMove(cardDisplay.transform.position, 2f));
            seqMove.OnComplete(() =>
            {
                firstCard.gameObject.SetActive(false);
                if (SoundManager.getInstance())
                    SoundManager.getInstance().PlaySound(SoundId.FLY);
                List<int> keys = dicCards.Keys.ToList();
                if(keys != null)
                {
                    int firstkey = keys[0];
                    cardDisplay.OnUpdateCard(false, dicCards[firstkey], firstkey);
                }
                
            });
        });
    }
}

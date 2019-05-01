using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct PostCardInvisible
{
    public int cardIndex;
    public Vector3 position;
}

public class GamePlayScript : MonoBehaviour {

    [SerializeField]
    private List<PostCardInvisible> listPostCardInvisible;

    [SerializeField]
    private Dictionary<int, Vector3> dicPostCardInvisible;
    
    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private GameObject rootTrans;

    private static GamePlayScript instance;
    private bool isCompleteShowCard;
    private bool isCompleteCreatePlayerCard = true;

    private Dictionary<int,GameObject> dicInvisibleCards;

    public bool IsCompleteShowCard
    {
        get { return isCompleteShowCard; }
        set { isCompleteShowCard = value; }
    }

    public bool IsCompleteCreatePlayerCard
    {
        get { return isCompleteCreatePlayerCard; }
        set { isCompleteCreatePlayerCard = value; }
    }


    private void Awake()
    {
        instance = this;
        OnCreateResource();
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(OnDelayCreateNewCard());
    }

    public static GamePlayScript GetInstance()
    {
        return instance;
    }

    void OnCreateResource()
    {
        if (listPostCardInvisible == null || listPostCardInvisible.Count <= 0)
            return;
        PostCardInvisible postCard;
        if (dicPostCardInvisible == null)
            dicPostCardInvisible = new Dictionary<int, Vector3>();
        for (int i = 0, len = listPostCardInvisible.Count; i < len; i++)
        {
            postCard = listPostCardInvisible[i];
            dicPostCardInvisible[postCard.cardIndex] = postCard.position;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
        if (SoundManager.getInstance())
            SoundManager.getInstance().PlaySound(SoundId.DIE);
        UnityEditor.EditorApplication.isPlaying = false;
#else
            
        if (SoundManager.getInstance())
            SoundManager.getInstance().PlaySound(SoundId.DIE);
         Application.Quit();
#endif

        }
    }

    private IEnumerator OnDelayCreateNewCard()
    {
        isCompleteShowCard = false;
        if (dicInvisibleCards == null)
            dicInvisibleCards = new Dictionary<int, GameObject>();
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < ConstantManager.MAX_CARD_INITIALIZE; i++)
        {
            OnCloneAndMoveCard(cardPrefab, i);
            yield return new WaitForSeconds(0.5f);
        }
        isCompleteShowCard = true;
    }
	
	private void OnCloneAndMoveCard(GameObject prefab, int index)
    {
        if (prefab == null)
        {
            Debug.LogError("Missing Prefab");
            return;
        }

        if(dicPostCardInvisible == null)
        {
            Debug.LogError("Missing dicPostCardInvisible");
            return;
        }
        
        //Create new card
        GameObject cardClone = Instantiate(prefab, Vector3.zero, Quaternion.identity, rootTrans.transform) as GameObject;
        CardInVisibleScript cardInvisible = cardClone.GetComponent<CardInVisibleScript>();
        if (cardInvisible == null)
        {
            Debug.LogError("Missing CardInvisibleScript");
            return;
        }
        dicInvisibleCards[index] = cardClone;
        cardInvisible.OnUpdateCardVisible(index);        

        Button btnCardInvisible = cardInvisible.GetComponent<Button>();
        if (btnCardInvisible == null)
        {
            Debug.LogError("Missing btnCardInvisible");
            return;
        }
        btnCardInvisible.interactable = false;

        Sequence seq = DOTween.Sequence();
        Vector3 localPosition = Vector3.zero;
        localPosition.x = -(Screen.width / 2);
        localPosition.y = 0;
        localPosition.z = 0;
        cardClone.transform.localPosition = localPosition;
        Vector3 localScale = prefab.transform.localScale;
        cardClone.transform.localScale = Vector3.zero;

        seq.Append(cardClone.transform.DOScale(new Vector3(0.3f, 0.5f, 0.3f), 0.3f));
        Vector3 newPostCard = dicPostCardInvisible[index];
        Tween posTween = cardClone.transform.DOLocalMove(newPostCard, 0.5f);
        posTween.OnComplete(() =>
        {
            btnCardInvisible.interactable = true;
            if (SoundManager.getInstance())
                SoundManager.getInstance().PlaySound(SoundId.FLY);
            if(index == ConstantManager.MAX_CARD_INITIALIZE - 1)
            {
                if (SoundManager.getInstance())
                    SoundManager.getInstance().PlaySound(SoundId.NORMAL, true);
            }
        });
        seq.Append(posTween);
        seq.Append(cardClone.transform.DOScale(new Vector3(localScale.x, localScale.y, localScale.z), 0.2f));
    }

    public void OnRoll(int index)
    {
        isCompleteCreatePlayerCard = false;
        GameObject gObj = GetInvisibleCard(index);
        Sequence seq = PlayRotation(gObj, 0.2f);
        Vector3 playerCardPos = PlayerCardManager.GetInstance().GetNextPosPlayerCard();
        seq.Append(gObj.transform.DOMove(playerCardPos, 0.2f));
        seq.OnComplete(() =>
        {
            CardInVisibleScript cardInvisible = gObj.GetComponent<CardInVisibleScript>();
            if (cardInvisible != null)
                cardInvisible.isHide = true;
            gObj.SetActive(false);
            if (SoundManager.getInstance())
                SoundManager.getInstance().PlaySound(SoundId.TOUCH);
            if (PlayerCardManager.GetInstance() != null)
                PlayerCardManager.GetInstance().OnCreatePlayerCard(index);
            isCompleteCreatePlayerCard = true;
        });
    }

    public GameObject GetInvisibleCard(int index)
    {
        if (index < 0 || dicInvisibleCards == null || dicInvisibleCards.Count <= 0)
            return null;
        GameObject gObj = null;
        if (!dicInvisibleCards.TryGetValue(index, out gObj))
        {
            return null;
        }
        return gObj;
    }

    public void HideInvibleCard(int index)
    {
        GameObject gObj = GetInvisibleCard(index);
        if(gObj != null)
            gObj.SetActive(false);
    }

    private Sequence PlayRotation(GameObject gObj, float durationRotate)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(gObj.transform.DOLocalRotate(new Vector3(0, 45f, 0), durationRotate));
        seq.Append(gObj.transform.DOLocalRotate(new Vector3(0, 90f, 0), durationRotate));
        seq.Append(gObj.transform.DOLocalRotate(new Vector3(0, 180f, 0), durationRotate));
        seq.Append(gObj.transform.DOLocalRotate(new Vector3(0, 270f, 0), durationRotate));
        seq.Append(gObj.transform.DOLocalRotate(new Vector3(0, 0, 0), durationRotate));
        return seq;
    }

    public Dictionary<int, GameObject> GetRemainInvisibleCards()
    {
        if (dicInvisibleCards == null || dicInvisibleCards.Count <= 0)
            return null;
        Dictionary<int, GameObject> dicResult = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, GameObject> data in dicInvisibleCards)
        {
            CardInVisibleScript card = data.Value.GetComponent<CardInVisibleScript>();
            if (!card.isHide)
            {
                dicResult[data.Key] = data.Value;
            }
        }
        return dicResult;
    }

    public void OnCreateCompCards()
    {
        StartCoroutine(DelayCreateCompCards());
    }

    private IEnumerator DelayCreateCompCards()
    {
        yield return new WaitForSeconds(0.1f);
        Dictionary<int, GameObject> dicInvisbileCards = GetRemainInvisibleCards();
        if (dicInvisbileCards != null && dicInvisbileCards.Count > 0)
        {
            CardCompare cardCompare = new CardCompare();
            List<int> keys = dicInvisbileCards.Keys.ToList();
            keys.Sort(cardCompare);
            foreach(int key in keys)
            {
                GameObject gObj = null;
                if(!dicInvisbileCards.TryGetValue(key, out gObj))
                {
                    continue;
                }
                Sequence seq = PlayRotation(gObj, 0.1f);
                Vector3 compCard = ComputerCardManager.GetInstance().GetPosCompCard();
                seq.Append(gObj.transform.DOMove(compCard, 0.3f));
                seq.OnComplete(() =>
                {
                    gObj.SetActive(false);
                    if (SoundManager.getInstance())
                        SoundManager.getInstance().PlaySound(SoundId.FLY);
                    if (ComputerCardManager.GetInstance() != null)
                        ComputerCardManager.GetInstance().OnCreateComCard(key);
                });
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}

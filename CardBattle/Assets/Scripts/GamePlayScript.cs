using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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


    private void Awake()
    {
        OnCreateResource();
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(OnDelayCreateNewCard());

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
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < ConstantManager.MAX_CARD_INITIALIZE; i++)
        {
            OnCloneAndMoveCard(cardPrefab, i);
            yield return new WaitForSeconds(0.5f);
        }
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
}

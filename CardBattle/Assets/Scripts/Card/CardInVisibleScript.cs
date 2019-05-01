using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInVisibleScript : MonoBehaviour {
    private int cardIndex;
    public bool isHide;

    public void OnUpdateCardVisible(int _cardIndex)
    {
        if (_cardIndex < 0)
            return;
        isHide = false;
        this.cardIndex = _cardIndex;
    }

	public void OnChooseCard()
    {        
        GamePlayScript gamePlayScript = GamePlayScript.GetInstance();
        GameController gController = GameController.GetInstance();
        if (gamePlayScript != null && !gamePlayScript.IsCompleteShowCard || !gamePlayScript.IsCompleteCreatePlayerCard)
        {
            if (gController != null)
                gController.ShowHint(ConstantManager.HINT_WAITING_FOR_COMPLETING_CARD, this.GetComponent<RectTransform>());
            return;
        }
        if (PlayerCardManager.GetInstance() != null && PlayerCardManager.GetInstance().IsFullSlot())
        {
            return;
        }
        gamePlayScript.OnRoll(cardIndex);
    }
}

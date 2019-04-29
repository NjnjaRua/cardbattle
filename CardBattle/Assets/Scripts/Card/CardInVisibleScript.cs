using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInVisibleScript : MonoBehaviour {
    private int cardIndex;

    public void OnUpdateCardVisible(int _cardIndex)
    {
        if (_cardIndex < 0)
            return;
        this.cardIndex = _cardIndex;
    }

	public void OnChooseCard()
    {
        if (SoundManager.getInstance())
            SoundManager.getInstance().PlaySound(SoundId.TOUCH);
        if(PlayerCardManager.GetInstance() != null)
            PlayerCardManager.GetInstance().OnCreatePlayerCard(cardIndex);
    }
}

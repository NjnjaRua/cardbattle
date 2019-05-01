using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    public bool isBackSide;

    [SerializeField]
    private GameObject gBackSide;

    [SerializeField]
    private GameObject gFrontSide;

    [SerializeField]
    private Text txtName;

    [SerializeField]
    private Text txtDescription;

    [SerializeField]
    private Image background;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text txtAttackPoint;
    
    [SerializeField]
    private Text txtLifePoint;


    public void OnUpdateCard(bool backSide, Card card, int cardIndex)
    {
        if (cardIndex < 0 || card == null)
            return;
        isBackSide = backSide;
        OnShowBackSide(backSide);
        txtName.text = card.name;
        txtDescription.text = card.description;
        image.sprite = card.image;

        //get attackPoint from Card SCripttable Object
        /*txtAttackPoint.text = Util.NumberFormat(card.attackPoint);
        txtLifePoint.text = Util.NumberFormat(card.lifePoint);*/

        //get data from Jason
        txtAttackPoint.text = Util.NumberFormat(ConstantManager.GetAttackPoint(cardIndex));
        txtLifePoint.text = Util.NumberFormat(ConstantManager.GetLifePoint(cardIndex));


        SpriteAtlasScript spriteAtlas = background.GetComponent<SpriteAtlasScript>();
        if (spriteAtlas != null)
            spriteAtlas.OnChangeSprite();
    }

    public void OnShowBackSide(bool backSide)
    {
        gBackSide.SetActive(backSide);
        gFrontSide.SetActive(!backSide);
    }
}

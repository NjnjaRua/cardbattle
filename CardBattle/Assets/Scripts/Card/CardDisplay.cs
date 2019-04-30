using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {
    
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


    public void OnUpdateCard(Card card, int cardIndex)
    {
        if (cardIndex < 0 || card == null)
            return;
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
        if(spriteAtlas != null)
            spriteAtlas.OnChangeSprite();
    }
}

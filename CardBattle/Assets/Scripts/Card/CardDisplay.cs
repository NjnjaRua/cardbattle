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


    public void OnUpdateCard(Card card)
    {
        if (card == null)
            return;
        txtName.text = card.name;
        txtDescription.text = card.description;
        image.sprite = card.image;
        txtAttackPoint.text = Util.NumberFormat(card.attackPoint);
        txtLifePoint.text = Util.NumberFormat(card.lifePoint);

        SpriteAtlasScript spriteAtlas = background.GetComponent<SpriteAtlasScript>();
        if(spriteAtlas != null)
            spriteAtlas.OnChangeSprite();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteAtlasScript : MonoBehaviour {

    [SerializeField]
    public Image img;

    [SerializeField]
    public SpriteAtlas spriteAtlas;
    
    [SerializeField]
    public string spriteName;

    private string curSpriteName;

    // Use this for initialization
    void Start () {
        img = this.GetComponent<Image>();
    }

    public void OnChangeSprite()
    {
        if(img != null && spriteAtlas != null /* && curSpriteName != spriteName*/)
        {
            curSpriteName = spriteName;
            img.sprite = spriteAtlas.GetSprite(curSpriteName);
        }
    }

    public void OnSetSprite(string name)
    {
        if (string.IsNullOrEmpty(name) || spriteName == name)
            return;
        this.spriteName = name;
        OnChangeSprite();
    }
}

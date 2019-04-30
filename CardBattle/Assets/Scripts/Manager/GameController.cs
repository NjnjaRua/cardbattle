using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    private static GameController instance;

    [SerializeField]
    private Text hintText;

    private void Awake()
    {
        instance = this;
    }

    public static GameController GetInstance()
    {
        return instance;
    }

    public void ShowHint(string txt, RectTransform rectTrans = null, float time = 1)
    {
        if (hintText == null)
            return;
        if (!hintText.gameObject.activeInHierarchy)
        {
            RectTransform hintTransform = hintText.gameObject.GetComponent<RectTransform>();
            const int DELTA = 50;
            Vector3 pos;
            Text text = hintText.gameObject.GetComponent<Text>();
            text.text = txt;
            hintText.gameObject.SetActive(true);
            float w = Screen.width * 0.7f;
            if (w >= Screen.width * 0.7f)
                w = Screen.width * 0.7f;
            Vector3 size = hintTransform.sizeDelta;
            size.x = w;
            hintTransform.sizeDelta = size;

            if (rectTrans != null)
                pos = rectTrans.transform.position;
            else
                pos = Util.GetPostConvert(Input.mousePosition);
            pos.z = 0;
            hintTransform.position = pos;
            pos.y += DELTA;
            Tweener t = hintTransform.DOLocalMove(pos, time);
            t.OnComplete(() =>
            {
                hintText.gameObject.SetActive(false);
            });
        }
    }
}

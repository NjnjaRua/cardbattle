using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Battle : MonoBehaviour {

    private static Battle instance;

    [SerializeField]
    private CardDisplay comCard;

    [SerializeField]
    private CardDisplay playerCard;

    private UnityAction enableBattleAction;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        enableBattleAction = new UnityAction(OnEnableBattleAction);
        comCard.gameObject.SetActive(false);
        playerCard.gameObject.SetActive(false);
    }

    public static Battle GetInstance()
    {
        return instance;
    }

    private void OnEnable()
    {
        if (enableBattleAction == null)
            enableBattleAction = new UnityAction(OnEnableBattleAction);
        EventManager.StartListening(ConstantManager.EVENT_ENALBE_BATTLE, OnEnableBattleAction);
    }

    private void OnDisable()
    {
        EventManager.StopListening(ConstantManager.EVENT_ENALBE_BATTLE, OnEnableBattleAction);
    }

    void OnEnableBattleAction(bool isShow)
    {
        comCard.gameObject.SetActive(isShow);
        playerCard.gameObject.SetActive(isShow);
    }

    void OnEnableBattleAction()
    {
        comCard.gameObject.SetActive(true);
        playerCard.gameObject.SetActive(true);
    }

    private void OnShowCompCard(bool isBackSide)
    {
        playerCard.gameObject.SetActive(true);
        comCard.gameObject.SetActive(true);
        comCard.OnShowBackSide(isBackSide);
    }

    private void OnShowPlayerCard(bool isBackSide)
    {
        comCard.gameObject.SetActive(true);
        playerCard.OnShowBackSide(isBackSide);
        playerCard.gameObject.SetActive(true);
    }

    public Vector3 GetPosCard(bool isComputerCard)
    {
        if (isComputerCard)
            return comCard.transform.position;
        return playerCard.transform.position;
    }

    public CardDisplay GetComputerCard()
    {
        return comCard;
    }

    public CardDisplay GetPlayerCard()
    {
        return playerCard;
    }
}

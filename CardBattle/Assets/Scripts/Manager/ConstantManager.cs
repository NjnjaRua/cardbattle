
using System;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct TextRes
{
    public string key;
    public TextAsset value;
}

public class ConstantManager : MonoBehaviour
{
	public const string CARD_INFO = "CARD_INFO";
    public const string EVENT_CRATE_COMPUTER_CARD = "EVENT_CRATE_COMPUTER_CARD";


    public const string HINT_PLAYER_CARD_FULL = "Player Cards are full";
    public const string HINT_WAITING_FOR_COMPLETING_CARD = "Player Cards are being created";

    static private Dictionary<string, JSONNode> constantsCardInfoDef = null;

    [SerializeField]
    private List<TextRes> localTextAssets;

    public static JSONNode cachedJson;

	private static ConstantManager _instance;
    public const int MAX_CARD_INITIALIZE = 10;
    public const int MAX_FIGHTING_CARD = 5;

	public static ConstantManager getInstance()
	{
		return _instance;
	}

	void Awake()
	{
		LocalConstantAsset = new Dictionary<string, byte[]>();
		CachedConstantAsset = new Dictionary<string, byte[]>();

		CachedConstantMD5 = new Dictionary<string, string>();

		ConstantDownloaded = new Dictionary<string, bool>();
		_instance = this;
	}

	public void StartLoad()
	{
		LoadAllConstant();
	}

    public void LoadAllConstant()
    {
        cachedJson = null;
        int count = CONST_KEY_TO_LOAD.Length;
        for (int i = 0; i < count; i++)
            LoadConstantByKey(CONST_KEY_TO_LOAD[i]);
    }


	public delegate void ReloadConstantCompleteDelegate();

	ReloadConstantCompleteDelegate onLoadConstantComplete;
	public Dictionary<string, byte[]> LocalConstantAsset;
	public Dictionary<string, byte[]> CachedConstantAsset;

	public Dictionary<string, string> CachedConstantMD5;

	public Dictionary<string, bool> ConstantDownloaded;

	public static float CONSTANT_TIMEOUT = 5;
	//public TMPro.SpriteAsset tmProSpriteAsset;

	public void Init(string constText, bool parseLang = true)
	{
		ConstantDownloaded.Clear();
		int count = CONST_KEY_TO_LOAD.Length;
		for (int i = 0; i < count; i++)
			ConstantDownloaded.Add(CONST_KEY_TO_LOAD[i], false);
	}

	public void LoadConstantByKey(string _key)
	{
		if (Array.IndexOf(CONST_KEY_TO_LOAD, _key) < 0)
			return;
        string cachedString = GetConstantString(_key, true);
        switch (_key)
        {
            case CARD_INFO:
                LoadItemConstants(cachedString);
                break;

            //case ....

            default:
                LoadItemConstants(_key);
                break;
        }
        cachedString = null;
    }

	static public string[] CONST_KEY_TO_LOAD = new string[]
	{
        CARD_INFO
    };

    public static void LoadItemConstants(string jsonText)
    {
        if (string.IsNullOrEmpty(jsonText)) return;

        if (constantsCardInfoDef == null)
            constantsCardInfoDef = new Dictionary<string, JSONNode>();

        JSONClass json = (JSONClass)JSON.Parse(jsonText);
        if (json == null)
            return;
        JSONNode node;

        foreach (KeyValuePair<string, JSONNode> pair in json)
        {
            node = pair.Value;
            constantsCardInfoDef[pair.Key] = node;
        }
    }

    public Dictionary<string, string> CachedFinalString = new Dictionary<string, string>();
    public string GetConstantString(string key, bool returnNullIfMatch = false)
    {
        string md5Hash = "", finalStr = "";
        if (CachedConstantMD5.TryGetValue(key, out md5Hash))
        {
            if (CachedFinalString.TryGetValue(md5Hash, out finalStr))
                return finalStr;
            byte[] bytes = GetConstantBytes(key, returnNullIfMatch);
            if (bytes != null && bytes.Length > 0)
            {
                finalStr = new string(Encoding.UTF8.GetChars(bytes));
                CachedFinalString[md5Hash] = finalStr;
            }
        }

        return finalStr;
    }

    public byte[] GetConstantBytes(string key, bool returnNullIfMatch = false)
    {
        byte[] bytes1 = null, bytes2 = null;

        if (CachedConstantAsset != null && CachedConstantAsset.Count > 0)
            CachedConstantAsset.TryGetValue(key, out bytes1);

        LocalConstantAsset.TryGetValue(key, out bytes2);

        if (bytes2 != null && bytes2.Length > 0)
            return bytes2;

        if (bytes1 != null && bytes1.Length > 0)
        {
            if ((bytes2 != null && bytes2.Length > 0) && Util.QuickCompare(bytes1, bytes2))
            {
                if (returnNullIfMatch)
                    return null;
                return bytes1;
            }
            else
            {
                return bytes1;
            }
        }

        return bytes2;
    }
    
    public void Initialize()
    {
        if (localTextAssets != null)
        {
            for (int i = 0, len = localTextAssets.Count; i < len; i++)
            {
                AddLocalConstantText(localTextAssets[i].key, localTextAssets[i].value);
            }
        }
    }


    private void AddLocalConstantText(string key, TextAsset content)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("AddLocalConstantText can not add null key");
            return;
        }

        if (content == null)
        {
            Debug.LogError("AddLocalConstantText can not add content null of key = " + key);
            return;
        }

        CachedConstantMD5[key] = Util.GetMd5Hash(content.bytes);
        LocalConstantAsset[key] = content.bytes;
    }

    private static JSONNode GetConstantsCardInfoDef(int cardId)
    {
        if (cardId < 0 || constantsCardInfoDef == null || constantsCardInfoDef.Count <= 0)
            return null;
        JSONNode node = null;
        string strCardId = cardId.ToString();
        if(!constantsCardInfoDef.TryGetValue(strCardId, out node))
        {
            return null;
        }
        return node;
    }

    public static int GetAttackPoint(int cardId)
    {
        int atk = -1;
        if (cardId < 0)
            return atk;
        JSONNode node = GetConstantsCardInfoDef(cardId);
        if (node == null || node["attackPoint"] == null)
            return atk;
        return node["attackPoint"].AsInt;
    }

    public static int GetLifePoint(int cardId)
    {
        int hp = -1;
        if (cardId < 0)
            return hp;
        JSONNode node = GetConstantsCardInfoDef(cardId);
        if (node == null || node["lifePoint"] == null)
            return hp;
        return node["lifePoint"].AsInt;
    }
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadingScript : MonoBehaviour {
	
	public static LoadingScript InstanceObject;

	public Text loadingText;

	public Text hint;
	public GameObject loader;
	public bool isInit;
	public static bool is_loading;
	public Image fillProgressBar;
	public string[] hints;
	public GameObject loadIndicator;

	private const int TIME = 5;
	private int currentPercent = 0;
	private int fakePercent =0;
    public Text userName;

    // Use this for initialization
    void Start () {		
		isInit = false;
        Init();
		InstanceObject = this;
        StartCoroutine(LoadingUpdate());
		
		hint.gameObject.SetActive (false);
		AutoHint ();
        if (SoundManager.getInstance())
            SoundManager.getInstance().PlaySound(SoundId.NORMAL, true);
	}

    public void IncreaseUpdate(int inc = 1)
    {
        fakePercent += inc;
    }

	public void ShowUpdate(string textLoading, int percentage)
	{
        Init();
        if (percentage >= 89)
            fakePercent = 100;
        else if (percentage > fakePercent)
            fakePercent = percentage;
    }

	public void SetLoaderActive(bool b)
	{
        loader.SetActive (b);
	}

	void UpdateProgressBar()
	{
        if (currentPercent < 0)
			loadingText.text = "0%";
		else if (currentPercent > 100)
			loadingText.text = "100%";
		else
			loadingText.text = currentPercent + "%";

		float p = ((float)currentPercent / 100);
		if(p > 1)
			fillProgressBar.fillAmount = 1;
		else
			fillProgressBar.fillAmount = p;
    }

	void Update()
	{
        if (currentPercent == 100)
        {
            OnChangeScene();
        }
        else if (currentPercent < 100)
		{
			if (fakePercent > currentPercent)
				currentPercent++;
			else
				currentPercent = fakePercent;
			UpdateProgressBar ();
		}
		else if (currentPercent < 105)
		{
			loadIndicator.SetActive (true);
			currentPercent++;
		}
	}

	public void OnChangeScene()
	{
		StartCoroutine(LoadingUpdateComplete());
	}

	IEnumerator LoadingUpdateComplete()
	{
        yield return new WaitForSeconds(0.5f);
        OnRunComplete ();
    }

	void OnRunComplete()
	{
        DeInit ();
	}

	IEnumerator LoadingUpdate()
	{
        IncreaseUpdate (5);
		yield return new WaitForSeconds(0.1f);
        ConstantManager.getInstance ().Initialize ();
		IncreaseUpdate (20);
        yield return new WaitForSeconds(0.1f);
        hint.gameObject.SetActive (true);
		AutoHint ();
		yield return new WaitForSeconds(0.1f);
        int count = ConstantManager.CONST_KEY_TO_LOAD.Length;
		int per = Mathf.RoundToInt(10/count);
		if (per <= 0)
			per = 1;
		for (int  i = 0; i < count; i++)
		{			
			ConstantManager.getInstance().LoadConstantByKey(ConstantManager.CONST_KEY_TO_LOAD[i]);
			if(i%per == 0)
				IncreaseUpdate (5);
			yield return new WaitForSeconds(0.2f);
		}
        yield return new WaitForSeconds(0.2f);
        IncreaseUpdate(20);
        yield return new WaitForSeconds(0.2f);
        IncreaseUpdate(30);
        yield return new WaitForSeconds(0.2f);
        IncreaseUpdate(20);
    }


	void Init ()
	{
		if (!isInit)
		{
			this.gameObject.SetActive (true);
			loader.SetActive (true);
			loadIndicator.SetActive (false);
			hint.gameObject.SetActive (true);
			AutoHint ();

			isInit = true;
			is_loading = true;
            string _userName = "TruongNguyen"; //@todo: can get from user data
            userName.text = "ID: " + _userName;
		}
	}

	void AutoHint ()
	{
		int randomIndex = Random.Range (0, hints.Length);
		hint.text = hints [randomIndex];
	}

	void DeInit ()
	{
		fakePercent = 0;
		currentPercent = 0;
		loadingText.text = "0%";
		hint.gameObject.SetActive (false);
		loadIndicator.SetActive (false);
		this.gameObject.SetActive (false);
		is_loading = false;
		isInit = false;
        if (SoundManager.getInstance())
            SoundManager.getInstance().StopSound();
        if (MainController.GetInstance() != null)
            MainController.GetInstance().SwitchScene(MainController.SCENE_MAIN_GAME);
    }
}

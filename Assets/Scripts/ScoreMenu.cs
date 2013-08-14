using UnityEngine;
using System.Collections;

public class ScoreMenu : MonoBehaviour {
	public bool closeGUI;

    public Texture2D WinScreen;
    public Texture2D LooseScreen;

    private Texture2D showScreen;

    public Vector2 ScreenSize;
	private Vector2 screenRes;

	// Use this for initialization
	void Awake () {
		closeGUI = false;
        showScreen = LooseScreen;
		screenRes = AGGame.Instance.guiManager.ScreenResolution;
		ScreenSize = new Vector2(LooseScreen.width, LooseScreen.height);
		ScreenSize = AGGame.Instance.guiManager.ResizeTexture(LooseScreen);
	}
	
    IEnumerator CloseGUI()
    {

        yield return new WaitForSeconds(1.5f);
        closeGUI = true;
    }
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
           StartCoroutine(CloseGUI());
            
        }
	}
    public void SetWinner(int ID)
    {
        showScreen = WinScreen;
    }
	void OnGUI()
	{

        GUI.DrawTexture(new Rect(screenRes.x / 2 - ScreenSize.x/2, screenRes.y / 2, ScreenSize.x, ScreenSize.y), showScreen, ScaleMode.ScaleToFit, true);
	}
}

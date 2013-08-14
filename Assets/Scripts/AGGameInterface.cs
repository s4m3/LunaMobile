using UnityEngine;
using System.Collections;

public class AGGameInterface : MonoBehaviour {
	public Texture2D crownBackground;
	public Texture2D crownWin;
	public Texture2D[] crownFinalTextures;
	public Texture2D controllerTexture;
	public Texture2D ultimateDescriptionTexture;
	private int rounds;
	public int Rounds
	{
		get { return rounds; }
		set { rounds = value; }
	}
	private int[] roundWin;
	private float guiStartPos;
	public float pixelsBetweenCrownTextures = 20;
	public float crownTopPosition = 50;
	public bool showRoundsVertical = false;
	private int[] roundView;
	public Vector2 ControllerPos;
	private bool showController = false;
	public bool ShowController
	{
		set{ showController = value; }
	}
	
	private Vector2 screenRes;
	private Vector2 normalCrownSize;
	private Vector2 finalCrownSize;
	
	void Start () {
		roundWin = new int[Rounds];
		roundView = new int[Rounds];
		GUIManager guiManager = AGGame.Instance.guiManager;
		screenRes = guiManager.ScreenResolution;
		normalCrownSize = guiManager.ResizeTexture(crownBackground);
		float resizeVal = normalCrownSize.x / crownBackground.width;
		finalCrownSize = new Vector2(crownFinalTextures[0].width, crownFinalTextures[0].height);
		finalCrownSize *= resizeVal;
		pixelsBetweenCrownTextures *= resizeVal;
		crownTopPosition*= resizeVal;
			 
		guiStartPos = screenRes.x/2 - ((normalCrownSize.x + pixelsBetweenCrownTextures) * (Rounds/2)) + pixelsBetweenCrownTextures/2;
		if(Rounds % 2 == 1) guiStartPos -= (normalCrownSize.x/2 + pixelsBetweenCrownTextures/2);
	}
	
	public void SetWinner(int roundNumber, int WinnerID)
	{
		if(roundNumber > Rounds) return;
		roundWin[roundNumber-1] = WinnerID;
		roundView = calculateRoundView(roundWin);
	}
	
	void OnGUI()
	{
		Texture2D crownTex;
		//Group of GUI Items: Crowns
		GUI.BeginGroup(new Rect(guiStartPos, crownTopPosition, (Rounds-1)*(normalCrownSize.x + pixelsBetweenCrownTextures) + finalCrownSize.x, finalCrownSize.y));
		for(int i = 0; i<Rounds; i++)
		{
			if(i != (Rounds/2))
			{
				crownTex = roundView[i] == 0 ? crownBackground : crownWin;
				GUI.DrawTexture(new Rect(i*normalCrownSize.x + i*pixelsBetweenCrownTextures, 0, normalCrownSize.x, normalCrownSize.y), crownTex);
			}
		}
		GUI.EndGroup();
		
		//WinningCrown
		GUI.DrawTexture(new Rect(screenRes.x/2 - finalCrownSize.x/2, crownTopPosition - finalCrownSize.y/4, finalCrownSize.x, finalCrownSize.y), crownFinalTextures[roundView[Rounds/2]]);
		
		//MOBILE: SHOW MOBILE CONTROLLS
//		if(showController) 
//		{
//			GUI.DrawTexture(new Rect(ControllerPos.x, ControllerPos.y, controllerTexture.width, controllerTexture.height), controllerTexture);
//			GUI.DrawTexture(new Rect(Screen.width* 7/8 - ultimateDescriptionTexture.width/2,  Screen.height / 4, ultimateDescriptionTexture.width, ultimateDescriptionTexture.height), ultimateDescriptionTexture);
//		}
	}
	
	private int[] calculateRoundView(int[] wins)
	{
		int wins_p1 = 0, wins_p2 = 0, blanks = 0;
		int[] result = new int[Rounds];
		foreach(int i in wins){
			if(i == 1) wins_p1++;
			if(i == 2) wins_p2++;
		}
		blanks = Rounds - wins_p1 - wins_p2;
		for(int j = 0; j < Rounds; j++)
		{
			if(wins_p1 > 0)
			{
				result[j] = 1;
				wins_p1--;
			} else if(blanks > 0)
			{
				result[j] = 0;
				blanks--;
			} else
			{
				result[j] = 2;
			}
		}
		return result;
	}
	
}

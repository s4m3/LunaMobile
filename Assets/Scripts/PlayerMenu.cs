using UnityEngine;
using System.Collections;

public class PlayerMenu : MonoBehaviour {
	private Vector2 ReadyButtonPos;
	AGPlayerController player;
	public Texture2D buttonTextureL;
	public Texture2D buttonTextureR;
	private Texture2D buttonTexture;
	public Texture2D readyTexture;
	public Texture2D greyTexture;
	public GUIStyle guiStyle;
	private bool greyOut;
	
	private Vector2 screenRes;
	
	public void SetPlayer(AGPlayerController _p){	
		player = _p;
		buttonTexture = player.PlayerID == 1 ? buttonTextureL : buttonTextureR;
		ReadyButtonPos = new Vector2(Screen.width/4 - buttonTexture.width/2, Screen.height/2 - buttonTexture.height/2);
		
		if(player.is_AI_Player)
		{
			greyOut = false;
			player.playerReady = true;
		}
	}
	void Awake () {
		greyOut = true;
		screenRes = AGGame.Instance.guiManager.ScreenResolution;
	}
	
	// Update is called once per frame
	void Update () {
		//MOBILE
//		if (Input.GetButton("FireA_p"+player.PlayerID))
//		{
//			joystickButtonDown = true;
//		}
	}
	
	void OnGUI ()
	{
		if(player.is_AI_Player) return;
		
		if (!player || !player.pawn || !buttonTexture)
			return;
		
		if (greyOut)
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), greyTexture);
		
		if (!player.playerReady)
		{
			GUILayout.BeginArea(new Rect(0, 0, screenRes.x, screenRes.y));
		
			GUILayout.BeginVertical(guiStyle);
			GUILayout.FlexibleSpace();
			
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			if(GUILayout.Button ("Ready?", guiStyle))
			{
				player.playerReady = true;
				greyOut = false;
				AGGame.Instance.getSoundServer ().Play ("menu");
			}
			
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			
			GUILayout.EndArea();
			
			
			
		}
		
		if (player.playerReady) {
			GUI.Label (new Rect (screenRes.x / 2 - readyTexture.width/2, ReadyButtonPos.y, readyTexture.width, readyTexture.height), readyTexture);
		}
	}
}

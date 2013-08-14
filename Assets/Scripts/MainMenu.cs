using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Vector2 LogoOffset;

	public GUIStyle buttonStyle;

	public Texture2D backgroundTexture;
	public Texture2D credits;
	public Texture2D creditBackground;
	public Texture2D logo;
	public Texture muteButtonTexture;
	private Vector2 creditSize;
	public enum GameMode {None, Multiplayer}
	public GameMode gameMode;
	public bool isMuted;
	
	private Vector2 screenRes;//TODO GUI-Manager zum speichern solcher werte
	private bool creditsVisible;
	void Awake () {
		gameMode = GameMode.None;
		//if(CreditsXPos == 0) CreditsXPos = Screen.width - credits.width;
		screenRes = AGGame.Instance.guiManager.ScreenResolution;
		creditSize = AGGame.Instance.guiManager.ResizeTexture(credits);
		creditsVisible = false;
	}
	


	private void showCredits()
	{
		//Credits
		GUI.DrawTexture(new Rect (screenRes.x * 0.7f, 0, screenRes.x * 0.3f, screenRes.y), creditBackground);
		GUI.DrawTexture(new Rect (screenRes.x * 0.7f, 0, creditSize.x, creditSize.y), credits);
	}
	
	void OnGUI ()
	{
		//GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( Screen.width / 1024.0f, Screen.height / 768.0f, 1.0f ) );
		GUI.DrawTexture (new Rect (0, 0, screenRes.x, screenRes.y), backgroundTexture);
		GUI.DrawTexture (new Rect (screenRes.x/2 - logo.width/2 + LogoOffset.x, LogoOffset.y, logo.width, logo.height), logo);

		GUILayout.BeginArea(new Rect(0,screenRes.y/2, screenRes.x, screenRes.y/2));
		
		GUILayout.BeginVertical(buttonStyle);
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button ("Start Game", buttonStyle))
		{
		//Start Multiplayer
			gameMode = GameMode.Multiplayer;
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button ("Credits", buttonStyle))
		{
			creditsVisible = !creditsVisible;
		}
				
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button ("Quit", buttonStyle))
		{
			//Quit Game
			Application.Quit();	
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		
		string buttontext = isMuted ? "Sound On" : "Sound Off";
		if(GUILayout.Button (buttontext, buttonStyle))
		{
			//Mute&Unmute
			isMuted = !isMuted;
		}
				
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		GUILayout.Space(20);
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		if(creditsVisible) showCredits();
	}
	
}

[System.Serializable]
public class ButtonGroup
{
	public Texture selected;
	public Texture unSelected;
}

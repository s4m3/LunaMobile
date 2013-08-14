using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterChoiceMenu : MonoBehaviour {

	private AGPlayerController player;
	
	public List<Texture> characters;
	private Vector2 charactersSize;
	
	public Texture chooseCharacterText;
	private Vector2 chooseCharacterTextSize;

	
	private string currentCharacter;
	private int characterIndex;
	private bool canChangeCharacter = true;
	private bool leftClick = false;
	private bool rightClick = false;
	private bool startDelay = true;

	public GUIStyle buttonStyle;
	public Texture2D greyTexture;
	
	private Vector2 screenRes;
	
	public int ChosenCharacter
	{
		get { return characterIndex; }
	}
	
	
	private IEnumerator doDelay(float seconds)
	{
		startDelay = true;
		yield return new WaitForSeconds(seconds);
		startDelay = false;
	}
	
	public void SetPlayer(AGPlayerController _p){	
		player = _p;
		
		if(player.is_AI_Player)
		{
			characterIndex = Random.Range(0, characters.Count - 1);
			player.characterChosen = true;
		}
	}
	
	void Awake () {
		StartCoroutine(doDelay(1f));
		characterIndex = 0;
		GUIManager guiManager = AGGame.Instance.guiManager;
		screenRes = guiManager.ScreenResolution;
		if(characters.Count < 1) 
		{
			Debug.LogError("Character textures not setup");
			return;
		}
		//calculate texture sizes according to screen resolution

		charactersSize = guiManager.ResizeTexture(characters[0]);
		chooseCharacterTextSize = guiManager.ResizeTexture(chooseCharacterText);

	}
	
	void Update () {
		if(startDelay) return;
		
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			// Get movement of the finger since last frame
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			if(touchDeltaPosition.x > 30)
				changeCharacter (1);
			else if(touchDeltaPosition.x < -30)
				changeCharacter (-1);
    	}
		
	}
	
	private void changeCharacter(float input)
	{
		characterIndex += (int)input;
		if(characterIndex > characters.Count - 1)
			characterIndex = 0;
		if(characterIndex < 0)
			characterIndex = characters.Count - 1;
		//characterIndex = Mathf.Clamp(characterIndex, 0, characters.Count - 1);
		//to prevent the change to happen multiple times per frame, there has to be waiting time
		StartCoroutine(doDelay(0.4f));

	}
	
	
	void OnGUI ()
	{
		//MOBILE: SWIPE CONTROL!
		if(player.is_AI_Player) return;
		
		GUI.DrawTexture (new Rect (0, 0, screenRes.x, screenRes.y), greyTexture);
		
		
		GUILayout.BeginArea(new Rect(0,0, screenRes.x, screenRes.y));
		GUILayout.BeginVertical(buttonStyle);
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
	
		if (!player.characterChosen) GUILayout.Box (chooseCharacterText, buttonStyle, GUILayout.Height(chooseCharacterTextSize.y), GUILayout.Width(chooseCharacterTextSize.x));
		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		
		if (GUILayout.Button (characters [characterIndex], buttonStyle, GUILayout.Height(charactersSize.y), GUILayout.Width(charactersSize.x))) {
			player.characterChosen = true;
			AGGame.Instance.getSoundServer ().Play ("menu");
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.Space(screenRes.y / 10);
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		
		
		
//		if (!player.characterChosen)
//			GUI.DrawTexture (new Rect (screenRes.x/2 - chooseCharacterText.width/2, 40, chooseCharacterText.width, chooseCharacterText.height), chooseCharacterText);
//
//		if (GUI.Button (new Rect (screenRes.x/2 - characters [characterIndex].width/2, screenRes.x/8, characters [characterIndex].width, characters [characterIndex].height), characters [characterIndex], buttonStyle)) {
//			player.characterChosen = true;
//			AGGame.Instance.getSoundServer ().Play ("menu");
//		}
		
		
		//TODO:Swipe control for change
//		if (!player.characterChosen && GUI.Button (new Rect (screenRes.x/2 - leftButtonPos.x, leftButtonPos.y, buttonL [0].width, buttonL [0].height), btnL, buttonStyle)) {
//			AGGame.Instance.getSoundServer ().Play ("menu");
//			StartCoroutine (changeCharacter (-1));
//		}
//		
//		if (!player.characterChosen && GUI.Button (new Rect (screenRes.x/2 + rightButtonPos.x, rightButtonPos.y, buttonR [0].width, buttonR [0].height), btnR, buttonStyle)) {
//			AGGame.Instance.getSoundServer ().Play ("menu");
//			StartCoroutine (changeCharacter (1));
//		}
		

	}
}

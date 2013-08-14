using UnityEngine;
using System.Collections;

public class AGPlayerInterface : MonoBehaviour {
	
	public Vector2 CandyBarOffsetR;	
	public Vector2 CandyBarOffsetL;
	public Vector2 HealthBarOffsetR;
	public Vector2 HealthBarOffsetL;
	
	public Vector2 StatsBackPos;	
	public Vector2 HeadTexturePos;
	
	public Texture2D CandybarTexture;
	public Texture2D HealthbarTexture;
	public Texture2D StatsBackTexture;
	
	private Vector2 candybarSize;
	private Vector2 healthbarSize;
	private Vector2 statsBackSize;
	private Vector2 headTextureSize;
	
	public HeadTextureGroup[] HeadTextures;
	private Texture2D currentHeadTexture;
	private int headGroupID;
	
	public GameObject VignettePrefab;
	public Vignette vignette;
	
	public GUIStyle guiStyle;
	AGPlayerController player;
	
	private Vector2 screenRes;
	
	
	
	public void SetPlayer(AGPlayerController _p){	
		player = _p;
	}
	
	// Use this for initialization
	void Start () {
		SetHeadTextureGroupID();
		GUIManager guiManager = AGGame.Instance.guiManager;
		screenRes = guiManager.ScreenResolution;
		healthbarSize = guiManager.ResizeTexture(HealthbarTexture);
		float resizeVal = healthbarSize.x / HealthbarTexture.width;
		candybarSize = new Vector2(CandybarTexture.width, CandybarTexture.height);
		headTextureSize = new Vector2(HeadTextures[0].dreamHead.width, HeadTextures[0].dreamHead.height);
		candybarSize *= resizeVal;
		headTextureSize *= resizeVal;
		StatsBackPos *= resizeVal;
		
		if(player.is_AI_Player) return;
		
		if(!vignette)
		{
			GameObject obj = (GameObject)GameObject.Instantiate (VignettePrefab);
			vignette = obj.GetComponent<Vignette> ();
			vignette.SetPlayer (player);
		}
	}
	
	public void SetHeadTextureGroupID()
	{
		if(player.info.PlayerClass == AGPlayerClass.Classes.Werewolf)
			headGroupID = 0;
		else
			headGroupID = 1;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void UpdateVignette(bool hit, float percnt)
	{
		if(!vignette)
			return;
        vignette.UpdateVignetteAlpha(hit, percnt);
	}
	
	public void DestroyVignette()
	{
		if(vignette) Destroy(vignette.gameObject);
	}
	
	public void UpdateHeadTexture(AGActor.LightState lightState)
	{
		if(lightState == AGActor.LightState.Dream)
			currentHeadTexture = HeadTextures[headGroupID].dreamHead;
		else if(lightState == AGActor.LightState.Real)
			currentHeadTexture = HeadTextures[headGroupID].realHead;
	}
	
	void OnGUI() {
		if(!player || !player.pawn)
			return;
		
		float health = (float) player.pawn.Health.currentValue / player.pawn.Health.max;
		float candy = (float) player.pawn.Energy.currentValue / player.pawn.Energy.max;
		
		//HUD
		if(!player.is_AI_Player)
		{
			GUI.BeginGroup(new Rect(StatsBackPos.x, StatsBackPos.y, healthbarSize.x, healthbarSize.y *1.5f + candybarSize.y));
				//GUI.DrawTexture(new Rect(statsBackSize.x, 0, -statsBackSize.x, statsBackSize.y), StatsBackTexture);
				int healthbarSizeX = Mathf.RoundToInt(healthbarSize.x);
				GUI.BeginGroup(new Rect(Mathf.CeilToInt((1-health) * healthbarSizeX), 0, health * healthbarSizeX, healthbarSize.y));
					GUI.DrawTexture(new Rect(Mathf.FloorToInt(health * healthbarSizeX), 0, -healthbarSizeX, healthbarSize.y), HealthbarTexture);
				GUI.EndGroup();
				GUI.BeginGroup(new Rect((1-candy) * candybarSize.x, healthbarSize.y * 1.2f, candy * candybarSize.x, candybarSize.y));
					GUI.DrawTexture(new Rect(candy * candybarSize.x, 0, -candybarSize.x, candybarSize.y), CandybarTexture);
				GUI.EndGroup();
			GUI.EndGroup();
			GUI.DrawTexture(new Rect(0, 0, headTextureSize.x, headTextureSize.y), currentHeadTexture);
		} else {
			GUI.BeginGroup(new Rect(screenRes.x - StatsBackPos.x - healthbarSize.x, StatsBackPos.y, healthbarSize.x, healthbarSize.y *1.5f + candybarSize.y));
				//GUI.DrawTexture(new Rect(0, 0, StatsBackTexture.width, StatsBackTexture.height), StatsBackTexture);
				GUI.BeginGroup(new Rect(0, 0, health * healthbarSize.x, healthbarSize.y));
					GUI.DrawTexture(new Rect(0, 0, healthbarSize.x, healthbarSize.y), HealthbarTexture);
				GUI.EndGroup();
				GUI.BeginGroup(new Rect(0, healthbarSize.y * 1.2f, candy * candybarSize.x, candybarSize.y));
					GUI.DrawTexture(new Rect(0, 0, candybarSize.x, candybarSize.y), CandybarTexture);
				GUI.EndGroup();
				
			GUI.EndGroup();
			GUI.DrawTexture(new Rect(screenRes.x, 0, -headTextureSize.x, headTextureSize.y), currentHeadTexture);
			
		}
	}
}


[System.Serializable]
public class HeadTextureGroup
{
	public Texture2D realHead;
	public Texture2D dreamHead;
}


//OLD TRIALS:
//		GUILayout.BeginArea(new Rect(0,0, screenRes.x/2, screenRes.y/4));
//		GUILayout.BeginHorizontal();
//		GUILayout.FlexibleSpace();
//		
//		GUILayout.BeginVertical();
//		GUILayout.FlexibleSpace();
//		
//		GUILayout.Box(currentHeadTexture);
//		GUILayout.FlexibleSpace();
//		
//		GUILayout.BeginHorizontal();
//		GUILayout.FlexibleSpace();
//		
//		GUI.BeginGroup(new Rect(0,0, statsBackSize.x, statsBackSize.y));
//			//GUI.DrawTexture(new Rect(statsBackSize.x, 0, -statsBackSize.x, statsBackSize.y), StatsBackTexture);
//			GUI.BeginGroup(new Rect((1-health) * healthbarSize.x, 0, health * healthbarSize.x, healthbarSize.y));
//				GUI.DrawTexture(new Rect(health * healthbarSize.x, 0, -healthbarSize.x, healthbarSize.y), HealthbarTexture);
//			GUI.EndGroup();
//			GUI.BeginGroup(new Rect((1-candy) * candybarSize.x, 0, candy * candybarSize.x, candybarSize.y));
//				GUI.DrawTexture(new Rect(candy * candybarSize.x, 0, -candybarSize.x, candybarSize.y), CandybarTexture);
//			GUI.EndGroup();	
//		GUI.EndGroup();
//		
//		GUILayout.FlexibleSpace();
//		GUILayout.EndHorizontal();
//		
//		GUILayout.FlexibleSpace();
//		GUILayout.EndVertical();
//			
//		GUILayout.FlexibleSpace();
//		GUILayout.EndHorizontal();
//		GUILayout.EndArea();
		
//		{
//			//HUD
//			float health = (float) player.pawn.Health.currentValue / player.pawn.Health.max;
//			float candy = (float) player.pawn.Energy.currentValue / player.pawn.Energy.max;
//			GUI.BeginGroup(new Rect(Screen.width/2 - SplitOffset.x - StatsBackPos.x - StatsBackTexture.width, SplitOffset.y + StatsBackPos.y, StatsBackTexture.width, StatsBackTexture.height));
//				GUI.DrawTexture(new Rect(StatsBackTexture.width, 0, -StatsBackTexture.width, StatsBackTexture.height), StatsBackTexture);
//				GUI.BeginGroup(new Rect(HealthBarOffsetL.x + (1-health) * HealthbarTexture.width, HealthBarOffsetL.y, health * HealthbarTexture.width, HealthbarTexture.height));
//					GUI.DrawTexture(new Rect(health * HealthbarTexture.width, 0, -HealthbarTexture.width, HealthbarTexture.height), HealthbarTexture);
//				GUI.EndGroup();
//				GUI.BeginGroup(new Rect(CandyBarOffsetL.x + (1-candy) * CandybarTexture.width, CandyBarOffsetL.y, candy * CandybarTexture.width, CandybarTexture.height));
//					GUI.DrawTexture(new Rect(candy * CandybarTexture.width, 0, -CandybarTexture.width, CandybarTexture.height), CandybarTexture);
//				GUI.EndGroup();
//				
//			GUI.EndGroup();
//			GUI.DrawTexture(new Rect(Screen.width/2 - SplitOffset.x - HeadTexturePos.x - currentHeadTexture.width, SplitOffset.y + HeadTexturePos.y, currentHeadTexture.width, currentHeadTexture.height), currentHeadTexture);
//		} else
//		{
//			float health = (float) player.pawn.Health.currentValue / player.pawn.Health.max;
//			float candy = (float) player.pawn.Energy.currentValue / player.pawn.Energy.max;
//			GUI.BeginGroup(new Rect(SplitOffset.x + StatsBackPos.x, SplitOffset.y + StatsBackPos.y, StatsBackTexture.width, StatsBackTexture.height));
//				GUI.DrawTexture(new Rect(0, 0, StatsBackTexture.width, StatsBackTexture.height), StatsBackTexture);
//				GUI.BeginGroup(new Rect(HealthBarOffsetR.x, HealthBarOffsetR.y, health * HealthbarTexture.width, HealthbarTexture.height));
//					GUI.DrawTexture(new Rect(0, 0, HealthbarTexture.width, HealthbarTexture.height), HealthbarTexture);
//				GUI.EndGroup();
//				GUI.BeginGroup(new Rect(CandyBarOffsetR.x, CandyBarOffsetR.y, candy * CandybarTexture.width, CandybarTexture.height));
//					GUI.DrawTexture(new Rect(0, 0, CandybarTexture.width, CandybarTexture.height), CandybarTexture);
//				GUI.EndGroup();
//				
//			GUI.EndGroup();
//			GUI.DrawTexture(new Rect(SplitOffset.x + HeadTexturePos.x + currentHeadTexture.width, SplitOffset.y + HeadTexturePos.y, -currentHeadTexture.width, currentHeadTexture.height), currentHeadTexture);
//		}
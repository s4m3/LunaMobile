using UnityEngine;
using System.Collections;

public class Vignette : MonoBehaviour {
	
	public Texture2D VignetteTexture;
	public Color VignetteColor;
	public float FadeOutBaseSpeed;
    private float currentFadeOutSpeed;

	private float vignetteAlpha;
	Vector2 SplitOffset;
	AGPlayerController player;
	
	// Use this for initialization
	void Start () {
		vignetteAlpha = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(player.is_AI_Player) return;
		if(vignetteAlpha > 0) UpdateVignetteAlpha(false,1);
	}
	
	public void SetPlayer(AGPlayerController _p){	
		player = _p;
		SetOffset(AGGame.Get2DCameraOffset(player));
		if(player.is_AI_Player) this.enabled = false;
	}
	public void SetOffset(Vector2 _offset){
		SplitOffset.x = Screen.width * _offset.x;
		SplitOffset.y = Screen.width * _offset.y;
		
	}
	
	public void UpdateVignetteAlpha(bool hit, float percent)
	{
        if (hit)
        {
            
            vignetteAlpha = 1;
            currentFadeOutSpeed = Mathf.Clamp( percent * FadeOutBaseSpeed, 0.2f, FadeOutBaseSpeed);
           // Debug.Log(currentFadeOutSpeed);
        }
        vignetteAlpha -= Time.deltaTime * currentFadeOutSpeed;
		
	}
	
	void OnGUI() {
		if(player.is_AI_Player) return;
		if(!VignetteTexture)
			return;

		VignetteColor.a = vignetteAlpha;
		GUI.color = VignetteColor;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), VignetteTexture, ScaleMode.ScaleAndCrop, true);
	}
}

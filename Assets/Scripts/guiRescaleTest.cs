using UnityEngine;
using System.Collections;

public class guiRescaleTest : MonoBehaviour {

	public ScreenArea screenArea;
	public Texture testTexture;
	Rect pos;
	
	void Start () {
		screenArea = new ScreenArea();
		screenArea.borderHor = 5;
		
		screenArea.borderVert = 5;
		
		screenArea.width = Screen.width / 4; //quarter of the screen
		
		screenArea.height = Screen.height / 4; //quarter of the screen
		
		screenArea.ePosition = ScreenArea.eScreenPos.TopLeft;
		pos = screenArea.DefinedArea();
		
	}
	
	void OnGUI() 
	{
		GUI.BeginGroup(pos);
			//GUI.DrawTexture(new Rect(0, 0, testTexture.width, testTexture.height), testTexture);
		GUI.EndGroup();
		GUI.DrawTexture(ResizeGUI(new Rect(5,5,testTexture.width,testTexture.height)), testTexture);
	}
	
	Rect ResizeGUI(Rect _rect)
	{
	    float FilScreenWidth = _rect.width / 800;
	    float rectWidth = FilScreenWidth * Screen.width;
	    float FilScreenHeight = _rect.height / 600;
	    float rectHeight = FilScreenHeight * Screen.height;
	    float rectX = (_rect.x / 800) * Screen.width;
	    float rectY = (_rect.y / 600) * Screen.height;
	 
	    return new Rect(rectX,rectY,rectWidth,rectHeight);
	}
}

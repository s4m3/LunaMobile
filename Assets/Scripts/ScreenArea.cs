using UnityEngine;
using System.Collections;

public class ScreenArea : Object {
	
	public enum eScreenPos
	{
		TopLeft,
		TopCenter,
	    TopRight,
	    MiddleLeft,
	    MiddleCenter,
	    MiddleRight,
	    BottomLeft,
	    BottomCenter,
	    BottomRight
	};
	
	public eScreenPos ePosition = eScreenPos.TopRight;
	public int width = 255;
	public int height = 124;
	public int borderHor = 0;
	public int borderVert = 0;
	
	private Rect _area;
	
	public Rect GetArea()
	{
		return _area;
	}
	
	public Rect DefinedArea()
	{
		float posLeft = 0;
		float posTop = 0;
		Rect tempRec = CalculateScreenArea(width, height);
		
		float wPC = tempRec.width;
		float hPC = tempRec.height;
		
		switch(ePosition)
		{
			case eScreenPos.TopLeft:
				posLeft 	= borderHor;
				posTop 		= borderVert;
				break;
				
		    case eScreenPos.TopCenter:
	            posLeft     = (Screen.width / 2) - (wPC / 2);
	            posTop      = borderVert;
	            break;
	
	        case eScreenPos.TopRight:
	            posLeft     = Screen.width - wPC - borderHor;
	            posTop      = borderVert;
	            break;
	
	        case eScreenPos.MiddleLeft:
	            posLeft     = borderHor;
	            posTop      = (Screen.height / 2) - (hPC / 2);
	            break;
	
	        case eScreenPos.MiddleCenter:
	            posLeft     = (Screen.width / 2) - (wPC / 2);
	            posTop      = (Screen.height / 2) - (hPC / 2);
	            break;
	
	        case eScreenPos.MiddleRight:
	            posLeft     = Screen.width - wPC - borderHor;
	            posTop      = (Screen.height / 2) - (hPC / 2);
	            break;   
	
	        case eScreenPos.BottomLeft:
	            posLeft     = borderHor;
	            posTop      = Screen.height - hPC - borderVert;
	            break;
	
	        case eScreenPos.BottomCenter:
	            posLeft     = (Screen.width / 2) - (wPC / 2);
	            posTop      = Screen.height - hPC - borderVert;
	            break;
	
	        case eScreenPos.BottomRight:
	            posLeft     = Screen.width - wPC - borderHor;
	            posTop      = Screen.height - hPC - borderVert;
	            break;
		}
		
		_area = new Rect(posLeft, posTop, wPC, hPC);
		
		return _area;
		
	}
	
	public Rect CalculateScreenArea(int w, int h)
	{
		Rect rec = new Rect(0 ,0 ,w, h);
		
		if(Screen.height < 600)
		{
			rec.width = Mathf.FloorToInt(Screen.width * (1.0f / (800.0f / w)));
			rec.height = Mathf.FloorToInt(Screen.height * (1.0f / (600.0f / h)));
		}
		
		return rec;
	}

}

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUITexture))]
public class JoystickControl : MonoBehaviour {
	public Vector2 Offset;
	public Vector3 movementVector;
	private Rect defaultPos;
	private GUITexture joystickTexture;
	
	private Vector2 touchOffset;
	private Vector2 joystickCenterPos;
	
	private Boundary guiBoundary;
	private float lengthOfVector;
	
	// Use this for initialization
	void Start () {
		guiBoundary = new Boundary();
		joystickTexture = gameObject.guiTexture;
		defaultPos = new Rect(Screen.width - joystickTexture.pixelInset.width + Offset.x, 
								0 + Offset.y, 
								joystickTexture.pixelInset.width, 
								joystickTexture.pixelInset.height);
		
		joystickTexture.pixelInset = defaultPos;
		
		touchOffset = new Vector2(defaultPos.width * 0.5f, defaultPos.height * 0.5f);
		
		guiBoundary.min.x = defaultPos.x - touchOffset.x;
		guiBoundary.min.y = defaultPos.y - touchOffset.y;
		guiBoundary.max.x = defaultPos.x + touchOffset.x;
		guiBoundary.max.y = defaultPos.y + touchOffset.y;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.touchCount == 0) ResetJoystick();
		
		
		foreach(Touch touch in Input.touches)
		{
			Vector2 touchPos = touch.position - touchOffset;
			if(joystickTexture.HitTest(touch.position)) 
			{
				Rect newPixelInset = new Rect(Mathf.Clamp(touchPos.x, guiBoundary.min.x, guiBoundary.max.x), 
											Mathf.Clamp(touchPos.y, guiBoundary.min.y, guiBoundary.max.y), 
											joystickTexture.pixelInset.width, 
											joystickTexture.pixelInset.height);
				joystickTexture.pixelInset = newPixelInset;
				
				
				movementVector = new Vector3((touch.position.x - defaultPos.x) / touchOffset.x, (touch.position.y - defaultPos.y) / touchOffset.y, 0);
			
				if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) ResetJoystick();
			}
		}
		
	}
	
	void ResetJoystick()
	{
		joystickTexture.pixelInset = defaultPos;
		movementVector = Vector3.zero;
	}
	
	struct Boundary {
		public Vector2 min;
		public Vector2 max;
	}
}


//	if(touch.phase != TouchPhase.Ended)
//			{
//				joystickTexture.transform.position = new Vector3(touch.position.x / Screen.width, touch.position.y / Screen.height, 0);
//			} else
//			{
//				joystickTexture.transform.position = Vector3.zero;
//			}
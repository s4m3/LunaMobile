using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AGPlayerController : MonoBehaviour
{
	public AGCamera AGCam;
	public AGPlayerInfo info;
	public int PlayerID;
	public bool is_AI_Player = false;
	static private int m_KM_PlayerID;
	static private int m_KM_CurrentPlayerID;
	static private int m_KM_MaxPlayerID;
	static private bool m_KM_UseKM;
	public float MovementInputDeadZone;
	public GameObject playerInterfacePrefab;
	public AGPlayerInterface playerInterface;
	public GameObject playerMenuPrefab;
	public PlayerMenu playerMenu;
	public GameObject characterChoiceMenuPrefab;
	public CharacterChoiceMenu charChoiceMenu;
	public AGPawn pawn;
	public AGAction_Shot Action_Shot;
	public AGAction_Dash Action_Dash;
    public AGAction_Ultimate Action_Ultimate;
    public AGAction_Melee Action_Melee;
    public AcceControlTest AccelerometerControl;
	public GameObject Joystick;
	private JoystickControl joystickController;
	public bool playerReady;
	public bool characterChosen;
	private bool bIsControllable;

	// Use this for initialization
	void Start ()
	{
		AGGame.Instance.SetPlayerController (this);    
		if(!is_AI_Player) 
		{
			AccelerometerControl = gameObject.AddComponent<AcceControlTest>();
			joystickController = Joystick.GetComponent<JoystickControl>();			
		}

	}

	public void SetRoundStart ()
	{
		playerReady = false;
 
		if (!playerMenu) {
			GameObject obj = (GameObject)GameObject.Instantiate (playerMenuPrefab);
			playerMenu = obj.GetComponent<PlayerMenu> ();
			playerMenu.SetPlayer (this);
          
		}
	}
	
	public void SetCharacterChoice()
	{
		characterChosen = false;
		
		if(!charChoiceMenu)
		{
			GameObject obj = (GameObject) GameObject.Instantiate(characterChoiceMenuPrefab);
			charChoiceMenu = obj.GetComponent<CharacterChoiceMenu>();
			charChoiceMenu.SetPlayer(this);
		}
	}
	
	public void SetCharacterChoiceBackgroundPlanet(Transform basePlanet, AGSpawnPoint[] spawnpoints)
	{
		if(!AGCam) return;
		AGCam.SetPlanetBackground(basePlanet, spawnpoints[PlayerID - 1].transform.position);
	}
	
	public void SetInterface()
	{
		if (!playerInterface) {
			GameObject obj = (GameObject)GameObject.Instantiate (playerInterfacePrefab);
			playerInterface = obj.GetComponent<AGPlayerInterface> ();
			playerInterface.SetPlayer (this);
		}
	}
	
	public void InitializePlayer ()
	{
		AGGame.Instance.SetPlayer (PlayerID, this);

		//(playerMenu);
		
		m_KM_PlayerID = PlayerID; // FIXME: Find right place for this line ...
		m_KM_CurrentPlayerID = m_KM_PlayerID;
		m_KM_MaxPlayerID = m_KM_PlayerID + 1;
//		Print.Log ("ID: " + m_KM_PlayerID + " max: " + m_KM_MaxPlayerID);
		m_KM_UseKM = false;
	}
	

	
//	// Update is called once per frame
//	void FixedUpdate ()
//	{
//		//  if(Input.GetButtonDown("Fire3_p2")) print("Input");
//		if (bIsControllable)
//			CheckInputs ();
//		
//	}

	public void SetPawn (AGPawn p)
	{
//		Print.Log ("SetPawn " + PlayerID);
		pawn = p;
		pawn.Player = this;

		pawn.SetLightZoneStats (true);
        pawn.SetLightZoneMesh(false);

		// FIXME: do something with renderer, subrenderer, really add color? 
//		pawn.mesh.renderer.material = info.PlayerMaterial;
//		var renderers = pawn.mesh.GetComponentsInChildren<UnityEngine.Renderer> ();
//		foreach (var ren in renderers) {
//			ren.material.color += info.PlayerMaterial.color;
//		}
		pawn.AimHelp.renderer.material = info.AimHelpMaterial;
    
		pawn.AimHelp.layer = 8 + PlayerID;
		if (p.MyProjector != null) {
			//Debug.Log ("Setting Projector Material Player " + PlayerID);
			p.MyProjector.material = info.PlayerProjectorMaterial;
		}

		Action_Shot.SetShotDisplay ();
		
		//MOBILE: HOTFIX
		if(PlayerID == 1)AGCam.SetViewTarget (pawn);		
	}

	bool PlayerCanMovePawn ()
	{
        return true;
	}

	void Update ()
	{
		if (!bIsControllable)
			return;

		CheckInputs ();
		//this.pawn.ModifyHealth(-3.5f);
	}
	
	void LateUpdate ()
	{
		m_KM_PlayerID = m_KM_CurrentPlayerID;
	}
	
	bool hasThisPlayer (bool value)
	{
		if (isThisPlayer () && value && m_KM_UseKM)
			return true;
		return false;
	}
	
	bool isThisPlayer ()
	{
		if (PlayerID == m_KM_PlayerID)
			return true;
		return false;
	}
	
	void controlMobileButtons()
	{
		if (Input.touchCount > 4){
            ActivateAction(Action_Ultimate);
        } else if (Input.touchCount > 3){
			ActivateAction (Action_Melee);
		} else if (Input.touchCount > 2) {
			ActivateAction (Action_Dash);
		} else if (Input.touchCount > 1) {
			ActivateAction (Action_Shot);
		}

		if(Action_Ultimate.bHoldButton)
            Action_Ultimate.ReleaseButton();
		
		if (Action_Shot.bHoldButton) 
			Action_Shot.ReleaseButton ();
		
		if (Action_Melee.bHoldButton)
			Action_Melee.ReleaseButton ();
	
		if (Action_Dash.bHoldButton) 
			Action_Dash.ReleaseButton ();
	}
	
	void controlMobileAxis()
	{

		//Vector3 InputVectorMovement = new Vector3 (horizontal, vertical, 0);
		Vector3 InputVectorMovement = joystickController.movementVector.normalized;
		Vector3 InputVectorLook = AccelerometerControl.movementVector;

		if (pawn != null) {
			if (PlayerCanMovePawn ()) {
				float MovementInputPercent = Mathf.Clamp ((InputVectorMovement.magnitude - MovementInputDeadZone), 0, 1) / (1 - MovementInputDeadZone);
				InputVectorMovement *= MovementInputPercent;
				Vector3 MoveDirection = AGCam.transform.rotation * InputVectorMovement;
				pawn.UpdateMoveDirection (Tools.CameraVectorToObject (MoveDirection, pawn.gameObject, AGCam));
				Debug.DrawRay (pawn.transform.position, Tools.CameraVectorToObject (MoveDirection, pawn.gameObject, AGCam), Color.cyan); 
			
			}

			if (InputVectorLook.magnitude < MovementInputDeadZone)
				InputVectorLook = Vector3.zero;
			Vector3 LookDirection = AGCam.transform.rotation * InputVectorLook;
			Debug.DrawRay (pawn.transform.position, Tools.CameraVectorToObject (LookDirection, pawn.gameObject, AGCam), Color.magenta);            
			pawn.SetLookDirection (Tools.CameraVectorToObject (LookDirection, pawn.gameObject, AGCam));	
		}
	}
	void controlButtons ()
	{
		if (hasThisPlayer (Input.GetButton ("KM_Shot")) || 
			Mathf.Abs (Input.GetAxisRaw ("Fire1_p" + PlayerID)) > 0.5f || 
			Input.GetButton("FireA_p"+PlayerID)){
			// Debug.Break();
			ActivateAction (Action_Shot);
		} else if (Action_Shot.bHoldButton) {
			Action_Shot.ReleaseButton ();
		}
		
		if (hasThisPlayer (Input.GetButtonDown ("KM_SwitchPlayer"))) {
			m_KM_CurrentPlayerID = ((m_KM_PlayerID + 1) % m_KM_MaxPlayerID == 0) ? 1 : m_KM_PlayerID + 1;
		}
		
		if (isThisPlayer () && Input.GetButtonDown ("KM_SwitchKM")) {
			m_KM_UseKM = !m_KM_UseKM;
		}


		if (hasThisPlayer (Input.GetButton ("KM_Dash")) ||
			Input.GetButton ("Fire2_p" + PlayerID) || 
			Input.GetButton("FireB_p"+PlayerID)) {
			ActivateAction (Action_Dash);
		} else if (Action_Dash.bHoldButton)
			Action_Dash.ReleaseButton ();

		if (hasThisPlayer (Input.GetButton ("KM_Melee")) ||
			Input.GetButton ("Fire3_p" + PlayerID) || 
			Input.GetButton("FireX_p"+PlayerID)) {
			ActivateAction (Action_Melee);
		} else if (Action_Melee.bHoldButton)
			Action_Melee.ReleaseButton ();

        if (hasThisPlayer (Input.GetButton ("KM_Ultimate")) || 
			Input.GetButton("FireY_p"+PlayerID))
        {
            ActivateAction(Action_Ultimate);
        }
        else if(Action_Ultimate.bHoldButton)
            Action_Ultimate.ReleaseButton();
	}
	
	void controlAxis ()
	{
		float horizontal = Input.GetAxis ("Horizontal_p" + PlayerID);
		float horizontalLook = Input.GetAxis ("HorizontalLook_p" + PlayerID);
		float vertical = Input.GetAxis ("Vertical_p" + PlayerID);
		float verticalLook = -Input.GetAxis ("VerticalLook_p" + PlayerID);
		if (hasThisPlayer (true)) {
//			Print.Log(Screen.width + " x "  + Screen.height);
			float ycenter = Screen.height / 2.0f;
			float xcenter = Screen.width / 2.0f;
			float xPlaceCenter = xcenter / 2.0f;
			float xPlace = (PlayerID - 1) * xcenter + xPlaceCenter; // ||  x  |  o  ||
//			Print.Log(xcenter + " " + xPlaceCenter + " " + xPlace + "    " + Input.mousePosition.x);
//			Print.Log (Input.mousePosition);
			horizontal += Input.GetAxis ("KM_Horizontal"); // A-D
			horizontalLook += Input.mousePosition.x - xPlace; // Input.GetAxis ("KM_HorizontalLook"); // Mouse
			vertical += Input.GetAxis ("KM_Vertikal"); // W-S
			verticalLook += Input.mousePosition.y - ycenter; // Input.GetAxis ("KM_VertikalLook"); // Mouse
		}
		
		Vector3 InputVectorMovement = new Vector3 (horizontal, vertical, 0);
		Vector3 InputVectorLook = new Vector3 (horizontalLook, verticalLook, 0);

		if (pawn != null) {
			if (PlayerCanMovePawn ()) {
				float MovementInputPercent = Mathf.Clamp ((InputVectorMovement.magnitude - MovementInputDeadZone), 0, 1) / (1 - MovementInputDeadZone);
				InputVectorMovement *= MovementInputPercent;
				Vector3 MoveDirection = AGCam.transform.rotation * InputVectorMovement;
				pawn.UpdateMoveDirection (Tools.CameraVectorToObject (MoveDirection, pawn.gameObject, AGCam));
				Debug.DrawRay (pawn.transform.position, Tools.CameraVectorToObject (MoveDirection, pawn.gameObject, AGCam), Color.cyan); 
			
			}

			if (InputVectorLook.magnitude < MovementInputDeadZone)
				InputVectorLook = Vector3.zero;
			Vector3 LookDirection = AGCam.transform.rotation * InputVectorLook;
			Debug.DrawRay (pawn.transform.position, Tools.CameraVectorToObject (LookDirection, pawn.gameObject, AGCam), Color.magenta);            
			pawn.SetLookDirection (Tools.CameraVectorToObject (LookDirection, pawn.gameObject, AGCam));	
		}
	}
	
	void CheckInputs () // FIXME TODO Rewrite for use with Update !!
	{   
		//MOBILE
//		controlButtons ();
//		controlAxis ();     
       	if(!is_AI_Player)
		{
			controlMobileButtons();
			controlMobileAxis();
		}	
//		//TODO: Refactor into Generic System
//		if (Mathf.Abs (Input.GetAxisRaw ("Fire1_p" + PlayerID)) > 0.5f) {
//			// Debug.Break();
//			ActivateAction (Action_Shot);
//		} else if (Action_Shot.bHoldButton) {
//			Action_Shot.ReleaseButton ();
//		}
//		
//		if (Input.GetButtonDown ("KM_SwitchPlayer")) {
//			Print.Log ("KM_SW: " + m_KM_PlayerID);
//			m_KM_PlayerID = ((m_KM_PlayerID + 1) % m_KM_MaxPlayerID == 0) ? 1 : m_KM_PlayerID + 1;
//			Print.Log ("KM_SW_2: " + m_KM_PlayerID);
//		}
//
//
//		if (Input.GetButton ("Fire2_p" + PlayerID)) {
//			ActivateAction (Action_Dash);
//		} else if (Action_Dash.bHoldButton)
//			Action_Dash.ReleaseButton ();
//
//		if (Input.GetButton ("Fire3_p" + PlayerID)) {
//			ActivateAction (Action_Melee);
//		} else if (Action_Melee.bHoldButton)
//			Action_Melee.ReleaseButton ();
			
		//MOVEMENTS
		//TODO: keyboard input for testing
		//Vector3 InputVectorMovement = new Vector3(Input.GetAxis("HorizontalKeyboard"), Input.GetAxis("VerticalKeyboard"));
		//Direct Input
//		Vector3 InputVectorMovement = new Vector3 (Input.GetAxis ("Horizontal_p" + PlayerID), Input.GetAxis ("Vertical_p" + PlayerID), 0);
//		Vector3 InputVectorLook = new Vector3 (Input.GetAxis ("HorizontalLook_p" + PlayerID), -Input.GetAxis ("VerticalLook_p" + PlayerID), 0);
//
//		if (pawn != null) {
//			if (PlayerCanMovePawn ()) {
//				float MovementInputPercent = Mathf.Clamp ((InputVectorMovement.magnitude - MovementInputDeadZone), 0, 1) / (1 - MovementInputDeadZone);
//				InputVectorMovement *= MovementInputPercent;
//				Vector3 MoveDirection = AGCam.transform.rotation * InputVectorMovement;
//				pawn.UpdateMoveDirection (Tools.CameraVectorToObject (MoveDirection, pawn.gameObject, AGCam));
//				Debug.DrawRay (pawn.transform.position, Tools.CameraVectorToObject (MoveDirection, pawn.gameObject, AGCam), Color.cyan); 
//			
//			}
//
//			if (InputVectorLook.magnitude < MovementInputDeadZone)
//				InputVectorLook = Vector3.zero;
//			Vector3 LookDirection = AGCam.transform.rotation * InputVectorLook;
//			Debug.DrawRay (pawn.transform.position, Tools.CameraVectorToObject (LookDirection, pawn.gameObject, AGCam), Color.magenta);            
//			pawn.SetLookDirection (Tools.CameraVectorToObject (LookDirection, pawn.gameObject, AGCam));	
//		}
	}
	
	public bool ActivateAction (AGAction action)
	{
		if (CanActivateAction (action) && pawn.AllowsAction ()) {
			return action.Activate ();
		} else {
			return false;	
		}
	}
	
	public void CleanUp()
	{
		if(pawn) Destroy (pawn.gameObject);
		if(Action_Shot)
		{
			Action_Shot.DestroyShotDisplay();
			Destroy (Action_Shot.gameObject);
		}
		if(Action_Dash) Destroy (Action_Dash.gameObject);
		if(Action_Ultimate) Destroy (Action_Ultimate.gameObject);
		if(Action_Melee) Destroy (Action_Melee.gameObject);
		if(playerInterface) 
		{
			playerInterface.DestroyVignette();
			Destroy(playerInterface.gameObject);
		}
	}

	protected bool CanActivateAction (AGAction action)
	{
		//Maybe check Gamestate or other stuff if an Action can be Performed
		return pawn != null;
	}

	public void PawnDied ()
	{
		//Action_Dash.Deactivate();
		AGGame.Instance.PlayerDied (PlayerID);
		// Action_Ultimate.Deactivate();
		//AGGame.Instance.SpawnPlayer(this);
	}
	
	public void NotifyGameState (AGGame.GameState gameState)
	{
		switch (gameState) {
		case AGGame.GameState.Running:
			bIsControllable = true;
			break;
		case AGGame.GameState.RoundOver:
			bIsControllable = false;
			pawn.UpdateMoveDirection (Vector3.zero);
            Action_Dash.Reset();
            Action_Melee.Reset();
            Action_Ultimate.Reset();
            Action_Shot.Reset();
			break;
		case AGGame.GameState.GameOver:
			CleanUp();
			break;
		default:
			bIsControllable = false;
			break;
		}
	}
}

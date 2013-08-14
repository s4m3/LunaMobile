using UnityEngine;
using System.Collections;

public class AcceControlTest : MonoBehaviour {

	public Vector3 movementVector;
	
	private Matrix4x4 calibrationMatrix;
	private float AccelerometerUpdateInterval;
	private float LowPassKernelWidthInSeconds;
	private float LowPassFilterFactor;
	private Vector3 lowPassValue;
	
	void Start () {
		calibrationMatrix = Matrix4x4.identity;
		
		AccelerometerUpdateInterval = 1.0f / 60.0f;
		LowPassKernelWidthInSeconds = 0.1f;
		
		LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
		lowPassValue = Input.acceleration;
		
		CalibrateAccelerometer();
	}
	// Update is called once per frame
	void Update () {
		movementVector = Vector3.zero;
		//Get input off accelerometer (multiplied with calibration matrix to get correct move)
		Vector3 fixedAcceleration = FixAcceleration(LowPassFilterAccelerometer());
		movementVector.x = fixedAcceleration.x;
		movementVector.y = fixedAcceleration.y;
		
		//Eliminate very small movement, to make control more smooth
		if(movementVector.magnitude < 0.1) return;
		movementVector.Normalize();

	}
	
	private Vector3 FixAcceleration(Vector3 accelerator)
	{
		return calibrationMatrix.MultiplyVector(accelerator);
	}
	
	private Vector3 LowPassFilterAccelerometer() {
		lowPassValue.x = Mathf.Lerp(lowPassValue.x, Input.acceleration.x, LowPassFilterFactor);
		lowPassValue.y = Mathf.Lerp(lowPassValue.y, Input.acceleration.y, LowPassFilterFactor);
		lowPassValue.z = Mathf.Lerp(lowPassValue.z, Input.acceleration.z, LowPassFilterFactor);
		return lowPassValue;
	}
	
	//Used to calibrate the initial Input.acceleration input
	public void CalibrateAccelerometer() {
	   	Vector3 accelerationSnapshot = Input.acceleration;
	   	Quaternion rotateQuaternion = Quaternion.FromToRotation(-Vector3.forward, accelerationSnapshot);
	
	   	//create identity matrix ... rotate our matrix to match up with down vec
	   	Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, Vector3.one);
	   	//get the inverse of the matrix
	   	this.calibrationMatrix = matrix.inverse;
	}
}



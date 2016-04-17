using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Player target;
	public Vector3 offset, disguiseOffset;

	public bool lookRotation;
	public float lookSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (UIManager.stopMovement)
			return;

		if (lookRotation)
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, target.transform.rotation.eulerAngles.y + (target.disguise.isActive ? 90:0) , target.transform.rotation.eulerAngles.z);
		else
			transform.rotation = target.transform.rotation;

		transform.position = target.transform.position + target.transform.TransformVector(target.disguise.isActive ? disguiseOffset : offset);
		
		transform.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * -lookSpeed);
	}
}

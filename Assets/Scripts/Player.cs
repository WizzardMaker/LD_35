using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Disguise))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {

	CharacterController cc;

	public List<GameObject> disguises;

	[HideInInspector]
	public Disguise disguise;

	public float lookSpeed;

	public float speed;
	public float jumpHeight;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
		disguise = GetComponent<Disguise>();
	}

	bool isJumping = false;

	Vector3 grav = Physics.gravity;

	void Move() {
		Vector3 vel = Vector3.zero;
		grav += Physics.gravity * Time.deltaTime;

		if (disguise.isActive) {
			vel += transform.right * Input.GetAxis("Vertical") * speed;
			vel += -transform.forward * Input.GetAxis("Horizontal") * speed;
		} else {
			vel += transform.forward * Input.GetAxis("Vertical") * speed;
			vel += transform.right * Input.GetAxis("Horizontal") * speed;
		}

		if (!isJumping && Input.GetButtonDown("Jump")) {
			grav = transform.up * jumpHeight;
			isJumping = true;
		}
		if (cc.isGrounded)
			isJumping = false;

		vel += grav;

		cc.Move(vel * Time.deltaTime);
	}

	// Update is called once per frame
	void Update() {
		Move();

		transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * lookSpeed);

		if (Input.GetButtonDown("Fire1")) {
			GameObject temp;
            if ((temp = Disguise.GetDisguise(Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2)))) != null) {
				if(!disguises.Contains(temp))
					disguises.Add(temp);
			}
		}

	}
}

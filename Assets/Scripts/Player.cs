using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Disguise))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {

	[HideInInspector]
	public CharacterController cc;

	public GameObject[] disguises = new GameObject[10];

	[HideInInspector]
	public Disguise disguise;

	public bool canMove;

	public float lookSpeed;
	public float supicion, supicionCoolrate;
	public float maxSupicion, minSupicion;

	public float speed, sprintSpeed;
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

		bool isSprinting = Input.GetKey(KeyCode.LeftShift);

		if (disguise.isActive) {
			vel += transform.right * Input.GetAxis("Vertical") * speed * (isSprinting ? sprintSpeed : 1);
			vel += -transform.forward * Input.GetAxis("Horizontal") * speed * (isSprinting ? sprintSpeed : 1);
		} else {
			vel += transform.forward * Input.GetAxis("Vertical") * speed * (isSprinting ? sprintSpeed : 1);
			vel += transform.right * Input.GetAxis("Horizontal") * speed * (isSprinting ? sprintSpeed : 1);
		}

		if (!isJumping && Input.GetButtonDown("Jump")) {
			grav = transform.up * jumpHeight;
			isJumping = true;
		}
		if (cc.isGrounded)
			isJumping = false;

		if (!canMove)
			vel = Vector3.zero;

		vel += grav;

		cc.Move(vel * Time.deltaTime);
	}

	void AddDisguise(GameObject go, int i){
		if (disguises[i] == (go))
			return;

		disguises[i] = go;

		UIManager.active.UpdateDisguises(disguises);
	}

	// Update is called once per frame
	void Update() {
		
		Move();

		transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * lookSpeed);

		if (Input.GetButtonDown("Fire1") && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
			GameObject temp;
            if ((temp = Disguise.GetDisguise(Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2)))) != null) {
				AddDisguise(temp,UIManager.active.activeDisguise);
            }
		}
		disguise.SetDisguise( disguises[UIManager.active.activeDisguise]);


		supicion -= supicionCoolrate * Time.deltaTime;
		supicion = Mathf.Clamp(supicion, minSupicion, maxSupicion);
		
	}
}

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
	public float suspicion, supicionCoolrate;
	public float maxSupicion, minSupicion;

	public float speed, sprintSpeed;
	public float jumpHeight;

	public Unit target;
	public int targetsLeft;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
		disguise = GetComponent<Disguise>();


		target = FindObjectsOfType<Unit>()[Random.Range(0,FindObjectsOfType<Unit>().Length)];
		target.MakeTarget();
	}

	bool isJumping = false;

	Vector3 grav = Physics.gravity;

	void SetNewTarget() {
		targetsLeft--;
		if(targetsLeft <= 0) {
			Debug.Log("All targets dead!");
			return;
		}
		UIManager.anim.SetTrigger("switchList");
		UIManager.anim.SetBool("showList", true);

		target = FindObjectsOfType<Unit>()[Random.Range(0, FindObjectsOfType<Unit>().Length)];
		target.MakeTarget();
	}


	public AudioSource runningSound, walkingSound;
	bool SprintSoundOn, WalkSound;

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

		
		
		if (!canMove)
			vel = Vector3.zero;

		if(vel != Vector3.zero) {
			if (isSprinting) {
				runningSound.UnPause();
				walkingSound.Pause();
			} else {
				walkingSound.UnPause();
				runningSound.Pause();
			}
		} else {
			runningSound.Pause();
			walkingSound.Pause();
		}
		if (cc.isGrounded)
			isJumping = false;
		else {
			runningSound.Pause();
			walkingSound.Pause();
		}
		if (!isJumping && Input.GetButtonDown("Jump")) {
			grav = transform.up * jumpHeight;
			isJumping = true;
		}
		vel += grav;

		cc.Move(vel * Time.deltaTime);
	}

	void AddDisguise(GameObject go, int i){
		if (disguises[i] == (go))
			return;

		disguises[i] = go;
		UIManager.active.UpdateDisguises(disguises);
	}

	void KillTarget(Unit target) {
		target.Kill(this);

	}
	
	int curLayer;
	float suspicionToAdd;
	public void AddSupicion(float suspicion, int layer) {
		if(curLayer < layer) {
			curLayer = layer;

			suspicionToAdd = suspicion;
		}
	}

	public float seenSuspicion;

	// Update is called once per frame
	void Update() {
		if (UIManager.stopMovement) {
			runningSound.Pause();
			walkingSound.Pause();

			return;
		}
#if !DEBUG
		Cursor.lockState = CursorLockMode.Locked;
#endif

		if (curLayer != -1) {
			suspicion += suspicionToAdd;

			curLayer = -1;
		}

		if (SniperAI.isInView(gameObject) && !disguise.isActive) {
			suspicion += seenSuspicion * Time.deltaTime;
		}


		Move();

		transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * lookSpeed);

		if (Input.GetButtonDown("Fire1") && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
			GameObject temp;
            if ((temp = Disguise.GetDisguise(Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2)))) != null) {
				AddDisguise(temp,UIManager.active.activeDisguise);
            }
		}
		disguise.SetDisguise( disguises[UIManager.active.activeDisguise]);


		suspicion -= supicionCoolrate * Time.deltaTime;
		suspicion = Mathf.Clamp(suspicion, minSupicion, maxSupicion);

		if(target == null) {
			SetNewTarget();
			targetsLeft++;
        }

		if (Input.GetButtonDown("Fire1") && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, 2, 1 << 10)) {

				Unit hitter = hit.collider.GetComponent<Unit>();

				if (hitter == null)
					throw (new System.Exception("Target Unit has no Unit class!"));

				if (hitter.isSpecial) {
					Debug.Log("Got your target");

					SetNewTarget();
				}

				KillTarget(hitter);
			}
		}
	}
}

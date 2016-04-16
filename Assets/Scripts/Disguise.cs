using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class Disguise : MonoBehaviour {

	Player owner;



	public bool isActive;
	public float coolDown;

	MeshFilter mf;

	public GameObject disguise {
		get;
		private set;
	}

	Mesh disguiseMesh;
	Mesh baseMesh;

	public static GameObject GetDisguise(Ray ray) {
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction*5,Color.red,15,false);

		Debug.Log("GetDisguise!");
		if (Physics.Raycast(ray, out hit)) {
			if (hit.transform.gameObject.GetComponent<DisguiseInfo>())
				return hit.transform.gameObject;
		}
		

        return null;
	}

	public bool canHide;

	public void SetDisguise(GameObject disguise, out bool canHide) {
		canHide = true;

		if (disguise == null) {
			canHide = false;
			this.canHide = canHide;
			return;
		}

		this.canHide = canHide;
		ApplyObject(disguise);
	}

	DisguiseInfo di;


	float baseRadius = 0.5f, baseHeight = 1f;
	Vector3 baseCenter = Vector3.zero;

    void SetCCCollider() {
		if (isActive) {
			owner.cc.radius = di.colliderRadius;
			owner.cc.height = di.colliderHeight;
			owner.cc.center = di.colliderCenter;
		} else {
			owner.cc.radius = baseRadius;
			owner.cc.height = baseHeight;
			owner.cc.center = baseCenter;
		}
	}

	void ApplyObject(GameObject disguise) {
		this.disguise = disguise;
		disguiseMesh = disguise.GetComponent<MeshFilter>().mesh;

		di = disguise.GetComponent<DisguiseInfo>();

		neededProgress = di.disguiseTime;
		

		GetComponent<MeshCollider>().sharedMesh = disguiseMesh;


	}
	public void SetDisguise(GameObject disguise) {
		if (disguise == null) {
			canHide = false;
			return;
		}

		canHide = true;
		ApplyObject(disguise);
	}

	// Use this for initialization
	void Start () {
		mf = GetComponent<MeshFilter>();
		owner = GetComponent<Player>();
		baseMesh = mf.mesh;



		if (disguise != null)
			SetDisguise(disguise);
	}

	bool isDisguising;
	public float progress;
	public float neededProgress;

	// Update is called once per frame
	void Update () {
		SetCCCollider();
		if (isActive) {
			if (disguiseMesh == null)
				throw (new System.NullReferenceException("disguiseMesh is null, what the hell?!"));

			mf.mesh = disguiseMesh;
			GetComponent<MeshCollider>().sharedMesh = disguiseMesh;

		} else {
			mf.mesh = baseMesh;
			GetComponent<MeshCollider>().sharedMesh = baseMesh;
		}

		if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && canHide) {
			isDisguising = true;
			owner.canMove = false;
			UIManager.active.isFixed = true;

			progress += Time.deltaTime;

			if(progress >= neededProgress) {
				progress = neededProgress;
				isActive = true;
			}

		}else if (isDisguising) {
			if (isActive) {
				progress -= Time.deltaTime;
				if (progress <= 0)
					isActive = false;
			} else {
				isDisguising = false;
				progress = 0;
				owner.canMove = true;
				UIManager.active.isFixed = false;
			}
		}
	}
}

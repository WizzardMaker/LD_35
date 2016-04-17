using UnityEngine;
using System.Collections;

public enum DisguiseType {
	PottedPlant,
	Plant,
	Tree,
	Trash,
	TrashCan,
	PicknickCarpet,
	Grass,
	Car,
	StreetLamp,

}

public class Disguise : MonoBehaviour {

	Player owner;

	public DisguiseType type;

	public float supicionLevel;

	public bool isActive;
	public float coolDown;

	new GameObject renderer;

	MeshFilter mf;
	MeshRenderer mr;
	MeshRenderer disguiseRenderer;
	Quaternion rotation;
	Vector3 scale;

	public GameObject disguise {
		get;
		private set;
	}

	Mesh disguiseMesh;

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

		rotation = disguise.transform.rotation;
		scale = disguise.transform.lossyScale;

		disguiseRenderer = disguise.GetComponent<MeshRenderer>();

		neededProgress = di.disguiseTime;

		type = di.type;
		supicionLevel = di.supicionLevel;
		


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
		mf = GetComponentsInChildren<MeshFilter>()[1];
		mr = GetComponentsInChildren<MeshRenderer>()[1];

		owner = GetComponent<Player>();

		renderer = transform.GetChild(0).gameObject;

		if (disguise != null)
			SetDisguise(disguise);
	}

	bool isDisguising;
	public float progress;
	public float neededProgress;


	// Update is called once per frame
	void Update () {
		if (UIManager.stopMovement)
			return;

		SetCCCollider();
		if (isActive) {
			if (disguiseMesh == null)
				throw (new System.NullReferenceException("disguiseMesh is null, what the hell?!"));

			renderer.transform.localScale = scale;
			renderer.transform.rotation = rotation;

			mr.materials = disguiseRenderer.materials;
			owner.GetComponent<MeshRenderer>().enabled = false;
            mf.mesh = disguiseMesh;
			mr.enabled = true;
			//GetComponentsInChildren<MeshCollider>()[1].sharedMesh = disguiseMesh;

		} else {
			mr.enabled = false;
			owner.GetComponent<MeshRenderer>().enabled = true;
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

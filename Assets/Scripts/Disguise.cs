using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class Disguise : MonoBehaviour {

	Player owner;



	public bool isActive;
	public float coolDown;

	MeshFilter mf;

	public GameObject disguise;

	Mesh disguiseMesh;
	Mesh baseMesh;

	public static GameObject GetDisguise(Ray ray) {
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction*5,Color.red,15,false);

		Debug.Log("GetDisguise!");
		if (Physics.Raycast(ray, out hit)) {
			return hit.transform.gameObject;
		}
		

        return null;
	}

	public void SetDisguise(GameObject disguise, bool hide = false) {
		if (disguise == null)
			throw (new System.ArgumentNullException("disguise is NULL, fix that!"));

		this.disguise = disguise;
		disguiseMesh = disguise.GetComponent<MeshFilter>().mesh;

		GetComponent<MeshCollider>().sharedMesh = disguiseMesh;

		isActive = hide;
	}

	// Use this for initialization
	void Start () {
		mf = GetComponent<MeshFilter>();
		owner = GetComponent<Player>();
		baseMesh = mf.mesh;

		if (disguise != null)
			SetDisguise(disguise);
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (disguiseMesh == null)
				throw (new System.NullReferenceException("disguiseMesh is null, what the hell?!"));

			mf.mesh = disguiseMesh;
			GetComponent<MeshCollider>().sharedMesh = disguiseMesh;

		} else {
			mf.mesh = baseMesh;
			GetComponent<MeshCollider>().sharedMesh = baseMesh;
			
		}
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	public int i = 0;
	public Canvas[] tutSlides;

	public void StartGame() {
		SceneManager.LoadScene("_test_");
	}
	public void Exit() {
		Application.Quit();
	}

	public void NextSlide(bool back) {
		tutSlides[i].gameObject.SetActive(false);
		i += back ? -1 : 1;
        tutSlides[i].gameObject.SetActive(true);
	}

	public void StartTutorial() {
		tutSlides[i = 0].gameObject.SetActive(true);
	}
	public void StopTutorial() {
		tutSlides[i].gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

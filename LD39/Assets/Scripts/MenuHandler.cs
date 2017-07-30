using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	public void QuitApplication () {
		Application.Quit ();
	}

	public void RestartGame () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}

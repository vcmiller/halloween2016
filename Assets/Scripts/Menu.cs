using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public void Play() {
        SceneManager.LoadScene("Scene", LoadSceneMode.Single);
    }

    public void Quit() {
        Application.Quit();
    }
}

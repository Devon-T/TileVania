using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float lvlExitSlowMoFactor = 0.2f;

    void OnTriggerEnter2D(Collider2D other)
    {
        print("touched next level trigger");
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel ()
    {
        print("called coroutine");
        Time.timeScale = lvlExitSlowMoFactor;
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        print("passed yield statement");
        Time.timeScale = 1f;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}


using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public Animator SceneTransition;

    private float transitionTime;

    // Start is called before the first frame update
    void Start()
    {
        transitionTime = SceneTransition.runtimeAnimatorController.animationClips[0].length; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonClick()
    {

        StartCoroutine(LevelTransition());
    }

    IEnumerator LevelTransition()
    {
        SceneTransition.SetTrigger("Start");
        AudioManager.Instance.PlayEffect("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("MainScene");
    }
}

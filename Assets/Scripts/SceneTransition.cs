using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition
{
    public static IEnumerator Run(Animator transitionAnimator, string targetScene)
    {
        float transitionTime = transitionAnimator.runtimeAnimatorController.animationClips[0].length;
        transitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(targetScene);
    }
}

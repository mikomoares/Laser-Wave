using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Animator animator;

    public void Play(){
        StartCoroutine(PlayCoroutine());
    }
    public void Back(){
        StartCoroutine(BackCoroutine());
    }
    private IEnumerator PlayCoroutine(){
        animator.SetTrigger("end");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    private IEnumerator BackCoroutine(){
        animator.SetTrigger("end");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

}

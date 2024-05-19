using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour_Zakarias : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void LoadScene()
    {
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(1);
    }
}

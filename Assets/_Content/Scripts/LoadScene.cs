using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private Canvas LoadSceneCanvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed = 0.5f;

    public void Load(int index)
    {
        StartCoroutine(LoadSceneCoroutine(index));
    }

    IEnumerator LoadSceneCoroutine(int index)
    {
        Debug.Log("load " + index);
        LoadSceneCanvas.enabled = true;
        canvasGroup.DOFade(1, fadeSpeed).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(fadeSpeed);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        canvasGroup.DOFade(0, fadeSpeed).SetEase(Ease.InCubic).OnComplete(() => { LoadSceneCanvas.enabled = false; });
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class StartScreenLoadingBar : MonoBehaviour
{
    [SerializeField] private Slider _progressBar;

    private void Start()
    {
        StartCoroutine(LoadingBar());
    }
    
    private IEnumerator LoadingBar()
    {  
        while(UnityServicesHandler.Instance.LoadProgress < 1.0f)
        {
            _progressBar.value = UnityServicesHandler.Instance.LoadProgress;
            yield return null;
        }
    
        SceneHandler.Instance.RequestStartSceneTransition("MainScene");
    }
}
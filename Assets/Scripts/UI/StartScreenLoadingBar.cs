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
        float progress = 0.0f;
        float targetProgress = 3.0f; // Load save file, connect to store, ad init

        while(progress < targetProgress)
        {
            progress = 0.0f;
            if (AdIntegrationHandler.Instance.SdkInitialized)
            {
                progress += 1.0f;
            }
            if (PlayerDataHandler.Instance.DataLoaded)
            {
                progress += 1.0f;
            }
            if (IAPHandler.Instance.StoreConnected)
            {
                progress += 1.0f;
            }
            _progressBar.value = progress / targetProgress;
            yield return null;
        }
    }

}

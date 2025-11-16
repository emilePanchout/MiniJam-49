using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingPage;
    public TMP_Text loadingText;

    public void StartGame()
    {
        loadingPage.SetActive(true);

        loadingPage.GetComponent<Image>().DOFade(1f, 1.5f)
                   .SetEase(Ease.Linear).OnComplete(() =>
                   {
                       loadingText.DOFade(1f, 1f)
                          .SetEase(Ease.Linear).OnComplete(() =>
                          {
                              loadingText.DOFade(0f, 1f)
                                 .SetEase(Ease.Linear).OnComplete(() =>
                                 {
                                     DOVirtual.DelayedCall(1, () =>
                                     {
                                         SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
                                     });
                                 });
                          });
                   });
   

    }
    public void QuitGame()
    {
        Application.Quit();
    }

    
}

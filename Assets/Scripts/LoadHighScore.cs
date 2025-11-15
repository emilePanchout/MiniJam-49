using UnityEngine;
using TMPro;

public class LoadHighScore : MonoBehaviour
{
    public TMP_Text text;
    private void Awake()
    {
        text.text = "Highscore : " + PlayerPrefs.GetFloat("score").ToString();
    }
}

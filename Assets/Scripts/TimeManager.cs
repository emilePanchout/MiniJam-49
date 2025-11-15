using UnityEngine;

public class TimeManager : MonoBehaviour
{

    [SerializeField] public float SceneTime;

    private void Update()
    {
        SceneTime = Time.timeSinceLevelLoad;
    }

    public float GetTimeScene()
    {
        return SceneTime;
    }
}

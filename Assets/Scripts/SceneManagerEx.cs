using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    InRound,
    Ending
}

public class SceneManagerEx : Singleton<SceneManagerEx>
{
    public SceneType CurrentSceneType 
        => (SceneType)SceneManager.GetActiveScene().buildIndex;


    public void LoadScene(SceneType type)
    {
        SceneManager.LoadScene((int) type);
        if (type == SceneType.InRound)
        {
            GameManager.Instance.StartRound();
        }
    }
}

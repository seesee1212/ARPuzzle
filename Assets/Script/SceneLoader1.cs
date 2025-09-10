using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader1 : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene"); // 여기에 이동할 씬 이름 입력
    }
}

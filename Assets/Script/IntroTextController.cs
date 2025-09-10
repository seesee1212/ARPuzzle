using UnityEngine;

public class IntroTextController : MonoBehaviour
{
    public GameObject introTextObject;

    void Start()
    {
        introTextObject.SetActive(true);
        Invoke("HideIntroText", 10f);
    }

    void HideIntroText()
    {
        introTextObject.SetActive(false);
    }
}


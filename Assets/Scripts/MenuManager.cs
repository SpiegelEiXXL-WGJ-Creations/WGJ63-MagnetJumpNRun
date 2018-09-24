using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject IntroScreen;
    public float TypePause = .5f;
    public Text typeText;
    private string fullText;

    public void GoToScene(string SceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

    public void Intro()
    {
        MainMenu.SetActive(false);
        IntroScreen.SetActive(true);
        StartCoroutine(PrintText());
    }

    IEnumerator PrintText()
    {
        foreach (char c in fullText)
        {
            typeText.text += c;
            yield return new WaitForSeconds(TypePause);
        }
        yield return null;
    }

    // Use this for initialization
    void Start()
    {
        fullText = typeText.text;
        typeText.text = "";

    }

    // Update is called once per frame
    void Update()
    {

    }
}

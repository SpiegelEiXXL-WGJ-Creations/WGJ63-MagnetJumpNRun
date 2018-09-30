using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject IntroScreen;
    public AudioSource MainMenuAudio;
    public AudioSource IntroAudio;
    public float FadeInOut = .5f;
    public float TypePause = .5f;
    public Text typeText;
    private string fullText;
    public bool isTransitioning;

    public void GoToScene(string SceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

    public void Intro()
    {
        isTransitioning = true;
        MainMenu.SetActive(false);
        IntroScreen.SetActive(true);
        IntroAudio.Play();
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
        isTransitioning = false;
        if (typeText != null)
        {
            fullText = typeText.text;
            typeText.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTransitioning)
        {
            MainMenuAudio.volume = Mathf.Lerp(MainMenuAudio.volume, 0f, FadeInOut * Time.deltaTime);
            IntroAudio.volume = Mathf.Lerp(IntroAudio.volume, 1f, FadeInOut * Time.deltaTime);
            if (MainMenuAudio.volume <= 0.01f)
                isTransitioning = false;
        }
    }
}

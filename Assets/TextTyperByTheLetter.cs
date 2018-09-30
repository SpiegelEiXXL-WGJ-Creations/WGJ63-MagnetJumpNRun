using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTyperByTheLetter : MonoBehaviour
{

    public string fullText;
    public UnityEngine.UI.Text textObj;
    public float TypePause = 0.01f;

    // Use this for initialization
    void Start()
    {
        textObj = GetComponent<UnityEngine.UI.Text>();
        fullText = textObj.text;
        textObj.text = "";
    }

    public void startPrintingText()
    {
        StartCoroutine(PrintText());
    }

    IEnumerator PrintText()
    {
        textObj.text = "";

        foreach (char c in fullText)
        {
            textObj.text += c;
            yield return new WaitForSeconds(TypePause);
        }
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text textComponent;

    [Header("Typing Settings")]
    [TextArea]
    [SerializeField] private string fullText;

    [SerializeField] private float delayBeforeStart = 0f;
    [SerializeField] private float timeBetweenLetters = 0.05f;

    private Coroutine typingCoroutine;

    private void Awake()
    {
        if (textComponent.text != null)
            fullText = textComponent.text;
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    public void StartTyping(string newText)
    {
        fullText = newText;
        StartTyping();
    }

    private IEnumerator TypeText()
    {
        textComponent.text = "";

        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }
}
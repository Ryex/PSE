using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeAndBlink : MonoBehaviour
{
    // Start is called before the first frame update

    float speed = 2.0f;

    private IEnumerator blinkEn;
    private Color originalColor;
    void Start()
    {
        Text text = GetComponent<Text>();
        originalColor = text.color;
        blinkEn = Blink(speed);
        StartCoroutine(blinkEn);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeOut() {
        StopCoroutine(blinkEn);
        Text text = GetComponent<Text>();
        text.color = originalColor;
        StartCoroutine(FadeOutAndDeactivate(0.5f));
    }
    IEnumerator FadeOutAndDeactivate(float aTime)
    {
        Text text = GetComponent<Text>();
        for (float t = 0.01f; t < aTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t/aTime));
            yield return null;
        }
        gameObject.SetActive(false);
    }

    IEnumerator Blink(float speed)
    {
        Text text = GetComponent<Text>();
        while (true)
        {
            text.color = Color.Lerp(originalColor, Color.clear, (Mathf.Sin(Time.time * speed) + 1.0f)/2.0f);
            yield return null;
        }
    }
}

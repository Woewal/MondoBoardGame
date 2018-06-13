using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float duration = 420;
    float currentTime;
    private Text timerText;
    public Button resetButton;

    RectTransform theBarRectTransform;

    public Image progressBar;

    public Image sun;
    public Image background;

    public Gradient sky;

    public AnimationCurve curve;

    // Use this for initialization
    void Start()
    {
        timerText = GetComponent<Text>();
        StartTimer();
        Button btn = resetButton.GetComponent<Button>();
        btn.onClick.AddListener(StartTimer);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentTime < duration)
        {
            RunTimer();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Targeting");
        }
        progressBar.fillAmount = currentTime / duration;

        sun.rectTransform.localPosition = new Vector3(Mathf.Lerp(-sun.rectTransform.rect.width/2, Screen.width + sun.rectTransform.rect.width / 2, currentTime/duration) - Screen.width/2 , -400 + curve.Evaluate(currentTime/duration)*1000);
        sun.rectTransform.Rotate(new Vector3(0, 0, .2f));

        background.color = sky.Evaluate(currentTime / duration);


    }

    void RunTimer()
    {
        currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString("f0");
        //print(myCountdowntimer);
    }

    public void StartTimer()
    {
        currentTime = 0;
    }
}

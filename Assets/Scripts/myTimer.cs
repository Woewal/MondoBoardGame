using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myTimer : MonoBehaviour
{

    public float StartTime = 420;
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
        ResetTimer();
        Button btn = resetButton.GetComponent<Button>();
        btn.onClick.AddListener(ResetTimer);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentTime > 0)
        {
            RunTimer();
        }
        progressBar.fillAmount = currentTime / StartTime;

        sun.rectTransform.localPosition = new Vector3(Mathf.Lerp(Screen.width + sun.rectTransform.rect.width/2, -sun.rectTransform.rect.width/2, currentTime/StartTime) - Screen.width/2 , -400 + curve.Evaluate(currentTime/StartTime)*1000);
        sun.rectTransform.Rotate(new Vector3(0, 0, .2f));

        background.color = sky.Evaluate(currentTime / StartTime);
    }

    void RunTimer()
    {
        currentTime -= Time.deltaTime;
        timerText.text = currentTime.ToString("f0");
        //print(myCountdowntimer);
    }

    public void ResetTimer()
    {
        currentTime = StartTime;
    }
}

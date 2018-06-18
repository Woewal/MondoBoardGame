using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float duration = 420;
    public float MondoEndSoundDuration = 2;
    float currentTime;
    private Text timerText;
    public Button resetButton;

    bool doShit = false;

    RectTransform theBarRectTransform;

    public Image progressBar;

    public Image sun;
    public Image background;

    public Gradient sky;

    public AnimationCurve curve;

    bool playedEndSound = false;


    // Use this for initialization
    void Start()
    {
        doShit = false;
        timerText = GetComponent<Text>();
        StartTimer();
        //FindObjectOfType<AudioManager>().Stop("MondoMenu");

        AudioManager.instance.Stop("MondoMenu");

        //FindObjectOfType<AudioManager>().Play("MondoTimer");

        AudioManager.instance.Play("MondoTimer");

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
            //UnityEngine.SceneManagement.SceneManager.LoadScene("Targeting");
        }
        progressBar.fillAmount = currentTime / duration;

        sun.rectTransform.localPosition = new Vector3(Mathf.Lerp(-sun.rectTransform.rect.width / 2, Screen.width + sun.rectTransform.rect.width / 2, currentTime / duration) - Screen.width / 2, -400 + curve.Evaluate(currentTime / duration) * 1000);
        sun.rectTransform.Rotate(new Vector3(0, 0, .2f));

        background.color = sky.Evaluate(currentTime / duration);
        

        EndTimerSound();

    }

    void EndTimerSound()
    {
        if (currentTime > duration - MondoEndSoundDuration && !playedEndSound)
        {
            AudioManager.instance.Stop("MondoTimer");
            AudioManager.instance.Play("MondoTimerEnd");
            playedEndSound = true;
        }
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

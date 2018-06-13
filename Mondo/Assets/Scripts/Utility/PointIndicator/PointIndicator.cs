using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointIndicator : MonoBehaviour {

    Text text;
    
    public void SetPoints(int amount, string reason)
    {
        if (text == null)
            text = GetComponentInChildren<Text>();

        if(amount < 0)
        {
            text.color = Color.red;
        }
        else if(amount > 0)
        {
            text.color = Color.green;
        }

        text.text = string.Format("{0}: {1}", reason, amount.ToString());
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}

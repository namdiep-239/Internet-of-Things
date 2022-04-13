using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
    public bool b = true;
    public Image image1;
    public Image image2;
    public float speed = 0.5f;

    float time = 0f;

    public Text progress;

    // Update is called once per frame
    void Update()
    {
        if(b)
        {
            time += Time.deltaTime * speed;
            image1.fillAmount = time;
            image2.fillAmount = time;
            if (progress)
            {
                progress.text = (int)(image1.fillAmount * 100) + "%";
            }  
            if (time > 1)
            {
                time = 0;
            }    
        }    
        
    }
}

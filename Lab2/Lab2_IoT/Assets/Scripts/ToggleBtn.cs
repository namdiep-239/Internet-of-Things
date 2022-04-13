//Script nay dung de handle viec hien thi toggle button moi khi gia tri no thay doi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToggleBtn : MonoBehaviour
{
    public Toggle toggle;
    public GameObject circle;
    //public Image BG;
    public Image img_ON;
    public Image img_OFF;

    //Color Off = new Color(219, 219, 219);
    //Color On = new Color(252, 208, 145) ;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle.isOn)
        {
            circle.transform.DOLocalMoveX(40, 0.2f);
            //BG.color = On;
            img_OFF.gameObject.SetActive(false);
            img_ON.gameObject.SetActive(true);
        }
        else
        {
            circle.transform.DOLocalMoveX(-40, 0.2f);
            //BG.color = Off;
            img_OFF.gameObject.SetActive(true);
            img_ON.gameObject.SetActive(false);
        }
    }
}

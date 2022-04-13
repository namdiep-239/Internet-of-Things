using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public Text _text;
    public InputField _input;
    //public Button _button;
    public GameObject Login;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void Display()
    {
        _text.text = _input.text;
        if (Login.activeInHierarchy)
        {
            Login.SetActive(false);
        }    
        else
        {
            Login.SetActive(true);
        }    
    }    
    */
    public void ChangeScene()
    {
        SceneManager.LoadScene("Scene2");
    }     
    
    /*public void BackScene()
    {
        SceneManager.LoadScene("Scene1");
    } */   
}

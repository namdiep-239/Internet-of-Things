/*Script nay viet dua tren script ChuongGaManager.cs*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Dashboard
{
    public class DashboardManager : MonoBehaviour
    {
        //Layer 1
        [SerializeField]
        private CanvasGroup _canvasLayer1;
        //[SerializeField]
        //private InputField addressinputfield;
        //[SerializeField]
        //private InputField usernameField;
        //[SerializeField]
        //private InputField pwdField;

        //Layer 2
        [SerializeField]
        private CanvasGroup _canvasLayer2;
        [SerializeField]
        private Text temperature;
        [SerializeField]
        private Text humidity;
        [SerializeField]
        private Toggle LedControl;
        [SerializeField]
        private Toggle PumpControl;
        [SerializeField]
        public Image[] Temp_Gause_points;
        [SerializeField]
        public Image[] Humi_Gause_points;

        float MaxValue = 100;


        //public GameObject sceneLogin;
        //public GameObject sceneDB;

        ///////////DOtween Effect///////////
        private Tween twenFade;

        public void Fade(CanvasGroup _canvas, float endValue, float duration, TweenCallback onFinish)
        {
            if (twenFade != null)
            {
                twenFade.Kill(false);
            }

            twenFade = _canvas.DOFade(endValue, duration);
            twenFade.onComplete += onFinish;
        }

        public void FadeIn(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 1f, duration, () =>
            {
                _canvas.interactable = true;
                _canvas.blocksRaycasts = true;
            });
        }

        public void FadeOut(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 0f, duration, () =>
            {
                _canvas.interactable = false;
                _canvas.blocksRaycasts = false;
            });
        }

        IEnumerator _IESwitchLayer()
        {
            if (_canvasLayer1.interactable == true)
            {
                FadeOut(_canvasLayer1, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer2, 0.25f);
            }
            else
            {
                FadeOut(_canvasLayer2, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer1, 0.25f);
            }
        }

        public void SwitchLayer()
        {
            StartCoroutine(_IESwitchLayer());
        }

        //////////////Server to App///////////
        //Update temp and humidity status on App after subscribe
        public void Update_Status(Status_Data _status_data)
        {
            //Update at Text
            temperature.text = _status_data.temperature + "°C";
            humidity.text = _status_data.humidity + "%";
            //Update at Gause
            float temp_value = float.Parse(_status_data.temperature);
            GauseFiller(Temp_Gause_points ,temp_value);
            float humi_value = float.Parse(_status_data.humidity);
            GauseFiller(Humi_Gause_points ,humi_value);
        }

        //Gause handle functions
        void GauseFiller(Image[] Gause_points, float value)
        {
            for (int i = 0; i < Gause_points.Length; i++)
            {
                Gause_points[i].enabled = !DisplayGausePoints(value, i);
            }
        }

        bool DisplayGausePoints(float value, int pointNum)
        {
            return ((pointNum * 5) >= value);
        }

        //Update led and pump button status on App after subscribe
        public void Update_Control(Control_Data _control_data)
        {
            if (_control_data.device == "LED")
            {
                if (_control_data.status == "ON")
                    LedControl.isOn = true;
                else
                    LedControl.isOn = false;
            }
            else if (_control_data.device == "PUMP")
            {
                if (_control_data.status == "ON")
                    PumpControl.isOn = true;
                else
                    PumpControl.isOn = false;
            }    
        }

        ///////////////App to Server////////////
        //update LED json before publish to server
        public Control_Data Update_Led_Value(Control_Data _control)
        {
            _control.device = "LED";
            _control.status = (LedControl.isOn ? "ON" : "OFF");
            return _control;
        }

        //update PUMP json before publish to server
        public Control_Data Update_Pump_Value(Control_Data _control)
        {
            _control.device = "PUMP";
            _control.status = (PumpControl.isOn ? "ON" : "OFF");
            return _control;
        }

        public void Start()
        {
            FadeIn(_canvasLayer1, 0.1f);
            FadeOut(_canvasLayer2, 0.1f);
        }
    }

}

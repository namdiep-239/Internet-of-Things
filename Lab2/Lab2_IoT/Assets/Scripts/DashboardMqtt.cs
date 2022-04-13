/*Script nay viet dua tren script ChuongGaMqtt.cs */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace Dashboard
{
    //format: {"temperature": 30, "humidity": 50}
    public class Status_Data
    {
        public string temperature { get; set; }
        public string humidity { get; set; }
    }

    //format: {"device": "LED", "status": "OFF"}
    //      or {"device":"PUMP","status":"OFF"}
    public class Control_Data
    {
        public string device { get; set; }
        public string status { get; set; }
    }


    public class DashboardMqtt : M2MqttUnityClient
    {
        //Variables for Login
        public InputField addressinputfield;
        //public InputField portInputField;
        public InputField usernameField;
        public InputField pwdField;
        //public string Topic;
        //public string Machine_Id;
        //public string Topic_to_Subcribe = "";
        //public string msg_received_from_topic = "";
        public Text error_display;

        //Topics is stored in a list
        public List<string> topics = new List<string>();

        public string msg_received_from_topic_status = "";
        public string msg_received_from_topic_led = "";
        public string msg_received_from_topic_pump = "";

        private List<string> eventMessages = new List<string>();
        [SerializeField]
        public Status_Data _status_data;
        [SerializeField]
        public Control_Data _control_data;

        //set brokeraddress from inputfield
        public void setbrokeraddress(string brokeraddress)
        {
            if (addressinputfield)
            {
                this.brokerAddress = brokeraddress;
            }
        }

        //public void SetBrokerPort(string brokerPort)
        //{
        //    if (portInputField && !updateUI)
        //    {
        //        int.TryParse(brokerPort, out this.brokerPort);
        //    }
        //}

        //set username from inputfield
        public void setUsername(string username)
        {
            if (usernameField)
            {
                this.mqttUserName = username;
            }
        }

        //set password from inputfield
        public void setPWD(string PWD)
        {
            if (pwdField)
            {
                this.mqttPassword = PWD;
            }
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            GetComponent<DashboardManager>().SwitchLayer();
            SubscribeTopics();
            error_display.text = "";
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            Debug.Log("CONNECTION FAILED! " + errorMessage);
            error_display.text = "Connection Failed!";
        }

        protected override void OnDisconnected()
        {
            Debug.Log("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }

        protected override void Start()
        {

            base.Start();
            addressinputfield.text = "mqttserver.tk";
            usernameField.text = "bkiot";
            pwdField.text = "12345678";

        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            //if (autoTest)
            //{
            //    autoConnect = true;
            //}
        }

        //////////////////Subscribe /////////////////
        protected override void SubscribeTopics()
        {

            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                }
            }
        }

        protected override void UnsubscribeTopics()
        {
            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Unsubscribe(new string[] { topic });
                }
            }

        }
        
        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            //text_display.text = msg;
            Debug.Log("Received: " + msg);
            //StoreMessage(msg);
            if (topic == topics[0])
                ProcessMessageStatus(msg);

            if (topic == topics[1] || topic == topics[2])
                ProcessMessageControl(msg);
        }

        private void ProcessMessageStatus(string msg)
        {
            _status_data = JsonConvert.DeserializeObject<Status_Data>(msg);
            msg_received_from_topic_status = msg;

            GetComponent<DashboardManager>().Update_Status(_status_data);

        }

        private void ProcessMessageControl(string msg)
        {
            _control_data = JsonConvert.DeserializeObject<Control_Data>(msg);
            if (_control_data.device == "LED")
                msg_received_from_topic_led = msg;
            else if (_control_data.device == "PUMP")
                msg_received_from_topic_pump = msg;

            GetComponent<DashboardManager>().Update_Control(_control_data);

        }


        //////////////////Publish////////////////
        public void PublishLed()
        {
            _control_data = GetComponent<DashboardManager>().Update_Led_Value(_control_data);
            string msg_led = JsonConvert.SerializeObject(_control_data);
            
            client.Publish(topics[1], System.Text.Encoding.UTF8.GetBytes(msg_led), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("publish led");

        }

        public void PublishPump()
        {
            _control_data = GetComponent<DashboardManager>().Update_Pump_Value(_control_data);
            string msg_pump = JsonConvert.SerializeObject(_control_data);
            
            client.Publish(topics[2], System.Text.Encoding.UTF8.GetBytes(msg_pump), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("publish pump");

        }

    }
}




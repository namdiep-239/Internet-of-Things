print("IoT Gateway")
#library
import paho.mqtt.client as mqttclient
import time
import json


BROKER_ADDRESS = "mqttserver.tk"
PORT = 1883
MQTT_USER_NAME = "bkiot"
MQTT_PASSWORD = "12345678"

STATUS_TOPIC = "/bkiot/1914213/status"
DEVICE_TOPICS = {"led": "/bkiot/1914213/led",
                 "pump": "/bkiot/1914213/pump"}

# functions
def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")


def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    # temp_data = {'value': True}
    # try:
    #     jsonobj = json.loads(message.payload)
    #     if jsonobj['method'] == "setValue":
    #         temp_data['value'] = jsonobj['params']
    #         client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
    # except:
    #     pass


def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Thingsboard connected successfully!!")
        client.subscribe(STATUS_TOPIC)
        for topic in DEVICE_TOPICS:
            client.subscribe(topic)
    else:
        print("Connection is failed")


client = mqttclient.Client("Gateway_Thingsboard")
client.username_pw_set(MQTT_USER_NAME, MQTT_PASSWORD)

client.on_connect = connected       #khi kết nối thành công thì gọi hàm connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()

client.on_subscribe = subscribed
client.on_message = recv_message

temp = 25
humi = 70

count = 0
while True:
    collect_data = {'temperature': temp, 'humidity': humi}
    temp += 1
    humi += 1
    led_data = {'device': 'LED', 'status': 'OFF'}
    pump_data = {'device': 'PUMP', 'status': 'OFF'}
    # print(getLoc())
    # Chi pub 1 lan roi sub thoi chu ko pub nua
    if count == 0:
        client.publish(STATUS_TOPIC, json.dumps(collect_data), 1, True)
        count = 1
    time.sleep(5)




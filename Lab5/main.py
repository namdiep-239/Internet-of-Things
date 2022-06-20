print("IoT Gateway")
import paho.mqtt.client as mqttclient
import time
import json
import serial.tools.list_ports
from simple_ai import  *
import random

BROKER_ADDRESS = "demo.thingsboard.io"
PORT = 1883
mess = ""

#TODO: Add your token and your comport
#Please check the comport in the device manager
THINGS_BOARD_ACCESS_TOKEN = "q5jEEII0Wrm169d15Fet"

def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")

def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))

def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Thingsboard connected successfully!!")
        client.subscribe("v1/devices/me/rpc/request/+")
    else:
        print("Connection is failed")


client = mqttclient.Client("Gateway_Thingsboard")
client.username_pw_set(THINGS_BOARD_ACCESS_TOKEN)

client.on_connect = connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()

client.on_subscribe = subscribed
client.on_message = recv_message

counter = 0
AI_Results = ["Unmaskable", "Maskable", "Background"]
while True:
    counter = counter + 1
    if counter >= 5:
        counter = 0
        capture_image()
        index, confidence = ai_detection()
        print("from main.py", index, confidence, AI_Results[index])
        #prepare and send to server
        # if index == 0:
        #     data = {"unmask": confidence}
        # elif index == 1:
        #     data = {"mask": confidence}
        # elif index == 2:
        #     data = {"bg": confidence}
        unmask_data = {"unmask": int(confidence[0] * 100)}
        mask_data = {"mask": int(confidence[1] * 100)}
        bg_data = {"bg": int(confidence[2] * 100)}

        client.publish("v1/devices/me/telemetry", json.dumps(unmask_data), 1, True)
        client.publish("v1/devices/me/telemetry", json.dumps(mask_data), 1, True)
        client.publish("v1/devices/me/telemetry", json.dumps(bg_data), 1, True)

    time.sleep(1)
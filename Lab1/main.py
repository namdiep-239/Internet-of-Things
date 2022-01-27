print("IoT Gateway")
#library
import paho.mqtt.client as mqttclient
import time
import json
# locate library
import requests
import winrt.windows.devices.geolocation as wdg, asyncio


BROKER_ADDRESS = "demo.thingsboard.io"
PORT = 1883
THINGS_BOARD_ACCESS_TOKEN = "PYQmBpK0IuMPNR5N8Dsa"

# functions
def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")


def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    temp_data = {'value': True}
    try:
        jsonobj = json.loads(message.payload)
        if jsonobj['method'] == "setValue":
            temp_data['value'] = jsonobj['params']
            client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
    except:
        pass


def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Thingsboard connected successfully!!")
        client.subscribe("v1/devices/me/rpc/request/+")
    else:
        print("Connection is failed")


# location functions
async def getCoords():
    locator = wdg.Geolocator()
    pos = await locator.get_geoposition_async()
    return [pos.coordinate.latitude, pos.coordinate.longitude]

def getLoc():
    return asyncio.run(getCoords())

client = mqttclient.Client("Gateway_Thingsboard")
client.username_pw_set(THINGS_BOARD_ACCESS_TOKEN)

client.on_connect = connected       #khi kết nối thành công thì gọi hàm connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()

client.on_subscribe = subscribed
client.on_message = recv_message

temp = 30
humi = 50

# longitude = 106.6297
# latitude = 10.8231
location = getLoc()
latitude = location[0]
longitude = location[1]

counter = 0
while True:
    collect_data = {'temperature': temp, 'humidity': humi,
                    'longitude': longitude, 'latitude':latitude}
    temp += 1
    humi += 1

    location = getLoc()
    latitude = location[0]
    longitude = location[1]
    # print(getLoc())

    client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)
    time.sleep(10)



import paho.mqtt.client as mqtt
import socket

# Configurações MQTT
MQTT_BROKER = ""  # Endereço do broker MQTT
MQTT_PORT = 1883
MQTT_TOPIC = ""

# Configurações Ethernet
ETHERNET_IP = "192.168.1.100"  # IP do servidor de destino
ETHERNET_PORT = 12345

# Callback quando o cliente se conecta ao broker
def on_connect(client, userdata, flags, rc):
    print("Conectado com o código de resultado " + str(rc))
    client.subscribe(MQTT_TOPIC)

# Callback quando uma mensagem é recebida do broker
def on_message(client, userdata, msg):
    print(f"Mensagem recebida no tópico {msg.topic}: {msg.payload.decode()}")
    send_over_ethernet(msg.payload.decode())

# Função para enviar dados via Ethernet
def send_over_ethernet(data):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((ETHERNET_IP, ETHERNET_PORT))
        s.sendall(data.encode())
        print(f"Dado enviado para {ETHERNET_IP}:{ETHERNET_PORT}")

# Configuração do cliente MQTT
client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message

client.connect(MQTT_BROKER, MQTT_PORT, 60)
client.loop_forever()

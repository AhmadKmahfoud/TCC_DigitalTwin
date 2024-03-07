import requests
import io

def print_image(image_path, printer_ip="192.168.44.1"):
  """Envia uma imagem para a impressora MBrush.
    Dicionário com a resposta da API da impressora.
  """

  # Converte a imagem para um formato compatível com a impressora
  with open(image_path, "rb") as f:
    image_data = f.read()
  response = requests.post(
      f"http://{printer_ip}/cgi-bin/upload",
      files={"file": ("image.png", image_data)},
  )
  if response.status_code != 200:
    raise RuntimeError(f"Erro ao enviar imagem: {response.status_code}")

  # Inicia a impressão
  response = requests.get(f"http://{printer_ip}/cgi-bin/cmd?cmd=simulate&key=1")
  if response.status_code != 200:
    raise RuntimeError(f"Erro ao iniciar impressão: {response.status_code}")

  return response.json()

if __name__ == "__main__":
  # Insira o caminho para a sua imagem PNG
  image_path = "transferir.png"

  # Envia a imagem para a impressora e imprime a resposta da API
  response = print_image(image_path)
  print(response)

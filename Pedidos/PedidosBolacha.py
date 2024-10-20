from http.server import HTTPServer, SimpleHTTPRequestHandler
import os

# Cria o arquivo HTML
# html_content = '''<!DOCTYPE html>
# <html lang="pt-BR">
# <head>
#     <meta charset="UTF-8">
#     <meta name="viewport" content="width=device-width, initial-scale=1.0">
#     <title>Pedido de Bolachas</title>
#     <script src="https://cdnjs.cloudflare.com/ajax/libs/axios/1.6.2/axios.min.js"></script>
#     <style>
#         body {
#             font-family: 'Arial Rounded MT Bold', Arial, sans-serif;
#             display: flex;
#             flex-direction: column;
#             align-items: center;
#             min-height: 100vh;
#             margin: 0;
#             padding: 20px;
#             background-color: #FFF8E7;
#             position: relative;
#             overflow-x: hidden;
#         }

#         .background-decoration {
#             position: fixed;
#             font-size: 100px;
#             color: #F5DEB3;
#             z-index: -1;
#             opacity: 0.3;
#             transform: rotate(-45deg);
#         }

#         .content {
#             display: flex;
#             flex-direction: column;
#             align-items: center;
#             width: 100%;
#             max-width: 600px;
#             margin-top: 40px;
#         }

#         h1.main-title {
#             font-size: 48px;
#             color: #8B4513;
#             margin-bottom: 10px;
#             text-align: center;
#         }

#         h2.subtitle {
#             font-size: 28px;
#             color: #A0522D;
#             margin-bottom: 40px;
#             text-align: center;
#             font-weight: bold;
#         }

#         .input-container {
#             margin-bottom: 30px;
#             width: 100%;
#             max-width: 300px;
#             padding-right: 40px;
#         }

#         .input-label {
#         font-size: 24px;
#         color: #8B4513;
#         margin-bottom: 10px;
#         text-align: center;
#         }

#         input {
#             width: 100%;
#             padding: 15px;
#             font-size: 18px;
#             border: 3px solid #DEB887;
#             border-radius: 25px;
#             background-color: #FFFFFF;
#             box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
#         }

#         .button-container {
#             display: flex;
#             gap: 30px;
#             margin-bottom: 20px;
#         }

#         .cookie-button {
#             width: 150px;
#             height: 150px;
#             border-radius: 50%;
#             cursor: pointer;
#             border: none;
#             font-size: 20px;
#             font-weight: bold;
#             transition: all 0.3s;
#             position: relative;
#             box-shadow: 0 6px 10px rgba(0, 0, 0, 0.15);
#             display: flex;
#             flex-direction: column;
#             align-items: center;
#             justify-content: center;
#         }

#         .cookie-button::before {
#             content: '';
#             position: absolute;
#             top: 5px;
#             left: 5px;
#             right: 5px;
#             bottom: 5px;
#             border-radius: 50%;
#             border: 2px dotted rgba(255, 255, 255, 0.5);
#         }

#         .cookie-button::after {
#             content: '🍪';
#             font-size: 40px;
#             margin-bottom: 5px;
#         }

#         #chocolate {
#             background: radial-gradient(circle at 30% 30%, #8B4513, #654321);
#             color: white;
#         }

#         #baunilha {
#             background: radial-gradient(circle at 30% 30%, #F3E5AB, #DAA520);
#             color: #5C4033;
#         }

#         .cookie-button:hover:not(.selected) {
#             transform: scale(1.05) rotate(5deg);
#         }

#         .cookie-button.selected {
#             border: 3px solid #28ff00;
#         }

#         #status {
#             margin-top: 20px;
#             font-size: 20px;
#             text-align: center;
#             min-height: 30px;
#             padding: 10px;
#             border-radius: 15px;
#         }

#         .success-message {
#             color: #2E8B57;
#             background-color: #98FB98;
#             padding: 10px 20px;
#             border-radius: 15px;
#         }

#         .error-message {
#             color: #B22222;
#             background-color: #FFB6C1;
#             padding: 10px 20px;
#             border-radius: 15px;
#         }

#         #enviarButton {
#             background-color: #8B4513;
#             color: white;
#             padding: 15px 30px;
#             border: none;
#             border-radius: 25px;
#             cursor: pointer;
#             font-size: 18px;
#             transition: all 0.3s;
#         }

#         #enviarButton:hover {
#             background-color: #A0522D;
#         }

#         /* Adicione estas classes ao seu bloco de estilos existente */

#         .input-label {
#             font-size: 24px;
#             color: #A0522D;
#             margin-bottom: 10px;
#         }

#         .cookie-choice-label {
#             font-size: 24px;
#             color: #A0522D;
#             margin-bottom: 20px;
#         }

#     </style>
# </head>
# <body>
#     <div class="background-decoration" style="top: 10%; left: -5%;">🍪</div>
#     <div class="background-decoration" style="top: 40%; right: -5%;">🍪</div>
#     <div class="background-decoration" style="bottom: 10%; left: 10%;">🍪</div>

#     <div class="content">
#         <h1 class="main-title">Bem-vindo!</h1>
        
#         <p class="input-label">Por favor, digite seu nome:</p>
#         <div class="input-container">
#             <input type="text" id="nome" placeholder="Digite seu nome" required>
#         </div>
        
#         <p class="input-label">Qual sabor de bolacha você deseja?</p>
#         <div class="button-container">
#             <button id="chocolate" class="cookie-button">Chocolate</button>
#             <button id="baunilha" class="cookie-button">Baunilha</button>
#         </div>
        
#         <button id="enviarButton">Enviar Pedido</button>
        
#         <div id="status"></div>
#     </div>

#     <script>
#         const API_URL = 'http://10.2.2.91:80/api/registrar-bolacha';
#         const statusElement = document.getElementById('status');
#         const nomeInput = document.getElementById('nome');
#         const chocolateButton = document.getElementById('chocolate');
#         const baunilhaButton = document.getElementById('baunilha');
#         const enviarButton = document.getElementById('enviarButton');

#         let selectedCookie = null;

#         function updateButtons() {
#             const isNomePreenchido = nomeInput.value.trim() !== '';
#             chocolateButton.disabled = !isNomePreenchido;
#             baunilhaButton.disabled = !isNomePreenchido;
#             enviarButton.disabled = !isNomePreenchido || selectedCookie === null;
#         }

#         nomeInput.addEventListener('input', updateButtons);

#         function selectCookie(tipo) {
#             selectedCookie = tipo;
#             chocolateButton.classList.toggle('selected', tipo === 'chocolate');
#             baunilhaButton.classList.toggle('selected', tipo === 'baunilha');
#             updateButtons();
#         }

#         chocolateButton.addEventListener('click', () => selectCookie('chocolate'));
#         baunilhaButton.addEventListener('click', () => selectCookie('baunilha'));

#         async function registrarBolacha() {
#             if (!nomeInput.value.trim()) {
#                 statusElement.textContent = 'Por favor, preencha seu nome.';
#                 statusElement.className = 'error-message';
#                 return;
#             }

#             if (selectedCookie === null) {
#                 statusElement.textContent = 'Por favor, escolha um tipo de bolacha.';
#                 statusElement.className = 'error-message';
#                 return;
#             }

#             try {
#                 const response = await axios.post(API_URL, {
#                     bTipoBolacha: selectedCookie === 'baunilha',
#                     nome: nomeInput.value.trim()
#                 });
                
#                 statusElement.textContent = 'Pedido enviado com sucesso! Sua bolacha está a caminho! 🍪';
#                 statusElement.className = 'success-message';
#                 nomeInput.value = '';
#                 selectedCookie = null;
#                 updateButtons();
                
#                 setTimeout(() => {
#                     statusElement.textContent = '';
#                     statusElement.className = '';
#                 }, 3000);
#             } catch (error) {
#                 statusElement.textContent = `Ops! Houve um problema com seu pedido: ${error.message}`;
#                 statusElement.className = 'error-message';
#             }
#         }

#         enviarButton.addEventListener('click', registrarBolacha);
#     </script>
# </body>
# </html>
# '''

# # Salva o conteúdo HTML em um arquivo
# with open('index.html', 'w', encoding='utf-8') as f:
#     f.write(html_content)

# Configura e inicia o servidor
PORT = 8000
Handler = SimpleHTTPRequestHandler
httpd = HTTPServer(("", PORT), Handler)
print(f"Servidor rodando em http://localhost:{PORT}")
print(f"Para acessar de outros dispositivos, use http://[IP_DO_SEU_PC]:{PORT}")
httpd.serve_forever()
from flask import Flask, request, jsonify
from flask_cors import CORS
import mysql.connector

app = Flask(__name__)
CORS(app)

# Configuração do banco de dados MySQL
DB_CONFIG = {
    'host': '10.2.2.91',
    'user': 'root',     # Substitua pelo seu usuário do MySQL
    'password': '!nspi00',   # Substitua pela sua senha do MySQL
    'database': 'mqtt_unity'
}

def get_db_connection():
    return mysql.connector.connect(**DB_CONFIG)

@app.route('/api/registrar-bolacha', methods=['POST'])
def registrar_bolacha():
    try:
        data = request.json
        nome = data.get('nome')
        bTipoBolacha = data.get('bTipoBolacha')  # True para baunilha, False para chocolate
        
        if nome is None or bTipoBolacha is None:
            return jsonify({'error': 'Dados incompletos'}), 400
        
        conn = get_db_connection()
        cursor = conn.cursor()
        
        # Mantém a lógica original: True para Baunilha, False para Chocolate
        etapa = 1 if bTipoBolacha else 2
        
        cursor.execute('INSERT INTO rv4frl (Etapa, bTipoBolacha) VALUES (%s, %s)', 
                       (etapa, int(bTipoBolacha)))
        conn.commit()
        
        inserted_id = cursor.lastrowid
        
        cursor.close()
        conn.close()
        
        return jsonify({
            'message': 'Pedido registrado com sucesso',
            'tipo': 'baunilha' if bTipoBolacha else 'chocolate',
            'nome': nome,
            'id': inserted_id
        })
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/pedidos', methods=['GET'])
def ver_pedidos():
    try:
        conn = get_db_connection()
        cursor = conn.cursor()
        cursor.execute('SELECT ID, Etapa, NextMov, bTipoBolacha FROM rv4frl ORDER BY ID DESC')
        pedidos = cursor.fetchall()
        cursor.close()
        conn.close()
        
        return jsonify([{
            'id': p[0],
            'etapa': p[1],
            'nextMov': p[2],
            'bTipoBolacha': p[3]
        } for p in pedidos])
    except Exception as e:
        return jsonify({'error': str(e)}), 500

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=80)
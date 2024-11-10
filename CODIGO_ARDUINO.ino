const int enable1 = 32;
const int dir1 = 34;
const int pulse1 = 36;
const int enable2 = 46;
const int dir2 = 40;
const int pulse2 = 38;
const int numPulses = 2665;  // Número de pulsos necessários 

const int outclp1 = 30;
const int outclp2 = 44;

const int feedback1 = 28;
const int feedback2 = 42;

int estadoAtualOutclp1 = LOW;
int estadoAnteriorOutclp1 = LOW;
int estadoAtualOutclp2 = LOW;
int estadoAnteriorOutclp2 = LOW;

unsigned long tempoUltimoAcionamento1 = 0;  // Tempo do último acionamento do motor 1
unsigned long tempoUltimoAcionamento2 = 0;  // Tempo do último acionamento do motor 2
const unsigned long intervalo = 20000;      // 20 segundos em milissegundos
unsigned long tempoInicioHigh1 = 0;         // Tempo em que o sensor 1 ficou HIGH
unsigned long tempoInicioHigh2 = 0;         // Tempo em que o sensor 2 ficou HIGH
const unsigned long tempoNecessarioHigh = 2000; // 2 segundos em milissegundos

void giramotor1();
void giramotor2();

void setup() {
  pinMode(enable1, OUTPUT);
  pinMode(dir1, OUTPUT);
  pinMode(pulse1, OUTPUT);
 
  pinMode(enable2, OUTPUT);
  pinMode(dir2, OUTPUT);
  pinMode(pulse2, OUTPUT);

  pinMode(outclp1, INPUT);
  pinMode(outclp2, INPUT);

  Serial.begin(9600);
}

void loop() {
  estadoAtualOutclp1 = digitalRead(outclp1);  // Leitura do estado atual do sensor 1
  estadoAtualOutclp2 = digitalRead(outclp2);  // Leitura do estado atual do sensor 2
  unsigned long tempoAtual = millis();        // Tempo atual em milissegundos

  //Serial.println(tempoInicioHigh1);
  Serial.println(tempoUltimoAcionamento1);
  //Serial.println(estadoAtualOutclp1);
  //Serial.println(tempoUltimoAcionamento1);


  // Motor 1: Verifica se o sensor 1 está HIGH por 2 segundos e se já passou o intervalo de 20 segundos
  if (estadoAtualOutclp1 == HIGH) {
    if (tempoInicioHigh1 == 0) {  // Se é a primeira vez que detecta HIGH, salva o tempo
      tempoInicioHigh1 = tempoAtual;
    } else if ((tempoAtual - tempoInicioHigh1 >= tempoNecessarioHigh) && (tempoAtual - tempoUltimoAcionamento1 >= intervalo)) {
      giramotor1();  // Ativa o motor 1 se o sensor ficou HIGH por 2 segundos e o tempo passou
      digitalWrite(feedback1,HIGH);
      delay(2000);
      digitalWrite(feedback1,LOW);
      tempoUltimoAcionamento1 = tempoAtual;  // Atualiza o tempo do último acionamento do motor 1
      tempoInicioHigh1 = 0;  // Reseta o tempo de HIGH para uma nova contagem no futuro
    }
  } else {
    tempoInicioHigh1 = 0;  // Reseta o tempo se o estado voltar para LOW
  }
 
  // Motor 2: Verifica se o sensor 2 está HIGH por 2 segundos e se já passou o intervalo de 20 segundos
  if (estadoAtualOutclp2 == HIGH) {
    if (tempoInicioHigh2 == 0) {  // Se é a primeira vez que detecta HIGH, salva o tempo
      tempoInicioHigh2 = tempoAtual;
    } else if (tempoAtual - tempoInicioHigh2 >= tempoNecessarioHigh && (tempoAtual - tempoUltimoAcionamento2 >= intervalo)) {
      giramotor2();  // Ativa o motor 2 se o sensor ficou HIGH por 2 segundos e o tempo passou
      digitalWrite(feedback2,HIGH);
      delay(2000);
      digitalWrite(feedback2,LOW);
      tempoUltimoAcionamento2 = tempoAtual;  // Atualiza o tempo do último acionamento do motor 2
      tempoInicioHigh2 = 0;  // Reseta o tempo de HIGH para uma nova contagem no futuro
    }
  } else {
    tempoInicioHigh2 = 0;  // Reseta o tempo se o estado voltar para LOW
  }
}

void giramotor1() {
  digitalWrite(enable1, LOW); // Habilita o motor 1
  digitalWrite(dir1, LOW);    // Define a direção (ajuste se necessário)
  for (int i = 0; i < numPulses; i++) {
    digitalWrite(pulse1, HIGH); // Pulso
    delayMicroseconds(1000);     // Ajuste se necessário
    digitalWrite(pulse1, LOW);
    delayMicroseconds(1000);     // Ajuste se necessário
  }
}

void giramotor2() {
  digitalWrite(enable2, LOW); // Habilita o motor 2
  digitalWrite(dir2, LOW);    // Define a direção (ajuste se necessário)
  for (int i = 0; i < numPulses; i++) {
    digitalWrite(pulse2, HIGH); // Pulso
    delayMicroseconds(1000);     // Ajuste se necessário
    digitalWrite(pulse2, LOW);
    delayMicroseconds(1000);     // Ajuste se necessário
  }
}
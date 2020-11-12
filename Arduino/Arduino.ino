#pragma region Includes
#include "Arduino.h"          // Biblioteca principal Arduino
#include <WiFi.h>             // Permite a conexão Wifi
#include "soc/soc.h"          // desabilitar problemas de queda de energia
#include "soc/rtc_cntl_reg.h" // desabilitar problemas de queda de energia
#include "esp_camera.h"       // Inicializa o driver e configurações da câmera
#pragma endregion Includes

#pragma region Config_Wifi
const char* ssid     = "Família_Silva";      // Nome da rede
const char* password = "N40T3mS3nh4"; // Senha da rede
// const char* ssid     = "2G_costa";      // Nome da rede
// const char* password = "32226486wslha"; // Senha da rede
// const char* ssid     = "abner_conect";
// const char* password = "362204aps";
#pragma endregion Config_Wifi

#pragma region Config_Server
String enderecoServidor         = "192.168.0.105";                      // Endereço do servidor
String caminhoServidor          = "/api/imagem/reconhecimentoFacial";  // Diretório para enviar a imagem
String part_boundary            = "RandomBoundaryPartsImagesTCCBrave"; // Tamanho limite das partes da imagem
String key                      = "7ea071e9a6f4462eb0fc31ba2ca3ed5c";  // Chave para acessar
const int portaServidor         = 80;                                  // Porta para acessar
WiFiClient cliente;                                                    // Cliente que acessa o servidor
const int tempoIntervalo        = 30000;                               // Tempo entre cada imagem HTTP POST
unsigned long horaUltimaEnviada = 0;                                   // Última vez que a imagem foi enviada
bool emProcessamento            = false;                               // Se pode fazer um requisição
#pragma endregion Config_Server

#pragma region Config_Camera
#define CAMERA_MODEL_AI_THINKER // Modelo da placa (ESP32-CAM)
#if defined(CAMERA_MODEL_AI_THINKER) // Pinagem da câmera
  
  #define PWDN_GPIO_NUM     32
  #define RESET_GPIO_NUM    -1
  #define XCLK_GPIO_NUM      0
  #define SIOD_GPIO_NUM     26
  #define SIOC_GPIO_NUM     27

  #define Y9_GPIO_NUM       35
  #define Y8_GPIO_NUM       34
  #define Y7_GPIO_NUM       39
  #define Y6_GPIO_NUM       36
  #define Y5_GPIO_NUM       21
  #define Y4_GPIO_NUM       19
  #define Y3_GPIO_NUM       18
  #define Y2_GPIO_NUM        5
  #define VSYNC_GPIO_NUM    25
  #define HREF_GPIO_NUM     23
  #define PCLK_GPIO_NUM     22
#else
  #error "Nenhum modelo de câmera selecionado!"
#endif
camera_config_t config;  // Recebe configurações de inicialização da câmera
camera_fb_t * fb       = NULL; // Usado para tirar foto da câmera, Frame Buffer
esp_err_t iniciaCamera = NULL; // Usado para definir os status da câmera nas operações e enviar os blocos HTTP
#pragma endregion Config_Camera

#pragma region Portas
#define BOTAO 13
#define LED_BRANCO 4
#define LED_VERMELHO 12
#define LED_VERDE 16
#pragma endregion Portas

void iniciaSerial(){
  Serial.begin(115200);         // Monitor Serial
  Serial.setDebugOutput(false); // Desliga a saída de depuração
}

void iniciaWifi(){

  WiFi.mode(WIFI_STA);
  Serial.print("Conectando em ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);             // Inicia conexão com Wifi
  while (WiFi.status() != WL_CONNECTED) { // Enquando não conecta
    delay(1000);
    Serial.print(".");
    if(WiFi.status() == WL_NO_SSID_AVAIL){ // Se a rede não for encontrada
      Serial.println("Rede Wifi não encontrada!");
      return;
    }else if(WiFi.status() == WL_CONNECT_FAILED){ // Tentativas de conexão falharam
      Serial.println("Não foi possível se conectar ao Wifi!");
      return;
    }
  }
  Serial.print("IP da câmera: ");
  Serial.println(WiFi.localIP());
  Serial.println("WiFi conectado com sucesso!");
}

void iniciarCamera(){
  iniciaCamera = esp_camera_init(&config); // Inicia a câmera
  if (iniciaCamera != ESP_OK) { // Se a inicialização não retornar OK
    Serial.printf("Falha ao iniciar câmera: 0x%x", iniciaCamera);
    delay(1000);
    ESP.restart();
  }else{
    Serial.println("Câmera iniciada com sucesso!");
  }
}

void setup() {
  WRITE_PERI_REG(RTC_CNTL_BROWN_OUT_REG, 0); // desabilita detector de queda de energia
  pinMode(LED_BRANCO, OUTPUT);
  pinMode(LED_VERDE, OUTPUT);
  pinMode(LED_VERMELHO, OUTPUT);
  pinMode(BOTAO, INPUT);

  iniciaSerial();

  iniciaWifi();

  #pragma region Config_Camera
  config.ledc_channel = LEDC_CHANNEL_0;
  config.ledc_timer   = LEDC_TIMER_0;
  config.pin_d0       = Y2_GPIO_NUM;
  config.pin_d1       = Y3_GPIO_NUM;
  config.pin_d2       = Y4_GPIO_NUM;
  config.pin_d3       = Y5_GPIO_NUM;
  config.pin_d4       = Y6_GPIO_NUM;
  config.pin_d5       = Y7_GPIO_NUM;
  config.pin_d6       = Y8_GPIO_NUM;
  config.pin_d7       = Y9_GPIO_NUM;
  config.pin_xclk     = XCLK_GPIO_NUM;
  config.pin_pclk     = PCLK_GPIO_NUM;
  config.pin_vsync    = VSYNC_GPIO_NUM;
  config.pin_href     = HREF_GPIO_NUM;
  config.pin_sscb_sda = SIOD_GPIO_NUM;
  config.pin_sscb_scl = SIOC_GPIO_NUM;
  config.pin_pwdn     = PWDN_GPIO_NUM;
  config.pin_reset    = RESET_GPIO_NUM;
  config.xclk_freq_hz = 20000000;
  config.pixel_format = PIXFORMAT_JPEG; // Formato da imagem

  // Inicia com especificações altas para pré-alocar buffers maiores
  if(psramFound()){ // Se estiver usando o PSRAM (8MB de memória)
    config.frame_size   = FRAMESIZE_SVGA; // Resolução
    config.jpeg_quality = 10;             // 0-63 Quanto menor mais qualidade
    config.fb_count     = 2;              // Número de buffers de quadro a serem alocados
  } else {
    config.frame_size   = FRAMESIZE_CIF; // Resolução
    config.jpeg_quality = 12;            // 0-63 Quanto menor mais qualidade
    config.fb_count     = 1;             // Número de buffers de quadro a serem alocados
  }
  #pragma endregion Config_Camera

  iniciarCamera();

}

void loop() {
  if(WiFi.status() == WL_CONNECTION_LOST){ // Se a conexão for perdida
    Serial.println("A conexão com a rede Wifi foi perdida!");
  }
  if(!digitalRead(BOTAO) && !emProcessamento){ // Se for pressionado
    emProcessamento = true;
    capturaEEnvia();
  }
}

String capturaEEnvia() {
  String pegaTudo;
  String pegaCorpo;
  digitalWrite(LED_BRANCO,HIGH); // Acende a luz de processamento

  fb = esp_camera_fb_get(); // Tira foto
  if(!fb) { // Caso ocorra erro na foto
    Serial.println("Falha ao capturar imagem!");
    digitalWrite(LED_BRANCO,LOW); // Apaga a luz de processamento
    for(int i = 0; i< 5; i++){
      digitalWrite(LED_VERMELHO,HIGH);
      delay(500);
      digitalWrite(LED_VERMELHO, LOW);
    }
    delay(1000);
    ESP.restart();
  }

  Serial.println("Conectando ao servidor: " + enderecoServidor);
  if (cliente.connect(enderecoServidor.c_str(), portaServidor)) {
    Serial.println("Conectado com sucesso!");

    String cabecalhoHTTP = "--"+part_boundary+"\r\nContent-Disposition: form-data; name=\"arquivo\"; filename=\"esp32-cam_picture.jpg\"\r\nContent-Type: image/jpeg\r\n\r\n";
    String raboHTTP      = "\r\n--"+part_boundary+"--\r\n";

    uint16_t tamanhoImagem = fb->len;
    uint16_t tamanhoExtra  = cabecalhoHTTP.length() + raboHTTP.length();
    uint16_t tamanhoTotal  = tamanhoImagem + tamanhoExtra;

    #pragma region Cabecalho
    cliente.println("POST " + caminhoServidor + " HTTP/1.1");
    cliente.println("Host: " + enderecoServidor);
    cliente.println("Content-Length: " + String(tamanhoTotal));
    cliente.println("Ocp-Apim-Subscription-Key: "+key);
    cliente.println("Content-Type: multipart/form-data; boundary="+part_boundary);
    cliente.println();
    cliente.print(cabecalhoHTTP);
    #pragma endregion Cabecalho

    #pragma region Imagem
    uint8_t *fbBuffer = fb->buf;
    size_t fbTamanho  = fb->len;

    for (size_t n=0; n<fbTamanho; n=n+1024) {
      if (n+1024 < fbTamanho) {
        cliente.write(fbBuffer, 1024);
        fbBuffer += 1024;
      }
      else if (fbTamanho%1024>0) {
        size_t remainder = fbTamanho%1024;
        cliente.write(fbBuffer, remainder);
      }
    }
    cliente.print(raboHTTP);
    #pragma endregion Imagem

    esp_camera_fb_return(fb); // liberar a memória alocada por esp_camera_fb_get()

    int tempoLimite   = 10000;
    long tempoInicial = millis();
    boolean estado    = false;

    Serial.print("Aguardando resposta do servidor");
    while ((tempoInicial + tempoLimite) > millis()) {
      Serial.print(".");
      delay(100);
      while (cliente.available()) {
        char c = cliente.read();

        if (c == '\n') {
          if (pegaTudo.length()==0) { estado=true; }
          pegaTudo = "";
        } else if (c != '\r') { pegaTudo += String(c); }

        if (estado==true) { pegaCorpo += String(c); }

        tempoInicial = millis();
      }
      if (pegaCorpo.length()>0) { break; }
    }

    pegaCorpo = pegaCorpo.substring(5,pegaCorpo.length() -7);
    
    Serial.println("Resposta:");
    Serial.println(pegaCorpo);
    digitalWrite(LED_BRANCO,LOW); // Apaga a luz de processamento
    
    if(pegaCorpo.equals("Liberado")){ // Liberado
      for(int i=0; i< 3;i++){
        Serial.print("Liberado");
        digitalWrite(LED_VERDE,HIGH); // Acende a luz de sucesso
        delay(500);
        digitalWrite(LED_VERDE,LOW); // Apaga a luz de sucesso
        delay(500);
      }
    }else if(pegaCorpo.equals("Nenhum rosto identificado na imagem.")){ // Nenhum rosto encontrado
      for(int i=0; i< 2;i++){
        Serial.print("Nenhum rosto identificado na imagem.");
        digitalWrite(LED_VERMELHO,HIGH); // Acende a luz de Erro
        delay(500);
        digitalWrite(LED_VERMELHO,LOW); // Apaga a luz de Erro
        delay(500);
      }
    }else if(pegaCorpo.equals("Nenhuma correspondência para esse rosto foi encontrada.")){ // Nenhum cadastro
      for(int i=0; i< 3;i++){
        Serial.print("Nenhuma correspondência para esse rosto foi encontrada.");
        digitalWrite(LED_VERMELHO,HIGH); // Acende a luz de Erro
        delay(500);
        digitalWrite(LED_VERMELHO,LOW); // Apaga a luz de Erro
        delay(500);
      }
    }else if(pegaCorpo.equals("Saldo insuficiente!")){ // Saldo Insuficiente
      for(int i=0; i< 4;i++){
        Serial.print("Saldo Insuficiente");
        digitalWrite(LED_VERMELHO,HIGH); // Acende a luz de Erro
        delay(500);
        digitalWrite(LED_VERMELHO,LOW); // Apaga a luz de Erro
        delay(500);
      }
    }else{
        digitalWrite(LED_BRANCO,LOW); // Apaga a luz de processamento
        for(int i = 0; i< 1; i++){
          Serial.print("Erro no servidor");
          digitalWrite(LED_VERMELHO,HIGH);
          delay(500);
          digitalWrite(LED_VERMELHO, LOW);
          delay(500);
        }
        emProcessamento = false;  
    }
    cliente.stop();
  }
  else {
    digitalWrite(LED_BRANCO,LOW); // Apaga a luz de processamento
    for(int i = 0; i< 1; i++){
      Serial.print("Erro no servidor");
      digitalWrite(LED_VERMELHO,HIGH);
      delay(500);
      digitalWrite(LED_VERMELHO, LOW);
      delay(500);
    }
    emProcessamento = false;
    pegaCorpo = "Conexão com " + enderecoServidor +  " falhou.";
    Serial.println(pegaCorpo);
  }
  emProcessamento = false;
  return pegaCorpo;
}

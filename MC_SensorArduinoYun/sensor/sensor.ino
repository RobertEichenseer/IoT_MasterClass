
#include <Process.h>
//Process to start Node.js script 
Process nodejs;   
char telemetry[] = "{'DeviceId':'Device-100','Temperature':88,'Humidity':68,'Pollution': 76}"; 
int firstStart = 0;

void setup() {
  Bridge.begin(); // Initialize the Bridge
  Serial.begin(9600);   // Initialize the Serial
 
  // For debug reason wait until a serial monitor is connected
  while (!Serial);
   
  // launch the Node.js script which ingests telemetry to EventHub
  nodejs.runShellCommandAsynchronously("node /mnt/sda1/arduino/www/EventHub/eventhub.js");
  // show process has started
  Serial.println("Process Started");
}
 
void loop() {

  //read from Arduino sensors
  //telemetry = "{'DeviceId':'Device-100','Temperature':88,'Humidity':68,'Pollution': 76}";
  Serial.println("New Ingest");
  if (firstStart == 0)
  {
    Serial.println("First Ingest - Wait for Node to completly start"); 
    delay(10000); 
    firstStart = 1; 
  }
  
  while (nodejs.available()) {
      Serial.write(nodejs.read());
  }

  
  if(nodejs.running())
  {
    Serial.println("Start Ingest");
    for (int i=0; i<=sizeof(telemetry); i++)
      nodejs.write(telemetry[i]);
    nodejs.write('\r'); 
    nodejs.write('\n');
    nodejs.flush(); 
  }

  // pass any incoming bytes from the running node process
  // to the serial port:
   
  while (nodejs.available()) {
    Serial.write(nodejs.read());
  }
  
  Serial.println("Wait 5 seconds"); 
  delay(5000); 
}



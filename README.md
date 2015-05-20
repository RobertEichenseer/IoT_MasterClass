IoT Master Class
================
The solution can be used as a copy/paste source to build own IoT solutions using Azure IoT services. 

#### Description of the specific solutions ####

##### [MC_PowerShellScripts](MC_PowerShellScripts/) #####
Creates an Anzure Service Bus Namespace using power shell scripting.
 
##### [MC_AdminConsole](MC_AdminConsole/) #####
Createas an Azure Service Bus EventHub using C# and a console app.
 
##### [MC_SensorDotNet](MC_SensorDotNet/) #####
Ingests telemetry data into an EventHub using C#, the Azure Service Bus SDK (Win32)
 
##### [MC_SensorNetduiono](MC_SensorNetduino/) #####
Ingests telemetry data into an EventHub using C#, Visuals Studio 2013 and the .net Micro Framework running on an Netduiono 2 plus

##### [MC_SensorWinRuntime](MC_SensorWinRuntime/) #####
Ingests telemetry data into an EventHub using C#, Visuals Studio and the Windows Runtime libraries (MS-Band, Store App, Raspberry 2)

##### [MC_SensorArduinoYun](MC_SensorArduinoYun/) #####
Ingests telemetry data into an EventHub using Arduino Yun sketches and Node.js

##### [MC_FieldGateway](MC_FieldGateway/) #####
Uses Node.js to run an Rest API on e. g. a Raspberry Pi, Arduino Yun acting as a field gateway. Modifies and forwards the telemetry data to an EventHub

##### [MC_EventConsumer](MC_EventConsumer/) #####
Consumes ingested telemetry data from an EventHub using C# and the Azure Service Bus SDK (Win32).
It uses raw access to specific partitions as well as IEventProcessor

##### [MC_EventConsumerAnalyse](MC_EventConsumerAnalyse/) #####
See MC_EventConsumer. Adds basic analytics using C#

##### [MC_StreamAnalytics](MC_StreamAnalytics/) #####
Similar analytics functionality like MC_EventConsumerAnalyse; implemented using Azure Stream Analytics

##### [MC_CommandControlFrontEnd](MC_CommandControlFrontEnd/) #####
Raw REST API using ASP.NET; C# to simualte a Command & Control backend



 
 


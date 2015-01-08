var eventhub = require("./eventhub");

function forward(clientData, response, responder){

	//modify, enrich telemetry data
	//convert from Fahrenheit to Celsius
	var jsonClientData = JSON.parse(clientData);
	
	if (jsonClientData.DeviceId == "Device-100")
	{
		console.log("Device-100");
		jsonClientData.Temperature = (jsonClientData.Temperature - 32) * 5/9 ; 
		clientData = JSON.stringify(jsonClientData);
	}
	eventhub.sendTelemetry(clientData, response, responder); 
}

exports.forward = forward; 
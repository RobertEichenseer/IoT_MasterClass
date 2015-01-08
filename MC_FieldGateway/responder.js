function respond(response, responseData){
	
	if (responseData.Action == "ProvideTelemetryFormat"){
		var telemetryFormat = {
			"DeviceId": "Device-100",
			"Temperature": 37,
			"Humidity": 68,
			"Polution": 70
		}
		response.writeHead(responseData.Code, {
			"Content-Type": responseData.ContentType, 
			"Content-Length": JSON.stringify(telemetryFormat).length 
		});
		
		response.write(JSON.stringify(telemetryFormat));
	}
	else if (responseData.Action == "SuccessfulTelemetrieIngest"){
		response.writeHead(responseData.Code, {
			"Content-Type": responseData.ContentType, 
			"Content-Length": responseData.Action.length 
		});
		
		response.write(responseData.Action);
	}
	else {
		response.writeHead(responseData.Code, {
			"Content-Type": responseData.ContentType, 
			"Content-Length": responseData.Message.length 
		});
		
		response.write(responseData.Message);	
	}
	
	response.end(); 
}

exports.respond = respond;
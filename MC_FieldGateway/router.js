var url = require("url");
var forwarder = require("./forwarder");
var responder = require("./responder"); 

function route(request, response){
	
	var pathname = url.parse(request.url).pathname.toLowerCase();

	if (pathname == "/sendtelemetry" && request.method == "POST")
	{
		getData(request, response, forwarder.forward);
	}
	else if (request.method == "GET")
	{
		var responseData = {
			"Action": "ProvideTelemetryFormat",
			"Code": 200,
			"ContentType": "applicaton/json",
		};
		responder.respond(response, responseData);
	}
	else
	{
		var responseData = {
			"Action": "Wrong Request",
			"Code": 404,
			"ContentType": "text/plain",
			"Message": "Wrong request; Use GET or POST to /sendtelemetry"
		}
		responder.respond(response, responseData);
	}
}

function getData(request, response, forwarder){
	if (request.method == "POST"){
	    var clientData = '';
	    request.on('data', function (data) {
	        clientData += data;
	        // Too much POST data, kill the connection!
	        if (clientData.length > 1e6)
	            request.connection.destroy();
	    });
	    request.on('end', function () {
	        //forwarder(JSON.parse(clientData), response, responder.respond);
	        forwarder(clientData, response, responder);
	    });
	}; 
}

exports.route = route; 
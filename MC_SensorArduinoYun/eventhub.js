var https = require('https');
var crypto = require('crypto');
var moment = require('moment');

//readline; stdin - stdout 
var readline = require('readline');

// Event Hubs parameters
var nameSpace = 'IoTMC-ns';
var hubName ='IoTMC';
var deviceName = 'Device01';

// Full Event Hub publisher URI
var eventHubUri = 'https://' + nameSpace + '.servicebus.windows.net' + '/' + hubName + '/publishers/' + deviceName + '/messages';

// Start listening for messages from Arduino
console.log("Hello Arduino");     
    
var lineReader = readline.createInterface({
  input: process.stdin,
  output: process.stdout,
  terminal: false
});
 
lineReader.on('line', function (data) {
	sendTelemetry(data)
});

 
// Create a SAS token
// See http://msdn.microsoft.com/library/azure/dn170477.aspx
function createSASToken()
{

	var token = 'SharedAccessSignature sr=https%3A%2F%2FIoTMC-ns.servicebus.windows.net%2Fiotmc%2Fpublishers%2FDevice01%2Fmessages&sig=%2FtAE5T5pCwPj%2B8ZTNTCS40dnBSeDbVkdksTFCU40%2FPE%3D&se=1424044790&skn=Device01';

    return token;
}

function sendTelemetry(clientData){
	var mySAS = createSASToken()

	// Send the request to the Event Hub
	var options = {
	  hostname: nameSpace + '.servicebus.windows.net',
	  port: 443,
	  path: '/' + hubName + '/publishers/' + deviceName + '/messages',
	  method: 'POST',
	  headers: {
	    'Authorization': mySAS,
	    'Content-Length': clientData.length,
	    'Content-Type': 'application/atom+xml;type=entry;charset=utf-8'
	  }
	};

	var req = https.request(options, function(res) {
      console.log("statusCode: ", res.statusCode);

      var responseData = {
		"Action": "SuccessfulTelemetrieIngest",
		"Code": res.statusCode,
		"ContentType": "text/plain",
      };
	  //responder.respond(response, responseData);
	  console.log('You sent me: '+ data);

      res.on('data', function(d) {
	    process.stdout.write(d);
	  });
	});
	 
	req.on('error', function(e) {
	  console.error(e);
	});
	 
	req.write(clientData);
	req.end();

}

exports.sendTelemetry = sendTelemetry; 
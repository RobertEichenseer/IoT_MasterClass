var https = require('https');
var crypto = require('crypto');
var moment = require('moment');
 
// Event Hubs parameters
var nameSpace = 'IoTMC-ns';
var hubName ='IoTMC';
var deviceName = 'Device01';

// Shared access key (from Event Hub configuration)
var policyName = 'Device01';
var policyKey = '<< Add your SAS here >>';
 
// Full Event Hub publisher URI
var eventHubUri = 'https://' + nameSpace + '.servicebus.windows.net' + '/' + hubName + '/publishers/' + deviceName + '/messages';
 
// Create a SAS token
// See http://msdn.microsoft.com/library/azure/dn170477.aspx
function createSASToken(uri, policyName, key)
{
    // Token expires in one hour
    var expiry = moment().add(1, 'hours').unix();
 
    var stringToSign = encodeURIComponent(uri) + '\n' + expiry;
    var hmac = crypto.createHmac('sha256', key);
    hmac.update(stringToSign);
    var signature = hmac.digest('base64');
    var token = 'SharedAccessSignature sr=' + encodeURIComponent(uri) + '&sig=' + encodeURIComponent(signature) + '&se=' + expiry + '&skn=' + policyName;
 
    return token;
}

function sendTelemetry(clientData, response, responder){
	var mySAS = createSASToken(eventHubUri, policyName, policyKey)

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
	  responder.respond(response, responseData);

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
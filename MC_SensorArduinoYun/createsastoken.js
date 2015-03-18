var crypto = require('crypto'); 
var moment = require('moment');

 
// Event Hubs parameters 
var namespace = 'IoTMC-ns'; 
var hubname = 'iotmc'; 
var deviceName = 'Device01'; 
var eventHubAccessKeyName = 'Device01'; 
var eventHubAccessKey = '<<Add your Shared Access Key here >>'; 

 

 
// Full Event Hub publisher URI 
var eventHubUri = 'https://' + namespace + '.servicebus.windows.net' + '/' + hubname + '/publishers/' + deviceName + '/messages'; 
  
function createSASToken(uri, keyName, key) 
{ 
    var expiry = moment().add(12, 'hours').unix();

    var signedString = encodeURIComponent(uri) + '\n' + expiry; 
    var hmac = crypto.createHmac('sha256', key); 
    hmac.update(signedString); 
    var signature = hmac.digest('base64'); 
    var token = 'SharedAccessSignature sr=' + encodeURIComponent(uri) + '&sig=' + encodeURIComponent(signature) + '&se=' + expiry + '&skn=' + keyName; 
  
    return token; 
} 
  
var createdSASToken = createSASToken(eventHubUri, eventHubAccessKeyName, eventHubAccessKey) 
console.log(createdSASToken); 

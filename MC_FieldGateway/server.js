var http = require("http");
var router = require("./router");

function start() {
	function onRequest(request, response){
		
		router.route(request, response);
	}
	http.createServer(onRequest).listen(80);
}

exports.start = start; 


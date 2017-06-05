var getHeadTag = document.getElementsByTagName('head')[0];
var newScriptTag = document.createElement('script');
newScriptTag.type = 'text/javascript';
newScriptTag.src = 'http://localhost/AQALibrary.js';
getHeadTag.appendChild(newScriptTag);
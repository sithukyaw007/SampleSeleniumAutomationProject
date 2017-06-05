function openURL() {
    var oShell = new ActiveXObject("Shell.Application");
    var commandtoRun = "C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe";
    oShell.ShellExecute(commandtoRun, inputparms, "", "open", "1");
}
function HandleHTMLElements(elementRef) {
    this.objectReference = elementRef;
    this.ClickOnElement = PerformClick;
    this.VerticalScroll = VerticalScroll;
    this.HorizontalScroll = HorizontalScroll;
}
function PerformClick() {
    alert("A click has been performed on : " + this.objectReference);
}
function VerticalScroll() {
    alert("A Vertical Scroll has been performed on : " + this.objectReference);
}
function HorizontalScroll() {
    alert("A Horizontal Scroll has been performed on : " + this.objectReference);
}
function CreateHandleHTMLElementsObject(elementReference) {
    var htmlElement = new HandleHTMLElements(elementReference);
    return htmlElement;
}

function SayHi($htmlElement) {
    //return "hello your body is injected with JS!!!";
	$htmlElement.value = "Bharat";
	//var e1 = $(htmlElement);
	//$e1.value = "Bharat";
}

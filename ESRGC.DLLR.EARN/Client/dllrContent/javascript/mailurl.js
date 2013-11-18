function mailThisUrl(){

var u = window.location.href;
		
var m = "I thought this might interest you -  ";
  
      // the following expression must be all on one line...
   window.location.href = "mailto:?subject="+document.title+"&body="+m+document.title+"   "+u;
   
}

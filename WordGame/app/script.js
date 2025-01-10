const clientID = ""

async function getClientId() {
const response = await fetch('/invite/');
clientID = await response.json();
return clientID;
}

$("#inviteBtn").on("click",function(){

    console.log(getClientId())
})
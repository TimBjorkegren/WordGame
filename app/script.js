document.addEventListener('DOMContentLoaded', function () {
    let countDownTime = 60;
    const countDownElement = document.getElementById('countdown');
    countDownElement.textContent = countDownTime;

    const interval = setInterval(function () {
        countDownTime--;
        countDownElement.textContent = countDownTime;
        if (countDownTime == 50) {
            square3.style.display = 'block';
        }

        if (countDownTime == 40) {
            square2.style.display = 'block';
        }

        if (countDownTime == 30) {
            square4.style.display = 'block';
        }

        if (countDownTime == 20) {
            square1.style.display = 'block';
        }

        if (countDownTime == 10) {
            square5.style.display = 'block';
        }

        if (countDownTime <= 0) {
            clearInterval(interval);
            countDownElement.textContent = 'Time is up!';
        }
    }, 1000);
});

document.getElementById("inviteBtn").addEventListener("click", function () {
    fetch('http://127.0.0.1:5000/generate-invite', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        credentials: 'include'
    })
        .then(response => response.json())
        .then(data => {
            startLobby(data.invite_code);
            document.getElementById("inviteCodeDisplay").innerText = `Your invite code is: ${data.invite_code}, Your cookie is: ${data.client_id}`;
        })
        .catch(error => {
            console.error(error);
        });
});

document.getElementById("playBtn").addEventListener("click", () => {
    const inviteCode = prompt("Enter Invite Code");

    if (inviteCode) {
        fetch("http://127.0.0.1:5000/api/validateinvite/validate-invite", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ inviteCode }),
            credentials: 'include'
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else if (response.status === 404) {
                    throw new Error("Invite code does not match.");
                }
            })
            .then(data => {
                alert(data.message);
                startLobby(inviteCode);
            })

            .catch(error => {
                alert(error.message);
            });
    } else {
        alert("You must enter an invite code!");
    }
});

function startLobby(lobbyId) {
    const eventSource = new EventSource(`http://127.0.0.1:5000/api/ValidateInvite/gamestatus/${lobbyId}`);

    eventSource.addEventListener("gameStarted", (event) => {
        console.log("Your game is starting");
    })

    eventSource.onerror = () => {
        console.log('Error occurred');
    };

    eventSource.onopen = () => {
        console.log('Connected to the SSE endpoint for lobby' + lobbyId);
    };

    eventSource.onclose = () => {
        console.log('Disconnected from the SSE endpoint for lobby' + lobbyId);
    };
};
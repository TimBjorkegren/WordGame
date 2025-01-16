function startTimer() {
    let countDownTime = 60;
    const countDownElement = document.getElementById('countdown');
    const inputBox = document.getElementById('inputbox');
    const submitButton = document.querySelector('.inputboxcontainer button');
    let scrambledWordArray = [];
    let wordFetched = false;
    let player1Score = 0;
    countDownElement.textContent = countDownTime;

    const squares = [
        ...document.getElementsByClassName('square'),
        document.getElementById('square1'),
        document.getElementById('square2'),
        document.getElementById('square3'),
        document.getElementById('square4'),
        document.getElementById('square5')
    ];

    const squareVisibility = Array(squares.length).fill(false);
    squareVisibility[0] = true;
    squareVisibility[1] = true;
    squareVisibility[2] = true;
    squareVisibility[3] = true;
    squareVisibility[4] = true;

    const interval = setInterval(function () {
        countDownTime--;
        countDownElement.textContent = countDownTime;

        if (countDownTime === 50) {
            squares[5].style.display = 'flex';
            squareVisibility[5] = true;
        }
        if (countDownTime === 40) {
            squares[6].style.display = 'flex';
            squareVisibility[6] = true;
        }
        if (countDownTime === 30) {
            squares[7].style.display = 'flex';
            squareVisibility[7] = true;
        }
        if (countDownTime === 20) {
            squares[8].style.display = 'flex';
            squareVisibility[8] = true;
        }
        if (countDownTime === 10) {
            squares[9].style.display = 'flex';
            squareVisibility[9] = true;
        }
        if (countDownTime <= 0) {
            clearInterval(interval);
            countDownElement.textContent = 'Time is up!';
            inputbox.style.display = 'none';
            submitButton.style.display = 'none';
        }
    }, 1000);
    };
    document.getElementById("inviteBtn").addEventListener("click", function () {
        fetch('http://localhost:5000/generate-invite', {
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
            fetch("http://localhost:5000/api/validateinvite/validate-invite", {
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
        const eventSource = new EventSource(`http://localhost:5000/api/ValidateInvite/gamestatus/${lobbyId}`);
    
        eventSource.addEventListener("gameStarted", (event) => {
            startGame(eventSource);
    
        })
    
        eventSource.onerror = (event) => {
            console.error("Error occurred:", event);
            console.log("ReadyState:", eventSource.readyState);
            eventSource.close();
        };
    
        eventSource.onopen = () => {
            console.log('Connected to the SSE endpoint for lobby' + lobbyId);
        };
    
        eventSource.onclose = () => {
            console.log('Disconnected from the SSE endpoint for lobby' + lobbyId);
        };
    };
    
    function startGame(eventSource) {
        if (eventSource) {
            eventSource.close();
            var gameContainer = document.getElementById("gameContainer");
            gameContainer.style.display = "block";
            gameContainer.style.pointerEvents = "auto";
            var buttonsContainer = document.getElementById("btnContainer");
            buttonsContainer.style.display = "none";
            buttonsContainer.style.pointerEvents = "none";
            startTimer();
            console.log("disconnecting")


    async function fetchRandomWord() {
        try {
            const response = await fetch('http://localhost:5000/api/word/random');
            if (!response.ok) {
                throw new Error(`Error fetching word: ${response.statusText}`);
            }
            const data = await response.json();
            console.log('Fetched word:', data.word);
            return data.word;
        } catch (error) {
            console.error('Error:', error);
            return null;
        }
    }

    function scrambleWord(word) {
        if (!word) return null;
        return word
            .toUpperCase()
            .split('')
            .sort(() => Math.random() - 0.5)
            .join('');
    }

    async function initWord() {
        if (wordFetched) return;
        const word = await fetchRandomWord();
        if (word) {
            scrambledWordArray = scrambleWord(word).split('');
            console.log('Scrambled word:', scrambledWordArray);
            wordFetched = true;
        } else {
            console.log('Failed to fetch word');
        }
    }

    async function fillFirstFiveSquares() {
        await initWord();
        if (wordFetched) {
            let wordIndex = 0;
            for (let i = 0; i < 5; i++) {
                if (wordIndex < scrambledWordArray.length) {
                    squares[i].innerText = scrambledWordArray[wordIndex];
                    console.log(`Placing letter "${scrambledWordArray[wordIndex]}" in square ${i + 1}`);
                    wordIndex++;
                }
            }
        }
    }

    fillFirstFiveSquares();

    async function displayWord() {
        await initWord();
        if (wordFetched) {
            let wordIndex = 5;
            for (let i = 5; i < squares.length; i++) {
                if (squareVisibility[i] && wordIndex < scrambledWordArray.length) {
                    squares[i].innerText = scrambledWordArray[wordIndex];
                    console.log(`Placing letter "${scrambledWordArray[wordIndex]}" in square ${i + 1}`);
                    wordIndex++;
                }
            }
        }
    }

    const wordUpdateInterval = setInterval(() => {
        displayWord();
    }, 1001);

    setTimeout(() => clearInterval(wordUpdateInterval), 60001);

    function isWordValid(input) {
        
        const visibleLetters = squares
            .filter((_, index) => squareVisibility[index])
            .map(square => square.innerText);

        const inputLetters = input.toUpperCase().split('');

        for (const letter of inputLetters) {
            const letterIndex = visibleLetters.indexOf(letter);
            if (letterIndex === -1) {
                return false;
            }
            visibleLetters.splice(letterIndex, 1);
        }

        return true;
    }

    async function validateWordWithBackend(word) {
        try {
            const response = await fetch('http://localhost:5000/api/word/validate', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ word })
            });
            if (!response.ok) {
                throw new Error(`Error validating word: ${response.statusText}`);
            }
            const data = await response.json();
            return data.isValid;
        } catch (error) {
            console.error('Error:', error);
            return false;
        }
    }
    inputBox.addEventListener('keydown', async (event) => {
        if (event.key === 'Enter') {
            await submitWord();
        }
    });
    
    // Add an event listener for the submit button click
    submitButton.addEventListener('click', async () => {
        await submitWord();
    });
    
    // Define the common function for word submission
    async function submitWord() {
        const inputWord = inputBox.value.trim();
        if (!inputWord) {
            alert('Please enter a word!');
            return;
        }
    
        if (!isWordValid(inputWord)) {
            alert('Invalid word! You can only use the letters in the visible squares.');
            return;
        }
    
        console.log('Sending word:', inputWord);
        const correctContainer = document.getElementById('correct-container');
        const incorrectContainer = document.getElementById('incorrect-container');
        const isValid = await validateWordWithBackend(inputWord);
        if (isValid) {
            player1Score++;
            correctContainer.classList.add('show');
            setTimeout(() => {
                correctContainer.classList.remove('show');
            }, 1000);
            inputBox.value = '';
        } else {
            incorrectContainer.classList.add('show');
            setTimeout(() => {
                incorrectContainer.classList.remove('show');
            }, 1000);
        }
    }
    }
}
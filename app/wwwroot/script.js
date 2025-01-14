document.addEventListener('DOMContentLoaded', function () {
    let countDownTime = 60;
    const countDownElement = document.getElementById('countdown');
    countDownElement.textContent = countDownTime;

    const firstFiveSquares = [
        document.getElementsByClassName('square')[0],
        document.getElementsByClassName('square')[1],
        document.getElementsByClassName('square')[2],
        document.getElementsByClassName('square')[3],
        document.getElementsByClassName('square')[4]
    ];

    const squares = [
        document.getElementById('square1'),
        document.getElementById('square2'),
        document.getElementById('square3'),
        document.getElementById('square4'),
        document.getElementById('square5')
    ];

    const squareVisibility = [false, false, false, false, false];


    let scrambledWordArray = [];
    let wordFetched = false;


    const interval = setInterval(function () {
        countDownTime--;
        countDownElement.textContent = countDownTime;
        if (countDownTime == 50) {
            square1.style.display = 'block';
            squareVisibility[0] = true;
        }

        if (countDownTime == 40) {
            square2.style.display = 'block';
            squareVisibility[1] = true;
        }

        if (countDownTime == 30) {
            square3.style.display = 'block';
            squareVisibility[2] = true;
        }

        if (countDownTime == 20) {
            square4.style.display = 'block';
            squareVisibility[3] = true;
        }

        if (countDownTime == 10) {
            square5.style.display = 'block';
            squareVisibility[4] = true;
        }

        if (countDownTime <= 0) {
            clearInterval(interval);
            countDownElement.textContent = 'Time is up!';
        }
    }, 1000);

    async function fetchRandomWord() {
        try {
            const response = await fetch('/api/word/random');
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
            scrambledWordArray = scrambleWord(word);
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

            for (let i = 0; i < firstFiveSquares.length; i++) {
                if (wordIndex < scrambledWordArray.length) {
                    firstFiveSquares[i].innerText = scrambledWordArray[wordIndex];
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

            for (let i = 0; i < squares.length; i++) {
                if (squareVisibility[i] && wordIndex < scrambledWordArray.length) {
                    squares[i].innerText = scrambledWordArray[wordIndex];
                    console.log(`Placing letter "${scrambledWordArray[wordIndex]}" in square ${i + 1}`);
                    wordIndex++;
                } else {
                    console.log(`Square ${i + 1} is not visible or wordIndex exceeded`);
                }
            }
        }
    }

    const wordUpdateInterval = setInterval(() => {
        displayWord();
    }, 1001);

    setTimeout(() => clearInterval(wordUpdateInterval), 60001);
});
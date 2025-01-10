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

    async function fetchRandomWord() {
        try {
            const response = await fetch('http://localhost:5000/api/word/random');
            if (!response.ok) {
                throw new Error(`Error fetching word: ${response.statusText}`);
            }
    
            const data = await response.json();
            return data.word;
        } catch (error) {
            console.error('Error:', error);
            return null;
        }
    }

    function scrambleWord(word) {
        if (!word) return null;
    
        // Scrambling the word
        return word
            .split('')
            .sort(() => Math.random() - 0.5)
            .join('');
    }

    async function displayWord() {
        const word = await fetchRandomWord(); // Fetch the word from the API
        if (word) {
            const scrambled = scrambleWord(word);
            document.getElementById('word-container').innerText = scrambled;
        } else {
            document.getElementById('word-container').innerText = 'Error fetching word!';
        }
    }
    
    window.onload = displayWord;

});
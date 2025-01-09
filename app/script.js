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


    function displayWordInSquare(squareId) {
        fetch('http://localhost:5500/api/words/random') // Använd din faktiska API-URL
            .then(response => response.json())
            .then(data => {
                const square = document.getElementById(`square${squareId}`);
                square.textContent = scrambleWord(data.word); // Scrambla ordet innan det visas
                square.style.display = 'block';
            })
            .catch(error => console.error('Error fetching word:', error));
    }

    // Funktion för att scrambla ett ord
    function scrambleWord(word) {
        return word.split('').sort(() => Math.random() - 0.5).join('');
    }
});
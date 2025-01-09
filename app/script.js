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
window.setupInputFocus = function () {
    console.log("setupInputFocus function is called.");

    const inputs = document.querySelectorAll('.totp-digit-input');

    inputs.forEach((input, index) => {
        input.addEventListener('keydown', (e) => {
            if (e.key === "Backspace" && input.value === '') {
                if (index > 0) {
                    inputs[index - 1].focus();
                }
            } else if (e.key !== "Backspace" && input.value !== '' && index < inputs.length - 1) {
                inputs[index + 1].focus();
            }
        });
    });
}

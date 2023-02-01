//Use local storage to store the user's choice
let darkMode = localStorage.getItem('darkModeCoderThoughts');

const darkModeToggle = document.getElementById('dark-mode-toggle');

//Check if dark mode is enabled
//If it's enabled, turn it off
//If it's disabled, turn it on

const enableDarkMode = () => {
    //1) Add a "dark-mode" class to the body
    document.body.classList.remove('light-mode');
    document.body.classList.add('dark-mode');

    //2 Change the header to the dark mode header
    document.getElementById('header-text').src="../images/coder-thoughts-header-dark.png";

    //3) Update darkMode in the local storage to 'enabled'
    localStorage.setItem('darkModeCoderThoughts', 'enabled');
}

const disableDarkMode = () => {
    //1) Add a "light-mode" class to the body
    document.body.classList.remove('dark-mode');
    document.body.classList.add('light-mode');

    //2 Change the header to the light mode header
    document.getElementById('header-text').src = "../images/coder-thoughts-header-light.png";

    //3) Update darkMode in the local storage to null
    localStorage.setItem('darkModeCoderThoughts', null);
}

if (darkMode === 'enabled') {
    enableDarkMode();
}

darkModeToggle.addEventListener('click', () => {
    darkMode = localStorage.getItem('darkModeCoderThoughts');
    if (darkMode !== 'enabled') {
        enableDarkMode();
    } else {
        disableDarkMode();
    }
})


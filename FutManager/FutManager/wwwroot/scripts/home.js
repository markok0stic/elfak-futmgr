var dropDown = document.querySelectorAll(".drop");
dropDown.forEach(drop => {
    let down = true;
    drop.addEventListener("click", () => {
        if (down) {
            drop.nextElementSibling.classList.remove("displayNone");
            drop.nextElementSibling.classList.add("displayFlex");
            down = !down
        }
        else {
            drop.nextElementSibling.classList.remove("displayFlex");
            drop.nextElementSibling.classList.add("displayNone");
            down = !down
        }
    })
})
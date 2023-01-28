const scheduler = "https://localhost:7044";
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

async function startPractice() {
    const payload = JSON.stringify({
    "id": 1,
    "homeSquad":{
        "name":"Brazil"
    },
    "awaySquad":{
        "name":"Aregentina"  
    },
    "scores":[],
    "result":null
    });
    
    await fetch(`${scheduler}/Schedule/StartPractice`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: payload
    }).then(res => {
        if (res.ok) {
            window.location.reload();        
        }
        else {
            alert(`Something went wrong!`);
        }
    });
}

async function scheduleFriendly() {
    let now = new Date();
    let newTime = now.setMinutes(now.getMinutes() + 1);
    let futureTime = new Date(newTime);
    console.log(futureTime)
    const offset = 1;
    const payload = JSON.stringify({
        "id": 2,
        "homeSquad":{
            "name":"Squad1"
        },
        "awaySquad":{
            "name":"Squad2"
        },
        "scores":[],
        "result":null,
        "matchTime":new Date(futureTime.setHours(futureTime.getHours() + offset))
    });

    await fetch(`${scheduler}/Schedule/ScheduleMatch`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: payload
    }).then(res => {
        if (res.ok) {
            window.location.reload();
        }
        else {
            alert(`Something went wrong!`);
        }
    });
}
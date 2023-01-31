const scheduler = "https://localhost:7044";
const manager = "https://localhost:7042";
let playerPageNumber = 1;
let squadPageNumber = 1;

window.addEventListener("load", () => {
    addEventListenerToPlayers();
    addEventListenerToSquads();
})

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

function clearcontent(elementID) {
    document.getElementById(elementID).innerHTML = "";
}

function createPlayer(player) {
    var showList = document.getElementById("shlPlayers");
    var playerDiv = document.createElement("div");
    playerDiv.innerHTML = `<p>${player.firstName}</p>
                            <p>${player.overallRating}</p>
                            <p>${player.nationality}</p>
                            <input type="hidden" value="${player.id}"/>
                            <button id="changePlayer">Change</button>
                            <button id="deletePlayer">Delete</button>`;

    playerDiv.classList.add("borderLine");                  
    showList.appendChild(playerDiv);
}

function createSquad(squad) {
    var showList = document.getElementById("shlSquads");
    var squadDiv = document.createElement("div");
    squadDiv.innerHTML = ` <p>${squad.name}</p>
                            <p>${squad.balance}</p>
                            <input type="hidden" value="${squad.id}" />
                            <button>Change</button>
                            <button id="deleteSquad">Delete</button>`;

    squadDiv.classList.add("borderLine");
    showList.appendChild(squadDiv);
}

function validateSquad() {
    var name = document.getElementById("squadName");
    var balance = document.getElementById("squadBalance");

    if (name.value == "" || balance.value == "" || name.value == null || balance.value == null) {
        return false;
    }
    else {
        return true;
    }
}

function validatePlayer() {
    var name = document.getElementById("playerName").value;
    var lname = document.getElementById("playerLName").value;
    var rating = document.getElementById("playerRating").value;
    var age = document.getElementById("playerAge").value;
    var nationality = document.getElementById("playerNationality").value;
    var position = document.getElementById("playerPosition").value;

    if (name == "" || name == null) return false;
    if (lname == "" || lname == null) return false;
    if (rating == "" || rating == null) return false;
    if (age == "" || age == null) return false;
    if (nationality == "" || nationality == null) return false;
    if (position == "" || position == null) return false;

    return true;
    
}

function addEventListenerToPlayers() {
    var deletePlayer = document.querySelectorAll("#deletePlayer");
    deletePlayer.forEach(btn => {
        btn.addEventListener("click", async () => {
            var showList = document.getElementById("shlPlayers");
            var divToRemove = btn.closest('div');
            var id = divToRemove.children[3].value;
            await fetch(`${manager}/PlayerManager/DeletePlayer/${id}`, {
                method: "DELETE"
            }).then(res => {
                if (res.ok) {
                    showList.removeChild(divToRemove);
                }
                else {
                    alert("Something went wrong!");
                }
            })
        })
    })
}

function addEventListenerToSquads() {
    var deleteSquad = document.querySelectorAll("#deleteSquad");
    deleteSquad.forEach(btn => {
        btn.addEventListener("click", async () => {
            var showList = document.getElementById("shlSquads");
            var divToRemove = btn.closest('div');
            console.log(divToRemove.children)
            var id = divToRemove.children[2].value;
            await fetch(`${manager}/SquadManager/DeleteSquad/${id}`, {
                method: "DELETE"
            }).then(res => {
                if (res.ok) {
                    showList.removeChild(divToRemove);
                }
                else {
                    alert("Something went wrong!");
                }
            })
        })
    })
}


var nextPlayers = document.getElementById("nextPlayers");
nextPlayers.addEventListener("click", async () => {
    await fetch(`${manager}/PlayerManager/GetPlayers/${playerPageNumber}`, {
        method: "GET"
    }).then(async res => {
        if (res.ok) {
            const data = await res.json();
            if (!(data == "" || data == null)) {
                clearcontent("shlPlayers")
                playerPageNumber++;
                data.forEach(player => {
                    createPlayer(player);
                })
                addEventListenerToPlayers();
            }
        }
        else {
            alert(`Something went wrong!`);
        }
    });
});

var previousPlayers = document.getElementById("previousPlayers");
previousPlayers.addEventListener("click", async () => {
    if (playerPageNumber > 0) {
        clearcontent("shlPlayers")
        playerPageNumber--;
        await fetch(`${manager}/PlayerManager/GetPlayers/${playerPageNumber}`, {
            method: "GET"
        }).then(async res => {
            if (res.ok) {
                const data = await res.json();
                data.forEach(player => {
                    createPlayer(player);
                })
                addEventListenerToPlayers();
            }
            else {
                alert(`Something went wrong!`);
            }
        });
    }
});

var nextSquads = document.getElementById("nextSquads");
nextSquads.addEventListener("click", async () => {
    await fetch(`${manager}/SquadManager/GetSquads/${squadPageNumber}`, {
        method: "GET"
    }).then(async res => {
        if (res.ok) {
            const data = await res.json();
            if (!(data == "" || data == null)) {
                clearcontent("shlSquads")
                squadPageNumber++;
                data.forEach(squad => {
                    createSquad(squad);
                })
                addEventListenerToSquads();
            }
        }
        else {
            alert(`Something went wrong!`);
        }
    });
});

var previousSquads = document.getElementById("previousSquads");
previousSquads.addEventListener("click", async () => {
    if (squadPageNumber > 0) {
        clearcontent("shlSquads")
        squadPageNumber--;
        await fetch(`${manager}/SquadManager/GetSquads/${squadPageNumber}`, {
            method: "GET"
        }).then(async res => {
            if (res.ok) {
                const data = await res.json();
                data.forEach(squad => {
                    createSquad(squad);
                })
                addEventListenerToSquads();
            }
            else {
                alert(`Something went wrong!`);
            }
        });
    }
});


var squadBtn = document.getElementById("squadAdd");
squadBtn.addEventListener("click",  async() => {
    if (validateSquad()) {
        var name = document.getElementById("squadName").value;
        var balance = document.getElementById("squadBalance").value;
        await fetch(`${manager}/SquadManager/AddSquad/${name}/${balance}`, {
            method: "POST"
        }).then(res => {
            if (res.ok) {
                alert("Squad added!")
            }
            else {
                alert(`Something went wrong!`);
            }
         });
    }
})

var playerBtn = document.getElementById("playerAdd");
playerBtn.addEventListener("click", async () => {
    if (validatePlayer()) {
        var name = document.getElementById("playerName").value;
        var lname = document.getElementById("playerLName").value;
        var rating = document.getElementById("playerRating").value;
        var age = document.getElementById("playerAge").value;
        var nationality = document.getElementById("playerNationality").value;
        var position = document.getElementById("playerPosition").value;

        const player = JSON.stringify({
            "FirstName" : name,
            "LastName" : lname,
            "OverallRating" : Number(rating),
            "Age" : Number(age),
            "Nationality" : nationality,
            "Position" : Number(position)
        });

        console.log(player)

        await fetch(`${manager}/PlayerManager/AddPlayer`, {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: player
        }).then(res => {
            if (res.ok) {
                alert("Player added!")
            }
            else {
                alert(`Something went wrong!`);
            }
        });
    }
})

var changePlayers = document.querySelectorAll("#changePlayer");
changePlayers.forEach(btn => {
    btn.addEventListener("click", () => {
        var id = btn.closest('div').children[3].value;
        document.querySelector('.popup').children[0].value = id;
        var overlay = document.querySelector(".overlay");
        overlay.style.display = "flex";

    })
})

var cancelBtn = document.getElementById("cancelBtn");
cancelBtn.addEventListener("click", () => {
    var overlay = document.querySelector(".overlay");
    overlay.style.display = "none";
})

async function startPractice() {
    const payload = JSON.stringify({
    "id": 1,
    "homeSquad":{
        "id":1,
        "name":"Brazil"
    },
    "awaySquad":{
        "id":2,
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
            "id":1,
            "name":"Squad1"
        },
        "awaySquad":{
            "id":2,
            "name":"Squad2"
        },
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

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("searchInput").addEventListener("input", filterPlayers);
    updateSelectedCount();
});

function filterPlayers() {
    let input = document.getElementById("searchInput").value.trim().toLowerCase();
    let players = document.querySelectorAll("#playersList li");
    let foundCount = 0;

    players.forEach(player => {
        let nameElement = player.querySelector(".player-name");
        if (!nameElement) return;

        let name = nameElement.textContent.trim().toLowerCase();
        if (name.includes(input)) {
            player.classList.remove("hidden-player");
            player.style.visibility = "visible";
            player.style.position = "relative";
            foundCount++;
        } else {
            player.classList.add("hidden-player");
            player.style.visibility = "hidden";
            player.style.position = "absolute";
        }
    });

    let noResultsMessage = document.getElementById("noResults");
    noResultsMessage.style.display = foundCount === 0 ? "block" : "none";
    noResultsMessage.textContent = foundCount === 0 ? "Brak wyników." : `Znaleziono: ${foundCount}`;
}

function updateSelectedCount() {
    let selectedPlayers = document.querySelectorAll("#selectedPlayersList li").length;
    document.getElementById("selectedCount").textContent = `(${selectedPlayers}/10)`;

    let randomTeamsBtn = document.getElementById("randomTeamsBtn");
    randomTeamsBtn.disabled = selectedPlayers !== 10;
}

function addPlayer(event, form) {
    event.preventDefault();

    let playerName = form.parentElement.querySelector(".player-name").textContent;
    let playerId = form.querySelector("input[name='id']").value;

    if (document.querySelectorAll("#selectedPlayersList li").length >= 10) return;

    let li = document.createElement("li");
    li.className = "list-group-item d-flex justify-content-between align-items-center";
    li.innerHTML = `
        ${playerName}
        <form method="post" asp-action="RemoveFromSelected" onsubmit="return removePlayer(event, this);">
            <input type="hidden" name="id" value="${playerId}" />
            <button type="submit" class="btn btn-danger btn-sm">❌ Usuń</button>
        </form>
    `;

    document.getElementById("selectedPlayersList").appendChild(li);

    // 🔹 Ukryj gracza na liście dostępnych
    let availablePlayer = document.querySelector(`#playersList li[data-id="${playerId}"]`);
    if (availablePlayer) {
        availablePlayer.style.display = "none"; // Teraz działa poprawnie
    }

    updateSelectedCount();
}


function removePlayer(event, form) {
    event.preventDefault();

    let playerName = form.parentElement.firstChild.textContent.trim();
    let playerId = form.querySelector("input[name='id']").value;

    // 🔹 Znajdź ukrytego gracza na liście dostępnych i pokaż go
    let availablePlayer = document.querySelector(`#playersList li[data-id="${playerId}"]`);
    if (availablePlayer) {
        availablePlayer.style.display = "flex"; // Teraz poprawnie go pokazujemy
    } else {
        // Jeśli nie istnieje, dodaj go na nowo
        let li = document.createElement("li");
        li.className = "list-group-item d-flex justify-content-between align-items-center";
        li.setAttribute("data-id", playerId); // Dodaj atrybut ID dla wyszukiwania
        li.innerHTML = `
            <span class="player-name">${playerName}</span>
            <form method="post" asp-action="AddToSelected" onsubmit="return addPlayer(event, this);">
                <input type="hidden" name="id" value="${playerId}" />
                <button type="submit" class="btn btn-primary btn-sm">➕ Dodaj</button>
            </form>
        `;

        document.getElementById("playersList").appendChild(li);
    }

    form.parentElement.remove();
    updateSelectedCount();
}



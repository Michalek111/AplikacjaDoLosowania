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


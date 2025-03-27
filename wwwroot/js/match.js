document.addEventListener("DOMContentLoaded", function () {
    let matchDataDiv = document.getElementById("matchData");
    let confirmMatchUrl = matchDataDiv.getAttribute("data-confirm-match-url");
    let selectPlayersUrl = matchDataDiv.getAttribute("data-select-players-url");

    document.getElementById("matchForm").addEventListener("submit", function (event) {
        event.preventDefault();

        let formData = new FormData(this);
        let team1Ids = [...formData.getAll("team1Ids[]")].map(Number);
        let team2Ids = [...formData.getAll("team2Ids[]")].map(Number);

        let duplicates = team1Ids.filter(id => team2Ids.includes(id));
        if (duplicates.length > 0) {
            showErrorMessage("⚠️ Gracz nie może być jednocześnie w obu drużynach!");
            return;
        }

        let jsonData = {
            team1Score: parseInt(document.getElementById("team1Score").value) || 0,
            team2Score: parseInt(document.getElementById("team2Score").value) || 0,
            team1Ids: [...formData.getAll("team1Ids[]")].map(Number),
            team2Ids: [...formData.getAll("team2Ids[]")].map(Number),
            map: document.getElementById("mapSelection").value
        };

        
        let matchIdInput = document.getElementById("matchId");
        if (matchIdInput) {
            jsonData.matchId = parseInt(matchIdInput.value);
        }


        fetch(confirmMatchUrl, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(jsonData)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    window.location.href = selectPlayersUrl;
                } else {
                    showErrorMessage(data.message);
                }
            })
            .catch(error => console.error("Błąd:", error));
    });

    function showErrorMessage(message) {
        let errorDiv = document.getElementById("errorMessage");
        errorDiv.textContent = message;
        errorDiv.style.display = "block";
        document.getElementById("team1Score").value = "";
        document.getElementById("team2Score").value = "";
    }
});

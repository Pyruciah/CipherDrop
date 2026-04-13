const API_BASE = "https://localhost:7200/api";

function goBack() {
    window.history.back();
}

// Save token
function saveToken(token) {
    localStorage.setItem("token", token);
}

// Get token
function getToken() {
    return localStorage.getItem("token");
}

// Auth header
function authHeader() {
    return {
        "Authorization": "Bearer " + getToken()
    };
}
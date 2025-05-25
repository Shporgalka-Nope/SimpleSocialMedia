let offset = 0;
let limit = 4;

let container = document.getElementById("posts-list");
let refresh = document.getElementById("refresh");
let username = document.getElementById("username").innerText;

const domain = window.location.hostname;

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            console.log("Update requested")
            RequestNewData(`/api/GetPosts/${username}?offset=${offset}&limit=${limit}`);
        }
    })
}, { threshold: 0.1 });

observer.observe(refresh);

function RequestNewData(url) {
    fetch(url)
        .then(response => response.text())
        .then(html => container.insertAdjacentHTML("beforeend", html));
    offset += limit;
}
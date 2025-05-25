let offset = 0;
let limit = 30;

let container = document.getElementById("search-body");
let refresh = document.getElementById("refresh");

const domain = window.location.hostname;


document.getElementById("search-input").addEventListener("input", function (event) {
    fetch(`/api/getprofiles/${event.target.value}`)
    .then(response => response.text())
    .then(html => container.innerHTML = html);
})

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            RequestNewData(`/api/getprofiles?offset=${offset}&limit=${limit}`);
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
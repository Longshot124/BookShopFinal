var searchInput = document.getElementById("search-products");
if (searchInput) {
    searchInput.addEventListener("keyup", function () {
        console.log("Ishlayir")
        let text = searchInput.value
        let courseList = document.querySelector("#product-list")
        console.log(courseList)
        fetch('Home/Search?searchText=' + text)
            .then((response) => response.text())
            .then((data) => {
                console.log(data)
                courseList.innerHTML = data
            });

    });
}
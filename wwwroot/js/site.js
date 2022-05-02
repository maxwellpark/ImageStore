const imageUrl = "api/Image";
const uploadForm = document.getElementById("upload-form");
const fileInput = document.getElementById("file-input");
const fileContentsHolder = document.getElementById("file-contents");

if (uploadForm) {
    uploadForm.addEventListener("submit", uploadImage);
}

function uploadImage(event) {
    event.preventDefault();
    console.log("Upload form submitted", event);

    const reader = new FileReader();

    reader.onerror = function () {
        console.log("Error:", reader.error);
    }
    reader.onloadend = async function () {
        //const response = await fetch("https://localhost:44385/api/Image", {
        //    method: "POST",
        //    headers: {
        //        "Content-Type": "multipart/form-data; boundary=XXX"
        //    },
        //    body: reader.result
        //});
        //console.log("Response:", response);

        fetch("https://localhost:44385/api/Image", {
            method: "POST",
            headers: {
                "Content-Type": "multipart/form-data; boundary=XXX"
            },
            body: reader.result
        })
            .then(resp => console.log(resp))
            .catch(err => console.log(err));
    }

    if (fileInput.files[0]) {
        reader.readAsText(fileInput.files[0]);
    }
}
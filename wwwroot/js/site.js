const requestUrl = "api/Image";
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
        const formData = new FormData(uploadForm);

        await fetch(requestUrl, {
            method: "POST",
            body: formData
        }).then(resp => {
            console.log(resp);
        }).catch(err => {
            console.log(err);
        });

        document.location.reload(true);
    }

    if (fileInput.files[0]) {
        reader.readAsText(fileInput.files[0]);
    }
}
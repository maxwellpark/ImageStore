const requestUrl = "api/Image";
const uploadForm = document.getElementById("upload-form");
const fileInput = document.getElementById("file-input");
const fileContentsHolder = document.getElementById("file-contents");
const captionInput = document.getElementById("caption-input");
const tagInput = document.getElementById("tag-input");
const tagsCapture = document.getElementById("tags-capture");
const addTagButton = document.getElementById("add-tag-button");

if (uploadForm) {
    uploadForm.addEventListener("submit", uploadImage);
}

addTagButton.onclick = function (event) {
    event.preventDefault();
    addTag();
}

addDeleteListeners();

function uploadImage(event) {
    event.preventDefault();
    console.log("Upload form submitted", event);

    const reader = new FileReader();

    reader.onerror = function () {
        console.log("Error:", reader.error);
    }
    reader.onloadend = async function () {
        const formData = new FormData(uploadForm);
        formData.append("caption", captionInput.value);
        formData.append("tags", tagsCapture.innerText);

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

function addDeleteListeners() {
    const imageContainers = document.getElementsByClassName("stored-image-container");
    for (const container of imageContainers) {
        const imageId = container.getAttribute("data-image-id");
        const deleteIcon = container.querySelector("i.delete-icon");

        deleteIcon.onclick = async function (event) {
            event.preventDefault();
            await fetch(requestUrl + "/" + imageId, {
                method: "DELETE"
            }).then(resp => {
                console.log(resp);
            }).catch(err => {
                console.log(err);
            });
            document.location.reload(true);
        }
    }
}

function addTag() {
    if (!tagInput.value) {
        return;
    }
    let tag = "";
    if (tagsCapture.innerText != "") {
        tag += ",";
    }
    tag += tagInput.value;
    tagsCapture.innerText += tag;
    tagInput.value = "";
}
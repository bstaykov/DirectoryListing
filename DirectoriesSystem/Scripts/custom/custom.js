function checkFileSize(inputId, maxSize) {
    var input = document.getElementById(inputId);
    if (input.files && input.files.length == 1) {
        if (input.files[0].size > maxSize) {
            alert("File must be less than 2MB.");
            return false;
        }
    }
    return true;
}
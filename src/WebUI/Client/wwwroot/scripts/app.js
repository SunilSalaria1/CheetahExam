var app = {};

app.hideModal = function (element) {
    bootstrap.Modal.getInstance(element).hide();
}

window.DownloadExamInExcel = (fileName, content) => {
    const blob = new Blob([content], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
};
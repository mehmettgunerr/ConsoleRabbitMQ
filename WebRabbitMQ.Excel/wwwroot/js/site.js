$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/MyHub").build();

    start();
    function start() {
        connection.start().then().catch((err) => {
            console.log(err);
            setTimeOut(() => start(), 2000);
        });
    }

    connection.on("CompletedFile", () => {
        Swal.fire({
            position: 'bottom-end',
            icon: 'success',
            title: 'Excel dosyanız hazır',
            showConfirmButton: true,
            confirmButtonText: "Dosyalarım"
        }).then((result) => {
            if (result.isConfirmed) {
                window.location = "/product/files";
            }
        })
    });

});
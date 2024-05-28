window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    console.log("uri：",uri);
    new QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 150,
            height: 150
        });
});
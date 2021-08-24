(function () {
    const head = document.getElementsByTagName("head")[0];

    // css
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = "https://cdn.syncfusion.com/ej2/material.css";
    head.appendChild(link);

    // js
    const script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "https://cdn.syncfusion.com/ej2/dist/ej2.min.js";
    head.appendChild(script);    
})();
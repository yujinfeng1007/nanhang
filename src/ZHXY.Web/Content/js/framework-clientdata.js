$(function () {
    window.clients = $.clientsInit();
    localStorage.clear();
    for (var k in clients) {
        localStorage.setItem(k, JSON.stringify(clients[k]));
    }
})

function getLimYear(data) {
    var output = {}
    var currentYear = new Date().getFullYear()
    var yearRange = []
    for (i = 0; i < 5; i++) {
        yearRange.push(currentYear + i)
    }
    $.each(data, function (n, t) {
        $.each(yearRange, function (i, item) {
            if (Number(n) === item) {
                output[n] = t
            }
        })
    })
    return output
}

$.clientsInit = function () {
    let dataJson = {
        dataItems: [],
        organize: [],
        role: [],
        duty: [],
        authorizeMenu: [],
        authorizeButton: [],
    };
    (() => {
        $.ajax({
            url: "/Home/ClientData?clientType=1",
            type: "get",
            dataType: "json",
            async: false,
            success: function (res) {
                let data = res.data;
                dataJson.dataItems = data.dataItems;
                dataJson.organize = data.organize;
                dataJson.role = data.role;
                dataJson.duty = data.duty;
                dataJson.authorizeMenu = eval(data.authorizeMenu);
                dataJson.authorizeButton = data.authorizeButton;
            }
        });
    })();
    return dataJson;
}
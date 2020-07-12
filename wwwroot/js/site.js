var times = [];
var secondTimer = null;
var monsters = { @foreach(var mob in Model.Monsters) { @Html.Raw($"'{mob.MonsterId}' : '{mob.Name}', "); } }


function pad(nr) {
    if (nr > 9)
        return nr;
    return "0" + nr;
}

function updateTime(data) {
    var el = $("#" + data.channel + "_" + data.monsterId);

    var txt = "Unknown";
    if (data.lastSpot != -999999) {
        if (data.lastSpot < -60 * 60)
            txt = "Expired";
        else if (data.lastSpot < 0)
            txt = "Spawned";
        else
            txt = "☠" + Math.floor(data.lastSpot / (60 * 60)) + ":" + pad(Math.floor(data.lastSpot / 60) % 60) + ":" + pad(Math.floor(data.lastSpot % 60));
    } else if (data.lastNoSpot != null) {
        txt = "🛑" + Math.floor(data.lastNoSpot / (60 * 60)) + ":" + pad(Math.floor(data.lastNoSpot / 60) % 60) + ":" + pad(Math.floor(data.lastNoSpot % 60));
    }

    el.html(txt);


    el = $("#sort" + data.channel + "_" + data.monsterId);
    if (el.length == 0) {
        el = $(`
<li id="sort` + data.channel + "_" + data.monsterId + `" class="list-group-item py-1">
    <span class="badge-pill badge-info">` + data.channel + `</span>
    <span class="badge-pill badge-secondary">` + txt + `</span><span>` +
            monsters[data.monsterId] + " " +
            `</span>   <a class="btn-sm btn-success" onclick="update('kill', '` + data.channel + `', ` + data.monsterId + `)">☠</a>
    <a class="btn-sm btn-danger" onclick="update('nospot', '`+ data.channel + `', ` + data.monsterId + `))">🛑</a>
</li>`)
        $("#timelist").append(el);
    }
    el.find("span")[1].innerText = txt;

}

function getTimeIndex(channel, monsterId) {
    for (var i = 0; i < times.length; i++)
        if (times[i].channel == channel && times[i].monsterId == monsterId)
            return i;
    return -1;
}

function updateFromServer() {
    $.getJSON("/api", function (data) {

        for (var i in data) {
            var index = getTimeIndex(data[i].channel, data[i].monsterId);
            if (index == -1)
                times.push(data[i]);
            else
                times[index] = data[i];
            updateTime(data[i]);
        }
    });
}
function updateSeconds() {
    for (var i in times) {
        if (times[i].lastSpot != -999999)
            times[i].lastSpot--;
        if (times[i].lastNoSpot >= 0)
            times[i].lastNoSpot++;
        updateTime(times[i]);
    }

    sortChannels();
}

function sortChannels() {
    times.sort(function (a, b) {
        if (a.lastSpot != -999999 && b.lastSpot != -999999)
            return Math.sign(a.lastSpot - b.lastSpot);
        else if (a.lastSpot != -999999 && b.lastSpot == -999999)
            return -1;
        else if (b.lastSpot != -999999 && a.lastSpot == -999999)
            return 1;
        return Math.sign(a.lastNoSpot - b.lastNoSpot);
    });
    for (var i = 0; i < times.length; i++) {
        var el = $("#sort" + times[i].channel + "_" + times[i].monsterId);
        $("#timelist").append(el);
    }
}

function update(action, channel, mob) {
    $.ajax("/api/" + action, {
        data: JSON.stringify(
            {
                Channel: channel,
                MonsterId: mob
            }),
        contentType: 'application/json',
        type: "POST",
        success: function (data) {
            var index = getTimeIndex(data.channel, data.monsterId);
            if (index == -1)
                times.push(data);
            else
                times[index] = data;
            updateTime(data);
            sortChannels();
        }
    });
}

function clearMob(mob) {
    $.ajax("/api/clear", {
        data: JSON.stringify(
            {
                MonsterId: mob
            }),
        contentType: 'application/json',
        type: "POST",
        success: function (data) {
            location.reload();
        }
    });
}

function hide(mobId) {
    $(".mob-" + mobId).hide();
}

$(document).ready(function () {
    secondTimer = setInterval(updateSeconds, 1000);
    setInterval(updateFromServer, 10000);
    updateFromServer();

});
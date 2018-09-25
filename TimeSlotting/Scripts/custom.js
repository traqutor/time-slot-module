$(document).ready(function () {
    $("#timeslot-date").click(function (e) {
        $("#timeslot-date").blur();
    }); 
});

function OpenDialog(name, elem) {
    $(name).modal('show');
}

function CloseDialog(name, elem) {
    $(name).modal('hide');
}

function PopOverClose(name) {
    $(name).popover('hide');
    $(name + '.fade').remove();
}
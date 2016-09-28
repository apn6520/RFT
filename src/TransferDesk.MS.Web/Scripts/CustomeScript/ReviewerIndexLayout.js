function ShowSuccessMessage(message) {

    $('#NotificationMessageControl').removeClass("InfoMessage");
    $('#NotificationMessageControl').removeClass("ErrorMessage");
    $('#NotificationMessageControl').addClass("SaveMessage");
    $('#NotificationMessageControl').fadeIn('slow');
    $('#NotificationMessage').html(message);
    $('#NotificationSubMessage').hide();
    $('#CloseButton').show();
    $("#non-vendorCode").height("40%");
    setTimeout(function () {
        $('#NotificationMessageControl').fadeOut('slow', function () { });
    }, 4000);
}
function ShowFailureResponseMessage(message) {

    $('#NotificationMessageControl').removeClass("InfoMessage");
    $('#NotificationMessageControl').removeClass("ErrorMessage");
    $('#NotificationMessageControl').addClass("ErrorMessage");
    $('#NotificationMessageControl').fadeIn('slow');
    $('#NotificationMessage').html(message);
    $('#NotificationSubMessage').hide();
    $('#CloseButton').show();
    $("#non-vendorCode").height("40%");
    setTimeout(function () {
        $('#NotificationMessageControl').fadeOut('slow', function () { });
    }, 4000);
}
function HideNotification() {
    $('#CloseButton').hide();
    $("#NotificationMessageControl").hide();
}
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ConfirmDelete(uniqueId, isTrue) {
    var Deletespan = ' Deletespan_' + uniqueId;
    var ConfirmDeletespan = 'ConfirmDeletespan_' + uniqueId;

    if (isTrue) {
        $('#' + Deletespan).hide();
        $('#' + ConfirmDeletespan).show();
    }
    else {
        $('#' + Deletespan).show();
        $('#' + ConfirmDeletespan).hide();

    }
}
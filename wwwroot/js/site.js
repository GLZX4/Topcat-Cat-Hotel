// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    var errorMsg = $(".codeErrorMsg").text().trim();
    var errorMsgLogin = $(".body--signIn--errorMsg").text().trim();
    if (errorMsg.length > 0) {
        $(".codeErrorMsg").addClass("error");
    } else if (errorMsgLogin.length > 0) {
        $(".body--signIn--errorMsg").addClass("error");
    }
});
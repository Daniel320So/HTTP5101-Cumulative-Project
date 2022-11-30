// script for adding new teacher

const loadPage = () => {

    const addTeacher = () => {

        //Reset background color
        $("#teacherFname").removeClass("error")
        $("#teacherLname").removeClass("error")
        $("#employeeNumber").removeClass("error")
        $("#salary").removeClass("error")

        //Input valitdations
        if ($("#teacherFname").val() == "" || $("#teacherFname").val() == null || $("#teacherFname").val() == undefined) {
            $("#teacherFname").addClass("error")
        } else if ($("#teacherLname").val() == "" || $("#teacherLname").val() == null || $("#teacherLname").val() == undefined) {
            $("#teacherLname").addClass("error")
        } else if (!$("#employeeNumber").val().match(/^[A-Z]\w{3}$/)) { //all employee numbers must be start from a upper case letter with 3 digits
            $("#employeeNumber").addClass("error")
        } else if (!($("#salary").val().match(/^\d+$/) || $("#salary").val().match(/^\d+\.{1}\d{0,2}$/))) { // can not have more than 2 decimal places
            $("#salary").addClass("error")
        } else {
            return true;
        }

        return false;
    }

    $("#submit-button").click(addTeacher)

}

$(window).on("load", loadPage)
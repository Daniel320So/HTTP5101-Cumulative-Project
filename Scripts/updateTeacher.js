// script for updating teacher

const loadPage = () => {

    const updateTeacher = () => {

        //Reset background color
        $("#teacherFname").removeClass("error")
        $("#teacherLname").removeClass("error")
        $("#employeeNumber").removeClass("error")
        $("#hireDate").removeClass("error")
        $("#salary").removeClass("error")

        //Input valitdations
        if ($("#teacherFname").val() == "" || $("#teacherFname").val() == null || $("#teacherFname").val() == undefined) {
            $("#teacherFname").addClass("error")
        } else if ($("#teacherLname").val() == "" || $("#teacherLname").val() == null || $("#teacherLname").val() == undefined) {
            $("#teacherLname").addClass("error")
        } else if (!$("#employeeNumber").val().match(/^[A-Z]\w{3}$/)) { //all employee numbers must be start from a upper case letter with 3 digits
            $("#employeeNumber").addClass("error")
        } else if (!$("#hireDate").val().match(/^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) (0?[0-9]|1[0-2]):[0-5]?[0-9]:[0-5]?[0-9] [A|P]M$/)) { //hire date must be in format yyyy/dd/MM hh:mm:ss tt)
            $("#hireDate").addClass("error")
        } else if (!($("#salary").val().match(/^\d+$/) || $("#salary").val().match(/^\d+\.{1}\d{0,2}$/))) { // can not have more than 2 decimal places
            $("#salary").addClass("error")
        } else {
            return true;
        }

        return false;
    }

    $("#submit-button").click(updateTeacher)

}

$(window).on("load", loadPage)
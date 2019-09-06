/// <reference path="jquery-1.4.1.min.js"/>
/// <reference path="../ui/jquery.easyui.min.js" />


$(function () {
    yanzheng();
    $("#users").datagrid({
        fit: true,
        striped: true,
        pagination: true,
        rownumbers: true,
        fitColumns: true,
        pageList: [20, 40, 60],
        pageSize: 20,
        url: "user.axd?type=selectPage",
        columns: [[
            {
                columns: "ck",
                checkbox: true
            },
            {
                field: "id",
                align: "center",
                title: "序号",
                width: 80
            },
            {
                field: "username",
                align: "center",
                title: "用户名",
                width: 80
            },
            {
                field: "cardId",
                align: "center",
                title: "卡号",
                width: 80
            },
            {
                field: "gflag",
                align: "center",
                title: "分组",
                width: 80,
                formatter: function (value) {
                    if (value == "admin") { return '管理员组'; }
                    else if (value == "localuser") {
                        return '内网用户';
                    } else if (value == "authlogin") {
                        return '授权用户';
                    } else if (value == "organizedA") {
                        return '机构A';
                    } else if (value == "organizedB") {
                        return '机构B';
                    } else if (value == "organizedC") {
                        return '机构C';
                    } else if (value == "teacher") {
                        return '教师';
                    } else if (value == "student") {
                        return '学生';
                    } else if (value == "black") {
                        return '黑明单';
                    }
                    else {
                        return '默认分组';
                    }

                }
            },
            {
                field: "state",
                align: "center",
                title: "状态",
                width: 80,
                formatter: function (value) {
                    if (value == true) { return '<input type="checkbox" checked="checked" onclick="return false;"/>'; }
                    else {
                        return '<input type="checkbox" onclick="return false;"/>';
                    }

                }
            }
        ]],
        toolbar: "#addUser"
    });
});


function yanzheng() {

    $("#iprange").validatebox({
        required: true,
        missingMessage: "请填写正确IP范围！"
    });
    $("#username").validatebox({
        required: true,
        missingMessage: "请输入用户名！"
    });
    $("#cardId").validatebox({
        required: true,
        missingMessage: "请输入卡号！"
    });
    $("#password").validatebox({
        required: true,
        missingMessage: "请输入密码！"
    });
    $("#realname").validatebox({
        required: true,
        missingMessage: "请输入真实密码！"
    });
    $("#phone").validatebox({
        required: true,
        missingMessage: "请输入联系号码！"
    });
    $("#nationalId").validatebox({
        required: true,
        missingMessage: "请输入身份证信息！"
    });
    $("#employer").validatebox({
        required: true,
        missingMessage: "请输入单位信息！"
    });
    $("#addr").validatebox({
        required: true,
        missingMessage: "请输入联系地址！"
    });
}
function ajaxFileUpload() {
    $.ajaxFileUpload
        (
        {
            url: 'user.axd?type=Daoru', //用于文件上传的服务器端请求地址
            secureuri: false, //是否需要安全协议，一般设置为false
            fileElementId: 'fileInsert', //文件上传域的ID
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data)  //服务器成功响应处理函数
            {
                var datalist = JSON.parse(data.responseText);
                if (datalist.result != undefined) {
                    if (datalist.result === true) {
                        alert(datalist.msg);
                        window.location.href = "Users.html";
                    }
                    else {
                        if (datalist.result != "none") {
                            alert(datalist.msg);
                            ERRORpath = datalist.file;
                            if ($(".datagrid-toolbar .DownERROR #ERROREXCEL").length <= 0) {
                                $('.datagrid-toolbar .DownERROR').append('<input type="button" id="ERROREXCEL" style="float: left;" value="下载错误用户信息表" onclick="xiaZAI();" />');
                            }
                        }

                    }
                }
                if (datalist.code === "201") {
                    $.messager.alert("提醒", datalist.msg);
                    //alert(datalist.msg);
                }

            },
            error: function (data) {
                var datalist = JSON.parse(data.responseText);
                console.info(datalist);
                if (datalist.result != undefined) {
                    if (datalist.result === true) {
                        alert(datalist.msg);
                        window.location.href = "Users.html";
                    }
                    else {
                        if (datalist.result != "none") {
                            alert(datalist.msg);
                            ERRORpath = datalist.file;
                            if ($(".datagrid-toolbar .DownERROR #ERROREXCEL").length <= 0) {
                                $('.datagrid-toolbar .DownERROR').append('<input type="button" id="ERROREXCEL" style="float: left;" value="下载错误用户信息表" onclick="xiaZAI();" />');
                            }


                        }

                    }
                }
                if (datalist.code === "201") {
                    $.messager.alert("提醒", datalist.msg);
                    //alert(datalist.msg);
                }
            }
        }
        )
    return false;
}
function addusers() {
    $("#add_menber_style").dialog({
        width: 600,
        height: 400,
        title: "添加用户",
        modal: true,
        buttons: [{
            iconCls: "icon-ok",
            text: "添加",
            onClick: function () {
                tijiao();
            }
        },
        {
            iconCls: "icon-cancel",
            text: "取消",
            onClick: function () {
                $.messager.confirm("提醒", "是否取消添加用户", function (data) {
                    if (data)
                        $("#add_menber_style").dialog("close");
                });
                //qingchu();
            }
        }
        ]
    });
}
function UpdateUser() {
    var rows = $("#users").datagrid('getSelections');
    if (rows.length > 1) {
        $.messager.alert("警告", "选择行数大于1，请重新选择");
    }
    else if (rows.length == 0) {
        $.messager.alert("警告", "请选择要修改的用户！");
    }
    else if (rows.length == 1) {
        var id = rows[0].id;
        $.ajax({
            url: "user.axd",
            method: "post",
            data: {
                type: "Update",
                sql: "SELECT * FROM users WHERE id=" + id
            },
            dataType: "json",
            success: function (data) {
                console.info(data);
                $("#Update_menber_style").dialog({
                    width: 600,
                    height: 400,
                    title: "编辑用户",
                    modal: true,
                    buttons: [{
                        iconCls: "icon-ok",
                        text: "保存",
                        onClick: function () {
                            tijiao2();
                        }
                    },
                    {
                        iconCls: "icon-cancel",
                        text: "取消",
                        onClick: function () {
                            $.messager.confirm("提醒", "是否取消编辑用户", function (data) {
                                if (data)
                                    $("#Update_menber_style").dialog("close");
                            });
                        }
                    }
                    ]
                })
                $("#birthdate1").val(data[0].birthdate);
                $("#addr1").val(data[0].addr);
                $("#employer1").val(data[0].employer);
                $("#nationalId1").val(data[0].nationalId);
                $("#phone11").val(data[0].phone);
                $("#gender1").val(data[0].gender);
                $("#part1").val(data[0].gflag);
                $("#realname1").val(data[0].realname);
                $("#password1").val(data[0].password);
                $("#cardId1").val(data[0].cardId);
                $("#username1").val(data[0].username);
                $("#iprange1").val(data[0].iprange);
                $("#userId1").val(data[0].id);
                if (data[0].state == true) {
                    $("#state1").attr("checked", "checked");
                    $("#state1").attr("value", "1");
                    $("#state1").attr("onclick", "statechecked3();");
                } else {
                    $("#state1").attr("onclick", "statechecked2();");
                }
            }
        });
    }
}
var ERRORpath = "";
function inserts() {
    var formData = new FormData();
    var file = $("#fileInsert")[0].files[0];
    //alert(file);
    formData.append("file", file);
    $.ajax({
        url: "user.axd?type=Daoru",
        method: "post",
        data: formData,
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.result !== null) {
                if (data.result === true) {
                    $.messager.alert("提醒", data.msg);
                    //alert(data.msg);
                    window.location.href = "Users.html";
                }
                else {
                    $.messager.alert("提醒", data.msg);
                    //alert(data.msg);
                    if (data.result != "none") {
                        ERRORpath = data.file;
                        $('.DownERROR').append('<input type="button" style="float: left;" value="下载错误用户信息表" onclick="xiaZAI();" />');
                    }

                }
            }
            if (data.code === "201") {
                $.messager.alert("提醒", data.msg);
                //alert(data.msg);
            }
        }
    });
}
function xiaZAI() {
    window.location.href = ERRORpath;
}
function DownRIGHT() {
    $.ajax({
        url: "user.axd",
        method: "post",
        data: { type: "DownNewExcel" },
        dataType: "json",
        success: function (data) {
            window.location.href = data.file;
        }
    });
}
function qingchu() {
    $("#start").val("");
    $("#addr").val("");
    $("#employer").val("");
    $("#nationalId").val("");
    $("#phone").val("");
    $("#gender").val("");
    $("#part").val("");
    $("#realname").val("");
    $("#password").val("");
    $("#cardId").val("");
    $("#username").val("");
    $("#iprange").val("");
    $("#state").css("checked", "");
}
function qingchu2() {
    $("#start1").val("");
    $("#addr1").val("");
    $("#employer1").val("");
    $("#nationalId1").val("");
    $("#phone11").val("");
    $("#gender1").val("");
    $("#part1").val("");
    $("#realname1").val("");
    $("#password1").val("");
    $("#cardId1").val("");
    $("#username1").val("");
    $("#iprange1").val("");
    $("#state1").css("checked", "");
}
var index = 0;
function statechecked() {
    index++;
    if (index % 2 != 0) {
        $("input#state").attr("value", "1");
    }
    else { $("input#state").attr("value", ""); }
}
function statechecked2() {
    index++;
    if (index % 2 != 0) {
        $("input#state1").attr("value", "1");
    }
    else { $("input#state1").attr("value", ""); }
}
function statechecked3() {
    index++;
    if (index % 2 != 0) {
        $("input#state1").attr("value", "");
    }
    else { $("input#state1").attr("value", "1"); }
}
function tijiao() {
    if ($("#form1").form("validate")) {
        $.ajax({
            url: "user.axd?type=insert",
            method: "post",
            data: $("#add_menber_style #form1").serialize(),
            dataType: "json",
            success: function (data) {
                console.info(data);
                if (data.result == "1") {
                    alert("添加成功!");
                    window.location.href = "Users.html";
                }
                else if (data.result == "2") {
                    alert("修改成功!");
                    window.location.href = "Users.html";
                }
                else if (data.result == "3") {
                    $("#add_menber_style").dialog("close");
                    alert(data.msg);
                    ERRORpath = data.file;
                    if ($(".datagrid-toolbar .DownERROR #ERROREXCEL").length <= 0) {
                        $('.datagrid-toolbar .DownERROR').append('<input type="button" id="ERROREXCEL" style="float: left;" value="下载错误用户信息表" onclick="xiaZAI();" />');
                    }
                }
            }
        });
    } else {
        $.messager.alert("提醒", "请填写相关信息！");
    }

}
function tijiao2() {
    $.ajax({
        url: "user.axd?type=insert",
        method: "post",
        data: $("#Update_menber_style #form2").serialize(),
        dataType: "json",
        success: function (data) {
            console.info(data);
            if (data.result == "1") {
                alert("添加成功!");
                window.location.href = "Users.html";
            }
            else if (data.result == "2") {
                alert("修改成功!");
                window.location.href = "Users.html";
            }
        }
    });
}
function bianji(id) {
    $.ajax({
        url: "user.axd",
        method: "post",
        data: {
            type: "Update",
            sql: "SELECT * FROM users WHERE id=" + id
        },
        dataType: "json",
        success: function (data) {
            var newdata = JSON.parse(data);
            $("#birthdate1").val(newdata[0].birthdate);
            $("#addr1").val(newdata[0].addr);
            $("#employer1").val(newdata[0].employer);
            $("#nationalId1").val(newdata[0].nationalId);
            $("#phone11").val(newdata[0].phone);
            $("#gender1").val(newdata[0].gender);
            $("#part1").val(newdata[0].gflag);
            $("#realname1").val(newdata[0].realname);
            $("#password1").val(newdata[0].password);
            $("#cardId1").val(newdata[0].cardId);
            $("#username1").val(newdata[0].username);
            $("#iprange1").val(newdata[0].iprange);
            $("#userId1").val(newdata[0].id);
            $("#update_menber_style").dialog({
                width: 600,
                height: 400,
                title: "编辑用户",
                modal: true,
                buttons: [{
                    iconcls: "icon-ok",
                    text: "保存",
                    onclick: function () {
                        tijiao2();
                    }
                },
                {
                    iconcls: "icon-cancel",
                    text: "取消",
                    onclick: function () {
                        qingchu2();
                    }
                }
                ]
            })
        }
    });
}
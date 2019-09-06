<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userInfo.aspx.cs" Inherits="ShareManagement.Products_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="renderer" content="webkit|ie-comp|ie-stand" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta http-equiv="Cache-Control" content="no-siteapp" />
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/style.css" />
    <link rel="stylesheet" href="assets/css/ace.min.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Widget/zTree/css/zTreeStyle/zTreeStyle.css" type="text/css" />
    <link href="Widget/icheck/icheck.css" rel="stylesheet" type="text/css" />
    <!--[if IE 7]>
      <link rel="stylesheet" href="assets/css/font-awesome-ie7.min.css" />
    <![endif]-->
    <!--[if lte IE 8]>
      <link rel="stylesheet" href="assets/css/ace-ie.min.css" />
    <![endif]-->
    <script src="js/jquery-1.9.1.min.js"></script>
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/typeahead-bs2.min.js"></script>
    <!-- page specific plugin scripts -->
    <script src="assets/js/jquery.dataTables.min.js"></script>
    <script src="assets/js/jquery.dataTables.bootstrap.js"></script>
    <script type="text/javascript" src="js/H-ui.js"></script>
    <script type="text/javascript" src="js/H-ui.admin.js"></script>
    <script src="assets/layer/layer.js" type="text/javascript"></script>
    <script src="assets/laydate/laydate.js" type="text/javascript"></script>
    <script type="text/javascript" src="Widget/zTree/js/jquery.ztree.all-3.5.min.js"></script>
    <script src="js/lrtk.js" type="text/javascript"></script>
    <script src="IndexJs/userInfo.js"></script>
    <title>用户列表</title>
</head>
<body>
    <div class=" page-content clearfix">
        <div id="products_style">
            <%--            <div class="search_style">
                <ul class="search_content clearfix">
                    <li>
                        <label class="l_f">用户名称</label><input name="" type="text" class="text_add" placeholder="输入用户名称" style="width: 250px" /></li>
                    <li>
                        <label class="l_f">添加时间</label><input class="inline laydate-icon" id="" style="margin-left: 10px;" /></li>
                    <li style="width: 90px;">
                        <button type="button" class="btn_search"><i class="icon-search"></i>查询</button></li>
                </ul>
            </div>--%>
            <div class="border clearfix" style="overflow: hidden;">
                <span class="l_f">
                    <a href="javascript:void(0)" id="member_add" title="添加商品" class="btn btn-warning" style="float: left;"><i class="icon-plus"></i>添加用户</a>
                    <%--                    <a href="javascript:void(0)" onclick="" class="btn btn-danger"><i class="icon-trash"></i>批量删除</a>--%>
                </span>
                <input type="file" style="float: left; width: 70px;" id="fileInsert" />
                <input type="button" style="float: left;" value="导入" id="FileInsert" onclick="inserts();" />
                <input type="button" style="float: left;" value="下载模板案例" id="DownERROR" onclick="DownRIGHT();" />
                <span class="DownERROR"></span>
                <span class="r_f">共：<b><%=dt.Rows.Count %></b>个用户</span>
            </div>
        </div>
        <div class="list_style" id="testIframe">
            <table class="table table-striped table-bordered table-hover" id="sample-table">
                <%--<thead>
                    <tr>
                        <th width="25px">
                            <label>
                                <input type="checkbox" class="ace" /><span class="lbl"></span></label></th>
                        <th width="80px">序号</th>
                        <th width="80px">用户名</th>
                        <th width="250px">卡号</th>
                        <th width="100px">分组</th>
                        <th width="70px">状态</th>
                        <th width="200px">操作</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>--%>
            </table>
        </div>
        <div class="add_menber" id="Update_menber_style" style="display: none;">
            <form action="#" method="post" id="form2">
                <table class="table table-striped table-bordered table-hover">
                    <tr>
                        <td align="center">用户账号：</td>
                        <td>
                            <input type="text" id="username1" name="username" placeholder="账号(字母/数字)" /></td>
                        <td align="center">用户卡号：</td>
                        <td>
                            <input type="text" id="cardId1" name="cardId" placeholder="卡号" /></td>
                    </tr>
                    <tr>
                        <td align="center">真实姓名：</td>
                        <td>
                            <input type="text" id="realname1" name="realname" placeholder="真实姓名" /></td>
                        <td align="center">联系电话：</td>
                        <td>
                            <input type="text" id="phone11" name="phone" /></td>
                    </tr>
                    <tr>
                        <td align="center">用户分组：</td>
                        <td>
                            <select id="part1" name="gflag">
                                <option value="admin">管理员组</option>
                                <option value="localuser">内网用户</option>
                                <option value="authlogin">授权用户</option>
                                <option value="organizedA">机构A</option>
                                <option value="organizedB">机构B</option>
                                <option value="organizedC">机构C</option>
                                <option value="teacher">教师</option>
                                <option value="student">学生</option>
                                <option value="normal">默认分组</option>
                                <option value="black">黑明单</option>
                            </select>
                        </td>
                        <td align="center">用户性别：</td>
                        <td>
                            <select id="gender1" name="gender">
                                <option value="male">男</option>
                                <option value="female">女</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">单位信息：</td>
                        <td>
                            <input type="text" id="employer1" name="employer" /></td>
                        <td align="center">居住地址：</td>
                        <td>
                            <input type="text" id="addr1" name="addr" /></td>
                    </tr>
                    <tr>
                        <td align="center">出生日期：</td>
                        <td>
                            <input type="text" class="inline laydate-icon" id="start1" name="birthdate" /></td>
                        <td align="center">状态：</td>
                        <td>
                            <input type="checkbox" id="state1" name="state" value="0" onclick="statechecked();" /></td>
                        <td>
                            <input type="text" id="userId1" name="id" style="display: none;" /></td>
                    </tr>
                </table>
                <div style="overflow: hidden;">
                    <div style="margin-right: 5px; margin-bottom: 5px; width: 76px; float: right;">
                        <button class="btn btn-primary radius" type="button" onclick="tijiao2();">&nbsp;&nbsp;提交&nbsp;&nbsp;</button>
                    </div>
                    <div style="margin-right: 5px; margin-bottom: 5px; width: 76px; float: right;">
                        <button class="btn btn-default radius" type="button" onclick="qingchu2();">&nbsp;&nbsp;清除&nbsp;&nbsp;</button>
                    </div>
                </div>
            </form>
        </div>
        <div class="add_menber" id="add_menber_style" style="display: none">
            <form action="#" id="form1">
                <table class="table table-striped table-bordered table-hover">
                    <tbody>
                        <tr>
                            <td align="center">用户账号：</td>
                            <td>
                                <input type="text" id="username" name="username" placeholder="账号(字母/数字)" /></td>
                            <td align="center">用户卡号：</td>
                            <td>
                                <input type="text" id="cardId" name="cardId" placeholder="卡号" /></td>
                        </tr>
                        <tr>
                            <td align="center">用户密码：</td>
                            <td>
                                <input type="text" id="password" name="password" placeholder="请输入密码" /></td>
                            <td align="center">真实姓名：</td>
                            <td>
                                <input type="text" id="realname" name="realname" placeholder="真实姓名" /></td>
                        </tr>
                        <tr>
                            <td align="center">用户分组：</td>
                            <td>
                                <select id="part" name="gflag">
                                    <option value="admin">管理员组</option>
                                    <option value="localuser">内网用户</option>
                                    <option value="authlogin">授权用户</option>
                                    <option value="organizedA">机构A</option>
                                    <option value="organizedB">机构B</option>
                                    <option value="organizedC">机构C</option>
                                    <option value="teacher">教师</option>
                                    <option value="student">学生</option>
                                    <option value="normal">默认分组</option>
                                    <option value="black">黑明单</option>
                                </select>
                            </td>
                            <td align="center">用户性别：</td>
                            <td>
                                <select id="gender" name="gender">
                                    <option value="male">男</option>
                                    <option value="female">女</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">联系电话：</td>
                            <td>
                                <input type="text" id="phone" name="phone" /></td>
                            <td align="center">证件号码：</td>
                            <td>
                                <input type="text" id="nationalId" name="nationalId" /></td>
                        </tr>
                        <tr>
                            <td align="center">单位信息：</td>
                            <td>
                                <input type="text" id="employer" name="employer" /></td>
                            <td align="center">居住地址：</td>
                            <td>
                                <input type="text" id="addr" name="addr" /></td>
                        </tr>
                        <tr>
                            <td align="center">自动登录：</td>
                            <td>
                                <input type="text" id="iprange" name="iprange" placeholder="eg:192.168.1.1-192.168.1.9" /></td>
                            <td align="center">出生日期：</td>
                            <td>
                                <input type="text" class="inline laydate-icon" id="start" name="birthdate" /></td>
                        </tr>
                        <tr>
                            <td align="center">状态：</td>
                            <td>
                                <input type="checkbox" id="state" name="state" value="0" onclick="statechecked();" /></td>
                            <td>
                                <input type="text" id="userId" name="id" style="display: none;" /></td>
                        </tr>
                    </tbody>
                </table>
                <div style="overflow: hidden;">
                    <div style="margin-right: 5px; margin-bottom: 5px; width: 76px; float: right;">
                        <button class="btn btn-primary radius" type="button" onclick="tijiao();">&nbsp;&nbsp;提交&nbsp;&nbsp;</button>
                    </div>
                    <div style="margin-right: 5px; margin-bottom: 5px; width: 76px; float: right;">
                        <button class="btn btn-default radius" type="button" onclick="qingchu();">&nbsp;&nbsp;清除&nbsp;&nbsp;</button>
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
<script type="text/javascript">
    $("#sample-table").DataTable({
        ajax: {
            method: "post",
            url: "user.axd",
            data: {
                sql: "SELECT * FROM users",
                type: "select"
            },
            dataType: "json",
            dataSrc: "data"
        },
        sServerMethod: "POST",
        bPaginate: true,
        destroy: true,
        data: { type: "select", sql: "SELECT * FROM users" },
        columns: [{
            data: "id",
            bSortable: false,
            mRender: function (e) {
                return "<td width='25px'><label><input type='checkbox' class='ace'/><span class='lbl'></span></label></td>";
            }
        }, {
            data: "id",
            sTitle: "序号",
            bSortable: false
        },
        {
            data: "username",
            sTitle: "用户名",
            bSortable: false
        },
        {
            data: "cardId",
            sTitle: "卡号",
            bSortable: false
        },
        {
            data: "gflag",
            sTitle: "分组",
            bSortable: false
        },
        {
            data: "state",
            sTitle: "状态",
            bSortable: false,
            mRender: function (e) {
                return "<td class='td-status'><input type='checkbox' id='states' value='" + e.state + "'/></td>";
            }
        },
        {
            data: "id",
            bSortable: false,
            mRender: function (e) {
                return "<a onclick='bianji(" + e.id + ")' href='javascript://' title='编辑' class='btn btn-xs btn-info'><i class='icon-edit bigger-120'></i></i></a >";
            }
        }
        ]
    })
    laydate({
        elem: '#start',
        event: 'focus'
    });
    jQuery(function ($) {
        var oTable1 = $('#sample-table').dataTable({
            "aaSorting": [[1, "desc"]],//默认第几个排序
            "bStateSave": true,//状态保存
            "aoColumnDefs": [
                //{"bVisible": false, "aTargets": [ 3 ]} //控制列的隐藏显示
                { "orderable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6] }// 制定列不参与排序
            ]
        });


        $('table th input:checkbox').on('click', function () {
            var that = this;
            $(this).closest('table').find('tr > td:first-child input:checkbox')
                .each(function () {
                    this.checked = that.checked;
                    $(this).closest('tr').toggleClass('selected');
                });

        });
    });
    /*用户-添加*/
    $('#member_add').on('click', function () {
        layer.open({
            type: 1,
            title: '添加用户',
            maxmin: true,
            shadeClose: true, //点击遮罩关闭层
            area: ['1000px', ''],
            content: $('#add_menber_style')
        });
    });
    //初始化宽度、高度
    $(".table_menu_list").width($(window).width() - 50);
    $(".table_menu_list").height($(window).height() - 215);
    //当文档窗口发生改变时 触发
    $(window).resize(function () {
        $(".widget-box").height($(window).height() - 215);
        $(".table_menu_list").width($(window).width() - 260);
        $(".table_menu_list").height($(window).height() - 215);
    })
</script>

using Management;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;

namespace ServiceDAL
{
    public class UserInfo : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }
        string requset = string.Empty;

        private static string FilePath = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            requset = context.Request["type"];
            switch (requset)
            {
                case "selectPage": seletc(context); break;
                case "insert": insert(context); break;
                case "Daoru": Daoru(context); break;
                case "Update": Update(context); break;
                case "DownNewExcel": CreateANLI(); break;
                case "Login": Login(context); break;
                case "STREAM": Down(); break;
                case "search": SearchUer(context); break;
                default: break;
            }
        }
        #region 模糊查询
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="context">前台参数</param>
        public static void SearchUer(HttpContext context)
        {
            string username = context.Request["name"];
            if (username == "")
                context.Response.Write(JsonConvert.SerializeObject(new { result = "none", msg = "没有输入用户名" }));
            else
            {
                string sql = $"SELECT * FROM users WHERE username LIKE '%{username}%'";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql, DBContent.mySql);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { rows = dt, total = dt.Rows.Count, msg = "存在用户名" }));
                }
                else
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { result = 0, msg = "请输入正确的用户名！" }));
                }
            }

        }
        #endregion
        #region 管理员登录
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="context">前台参数</param>
        public static void Login(HttpContext context)
        {
            string username = context.Request["username"];
            string password = context.Request["password"];
            if (username == "root" || username == "admin")
            {
                users user = new users();
                string sql = $"SELECT * FROM users WHERE username='{username}'";
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, DBContent.mySql);
                da.Fill(dt);
                user.salt = dt.Rows[0]["salt"].ToString();
                string mD5PASSWORD = FormsAuthentication.HashPasswordForStoringInConfigFile(password + user.salt, "MD5");
                string sql2 = $"SELECT * FROM users WHERE username='{username}' AND PASSWORD='{mD5PASSWORD}'";
                da = new MySqlDataAdapter(sql2, DBContent.mySql);
                DataTable dt2 = new DataTable();
                da.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { result = true, msg = "登录成功！" }));
                }
                else
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { resul = false, msg = "请输入正确的用户名或密码！" }));
                }
            }
            else
            {
                context.Response.Write(JsonConvert.SerializeObject(new { result = false, msg = "请输入正确的用户名和密码！" }));
            }
        }
        #endregion
        #region 创建模板案例
        private static long name;
        /// <summary>
        /// 创建模板案例
        /// </summary>
        private static string ANLipath = string.Empty;
        public static void CreateANLI()
        {
            var tmpName = System.Web.HttpContext.Current.Server.MapPath("~/NewExcelAnLi/");
            if (System.IO.File.Exists(tmpName + "用户案例信息表.xlsx"))
            {
                System.IO.File.Delete(tmpName + "用户案例信息表.xlsx");
            }
            FileInfo newFile = new FileInfo(tmpName + "\\" + "用户案例信息表.xlsx");
            ANLipath = Path.Combine("/NewExcelAnLi/", "用户案例信息表.xlsx");
            using (ExcelPackage excel = new ExcelPackage(newFile))
            {
                if (!System.IO.Directory.Exists(tmpName))
                    System.IO.Directory.CreateDirectory(tmpName);
                ExcelWorksheet excels = excel.Workbook.Worksheets.Add("sheet1");
                excels.Cells[1, 1].Value = "用户名";
                excels.Cells[1, 2].Value = "密码";
                excels.Cells[1, 3].Value = "分组";
                excels.Cells[1, 4].Value = "卡号";
                excels.Cells[1, 5].Value = "真实姓名";
                excels.Cells[1, 6].Value = "性别";
                excels.Cells[1, 7].Value = "出生日期";
                excels.Cells[1, 8].Value = "联系电话";
                excels.Cells[1, 9].Value = "单位信息";
                excels.Cells[1, 10].Value = "身份证";
                excels.Cells[1, 11].Value = "联系地址";
                excels.Cells[1, 12].Value = "IP登录";
                excels.Cells[1, 13].Value = "状态";
                excels.Cells[2, 1].Value = "admin";
                excels.Cells[2, 2].Value = "123....";
                var val = excels.DataValidations.AddListValidation(excels.Cells[2, 3].Address);//设置下拉框显示的数据区域
                for (int j = 0; j < fenzuChoice.Length; j++)
                {
                    val.Formula.Values.Add(fenzuChoice[j]);
                }
                val.Prompt = "管理员组,内网用户,授权用户,机构A,机构B,机构C,教师,学生,默认分组,黑明单";
                val.ShowInputMessage = true;
                excels.Cells[2, 3].Value = "默认用户";
                excels.Cells[2, 4].Value = "42....";
                excels.Cells[2, 5].Value = "张三";
                excels.Cells[2, 6].Value = "男";
                excels.Cells[2, 7].Value = "1998-2-28";
                excels.Cells[2, 8].Value = "131.....";
                excels.Cells[2, 9].Value = "某某公司";
                excels.Cells[2, 10].Value = "43......";
                excels.Cells[2, 11].Value = "..省....";
                excels.Cells[2, 12].Value = "127.0.0.1-127.0.0.1";
                excels.Cells[2, 13].Value = "true";
                excel.Save();
            }


            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(new { file = ANLipath }));
        }
        #endregion
        public static void Down()
        {
            string fileURL = System.Web.HttpContext.Current.Server.MapPath("~/NewExcel/");//文件路径，可用相对路径
            FileInfo fileInfo = new FileInfo(fileURL + name + ".xlsx");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;   filename=" + System.Web.HttpContext.Current.Server.UrlEncode(fileInfo.Name.ToString()));//文件名称  
            HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());//文件长度  
            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
            HttpContext.Current.Response.ContentType = "application/octet-stream";//获取或设置HTTP类型  
            HttpContext.Current.Response.WriteFile(fileInfo.FullName);//将文件内容作为文件块直接写入HTTP响应输出流  
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #region 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="context">前台参数</param>
        public static void seletc(HttpContext context)
        {
            int page = int.Parse(context.Request["page"].ToString());
            int row = int.Parse(context.Request["rows"].ToString());
            int SelectROWsCOUNT = (page - 1) * row;
            string Fsql = $"SELECT * FROM users LIMIT {SelectROWsCOUNT},{row}";
            DataTable dt = new DataTable();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(Fsql, DBContent.mySql);
            dataAdapter.Fill(dt);
            int ALL = UserInfo.ALLUserCount();
            context.Response.Write(JsonConvert.SerializeObject(new { total = ALL, rows = dt }));
        }
        #endregion
        #region 查询总用户人数
        /// <summary>
        /// 查询总用户人数
        /// </summary>
        /// <returns></returns>
        public static int ALLUserCount()
        {
            string sql = "SELECT * FROM users";
            DataTable dt = new DataTable();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql, DBContent.mySql);
            dataAdapter.Fill(dt);
            return dt.Rows.Count;
        }
        #endregion
        #region 更新用户信息
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="context">前台参数</param>
        public void Update(HttpContext context)
        {

            string sql = context.Request["sql"];
            MySqlDataAdapter mySqlData = new MySqlDataAdapter(sql, DBContent.ConnectSting);
            DataTable dt = new DataTable();
            mySqlData.Fill(dt);
            context.Response.Write(JsonConvert.SerializeObject(dt));
        }
        #endregion
        #region 批量导入
        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="context">前台参数</param>
        public void Daoru(HttpContext context)
        {
            HttpPostedFile file = context.Request.Files["file"];
            if (file == null)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { result = "none", msg = "请选择导入文件！" }));
            }
            else
            {
                string filename = file.FileName;
                Stream stream = file.InputStream;
                string[] filecombin = filename.Split('.');
                if (file == null || String.IsNullOrEmpty(filename) || file.ContentLength == 0 || filecombin.Length < 2 || file.FileName.IndexOf(".xlsx") < 0)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { code = "201", msg = "上传出错请检查文件格式或先下载模板进行编辑添加！" }));
                }
                else
                {
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        int count = package.Workbook.Worksheets.Count;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        List<users> NEWusers = worksheetToTable(worksheet, out int rowcount, out int listcount);
                        if (Users(NEWusers) && listcount == rowcount)
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new { result = true, msg = "批量导入成功" }));
                        }
                        else
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new { result = true, msg = "存在错误用户信息，请下载用户信息表进行更新！", file = FilePath }));
                        }

                    }
                }

            }

        }
        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="list">导入数据源</param>
        /// <returns></returns>
        public static bool Users(List<users> list)
        {
            MySqlConnection my = new MySqlConnection(DBContent.ConnectSting);
            my.Open();
            string sql = "";
            //List<users> WrongUsers = new List<users>();
            yuanPassword = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                users user = list[i];
                sql = $"insert into users(username,PASSWORD,salt,gflag,cardId,realname,gender,birthdate,phone,employer,nationalId,addr,iprange,state,createdAt,updatedAt)values('{user.username}','{user.password}','{user.salt}','{user.gflag}','{user.cardId}','{user.realname}','{user.gender}','{user.birthdate}','{user.phone}','{user.employer}','{user.nationalId}','{user.addr}','{user.iprange}',{user.state},'{DateTime.Now.ToShortDateString()}','{user.updateAt}')";
                MySqlCommand cmd = new MySqlCommand(sql, my);
            }
            my.Close();
            return true;
        }
        #endregion
        #region 生成错误用户信息表
        /// <summary>
        /// 生成错误用户信息表
        /// </summary>
        /// <param name="lists">错误用户源信息</param>
        private static string[] fenzuChoice = { "管理员组,内网用户,授权用户,机构A,机构B,机构C,教师,学生,默认分组,黑明单" };
        public static void CreateEXCEL(List<users> lists, List<string> pass)
        {
            string fenzu = string.Empty;
            string Sex = string.Empty;
            name = DateTime.Now.ToFileTime();
            var tmpName = System.Web.HttpContext.Current.Server.MapPath("~/NewExcel/");
            FileInfo newFile = new FileInfo(tmpName + "\\" + name + ".xlsx");
            FilePath = Path.Combine("/NewExcel/", name.ToString() + ".xlsx");
            using (ExcelPackage excel = new ExcelPackage(newFile))
            {
                if (!System.IO.Directory.Exists(tmpName))
                    System.IO.Directory.CreateDirectory(tmpName);
                ExcelWorksheet excels = excel.Workbook.Worksheets.Add("sheet1");
                excels.Cells[1, 1].Value = "用户名";
                excels.Cells[1, 2].Value = "密码";
                excels.Cells[1, 3].Value = "分组";
                excels.Cells[1, 4].Value = "卡号";
                excels.Cells[1, 5].Value = "真实姓名";
                excels.Cells[1, 6].Value = "性别";
                excels.Cells[1, 7].Value = "出生日期";
                excels.Cells[1, 8].Value = "联系电话";
                excels.Cells[1, 9].Value = "单位信息";
                excels.Cells[1, 10].Value = "身份证";
                excels.Cells[1, 11].Value = "联系地址";
                excels.Cells[1, 12].Value = "IP登录";
                excels.Cells[1, 13].Value = "状态";
                if (lists == null)
                    return;
                else
                {
                    int index = 0;
                    for (int i = 2; index < lists.Count; i++)
                    {
                        excels.Cells[i, 1].Value = lists[index].username;
                        excels.Cells[i, 2].Value = pass[index];
                        var val = excels.DataValidations.AddListValidation(excels.Cells[i, 3].Address);//设置下拉框显示的数据区域
                        for (int j = 0; j < fenzuChoice.Length; j++)
                        {
                            val.Formula.Values.Add(fenzuChoice[j]);
                        }
                        val.Prompt = "管理员组,内网用户,授权用户,机构A,机构B,机构C,教师,学生,默认分组,黑明单";
                        val.ShowInputMessage = true;
                        fenzu = lists[index].gflag;
                        #region 错误用户分组
                        if (fenzu == "admin")
                            excels.Cells[i, 3].Value = "管理员组";
                        if (fenzu == "localuser")
                            excels.Cells[i, 3].Value = "内网用户";
                        if (fenzu == "authlogin")
                            excels.Cells[i, 3].Value = "授权用户";
                        if (fenzu == "organizedA")
                            excels.Cells[i, 3].Value = "机构A";
                        if (fenzu == "organizedB")
                            excels.Cells[i, 3].Value = "机构B";
                        if (fenzu == "organizedC")
                            excels.Cells[i, 3].Value = "机构C";
                        if (fenzu == "teacher")
                            excels.Cells[i, 3].Value = "教师";
                        if (fenzu == "student")
                            excels.Cells[i, 3].Value = "学生";
                        if (fenzu == "normal")
                            excels.Cells[i, 3].Value = "默认分组";
                        if (fenzu == "black")
                            excels.Cells[i, 3].Value = "黑明单";
                        #endregion
                        excels.Cells[i, 4].Value = lists[index].cardId;
                        excels.Cells[i, 5].Value = lists[index].realname;
                        Sex = lists[index].gender;
                        #region 性别
                        if (Sex == "male")
                        {
                            excels.Cells[i, 6].Value = "男";
                        }
                        else
                        {
                            excels.Cells[i, 6].Value = "女";
                        }
                        #endregion
                        excels.Cells[i, 7].Value = lists[index].birthdate;
                        excels.Cells[i, 8].Value = lists[index].phone;
                        excels.Cells[i, 9].Value = lists[index].employer;
                        excels.Cells[i, 10].Value = lists[index].nationalId;
                        excels.Cells[i, 11].Value = lists[index].addr;
                        excels.Cells[i, 12].Value = lists[index].iprange;
                        excels.Cells[i, 13].Value = lists[index].state;
                        index++;
                    }
                }
                excel.Save();
            }
        }
        #endregion
        #region 批量导入检查是否存在相同用户
        /// <summary>
        /// 批量导入检查是否存在相同用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public static int SingleUser(string username)
        {
            string sql = $"SELECT* FROM users WHERE username ='{username}'";
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql, DBContent.mySql);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            return dt.Rows.Count;
        }
        #endregion
        #region 读取excel表格
        /// <summary>
        /// 读取excel表格
        /// </summary>
        /// <param name="worksheets">读取的目标表sheet</param>
        /// <returns></returns>
        private static string group = string.Empty;
        private static string Gender = string.Empty;
        private static List<string> yuanPassword = null;
        public static List<users> worksheetToTable(ExcelWorksheet worksheets, out int rowcount, out int listcount)
        {
            //获取表格的列数和行数
            rowcount = worksheets.Dimension.Rows;
            int ColCount = worksheets.Dimension.Columns;
            yuanPassword = new List<string>();
            DataTable DT = new DataTable();
            List<users> list = new List<users>();
            List<users> WrongList = new List<users>();
            int firstCount = 0;
            int secondCount = 0;
            for (int a = 2; a <= rowcount; a++)
            {
                for (int b = 1; b < ColCount; b++)
                {
                    if (worksheets.Cells[a, b].Value == null || SingleUser(worksheets.Cells[a, 1].Value.ToString()) > 0)
                    {
                        users wrongU = new users();
                        wrongU.addr = worksheets.Cells[a, 11].Value == null ? null : worksheets.Cells[a, 11].Value.ToString();
                        wrongU.birthdate = worksheets.Cells[a, 7].Value == null ? null : worksheets.Cells[a, 7].Value.ToString();
                        wrongU.cardId = worksheets.Cells[a, 4].Value == null ? null : worksheets.Cells[a, 4].Value.ToString();
                        wrongU.employer = worksheets.Cells[a, 9].Value == null ? null : worksheets.Cells[a, 9].Value.ToString();
                        Gender = worksheets.Cells[a, 6].Value == null ? null : worksheets.Cells[a, 6].Value.ToString();
                        #region 性别
                        if (Gender == "男")
                        {
                            wrongU.gender = "male";
                        }
                        else
                        {
                            wrongU.gender = "female";
                        }
                        #endregion
                        group = worksheets.Cells[a, 3].Value == null ? null : worksheets.Cells[a, 3].Value.ToString();
                        #region 分组
                        if (group == "内网用户")
                        {
                            wrongU.gflag = "localuser";
                        }
                        if (group == "管理员组")
                        {
                            wrongU.gflag = "admin";
                        }
                        if (group == "授权用户")
                        {
                            wrongU.gflag = "authlogin";
                        }
                        if (group == "机构A")
                        {
                            wrongU.gflag = "organizedA";
                        }
                        if (group == "机构B")
                        {
                            wrongU.gflag = "organizedB";
                        }
                        if (group == "机构C")
                        {
                            wrongU.gflag = "organizedC";
                        }
                        if (group == "教师")
                        {
                            wrongU.gflag = "teacher";
                        }
                        if (group == "学生")
                        {
                            wrongU.gflag = "student";
                        }
                        if (group == "默认分组")
                        {
                            wrongU.gflag = "normal";
                        }
                        if (group == "黑明单")
                        {
                            wrongU.gflag = "black";
                        }
                        #endregion
                        wrongU.iprange = worksheets.Cells[a, 12].Value == null ? null : worksheets.Cells[a, 12].Value.ToString();
                        wrongU.nationalId = worksheets.Cells[a, 10].Value == null ? null : worksheets.Cells[a, 10].Value.ToString();
                        wrongU.password = worksheets.Cells[a, 2].Value == null ? null : worksheets.Cells[a, 2].Value.ToString();
                        yuanPassword.Add(wrongU.password);
                        wrongU.phone = worksheets.Cells[a, 8].Value == null ? null : worksheets.Cells[a, 8].Value.ToString(); ;
                        wrongU.realname = worksheets.Cells[a, 5].Value == null ? null : worksheets.Cells[a, 5].Value.ToString();
                        if (worksheets.Cells[a, 6].Value.ToString() == "true")
                            wrongU.state = true;
                        else
                            wrongU.state = false;
                        wrongU.username = worksheets.Cells[a, 1].Value.ToString() == null ? null : worksheets.Cells[a, 1].Value.ToString();
                        WrongList.Add(wrongU);
                        secondCount = firstCount;
                        firstCount++;
                    }
                    if (SingleUser(worksheets.Cells[a, 1].Value.ToString()) > 0)
                        break;
                }
                if (firstCount > secondCount)
                    continue;
                else
                {
                    users newusser = new users();
                    newusser.username = worksheets.Cells[a, 1].Value.ToString();
                    newusser.password = FormsAuthentication.HashPasswordForStoringInConfigFile(worksheets.Cells[a, 2].Value.ToString() + UserInfo.salt(), "MD5");
                    newusser.salt = UserInfo.salt();
                    group = worksheets.Cells[a, 3].Value.ToString();
                    #region 分组
                    if (group == "内网用户")
                    {
                        newusser.gflag = "localuser";
                    }
                    if (group == "管理员组")
                    {
                        newusser.gflag = "admin";
                    }
                    if (group == "授权用户")
                    {
                        newusser.gflag = "authlogin";
                    }
                    if (group == "机构A")
                    {
                        newusser.gflag = "organizedA";
                    }
                    if (group == "机构B")
                    {
                        newusser.gflag = "organizedB";
                    }
                    if (group == "机构C")
                    {
                        newusser.gflag = "organizedC";
                    }
                    if (group == "教师")
                    {
                        newusser.gflag = "teacher";
                    }
                    if (group == "学生")
                    {
                        newusser.gflag = "student";
                    }
                    if (group == "默认分组")
                    {
                        newusser.gflag = "normal";
                    }
                    if (group == "黑明单")
                    {
                        newusser.gflag = "black";
                    }
                    #endregion
                    newusser.cardId = worksheets.Cells[a, 4].Value.ToString();
                    newusser.realname = worksheets.Cells[a, 5].Value.ToString();
                    Gender = worksheets.Cells[a, 6].Value.ToString();
                    #region 性别
                    if (Gender == "男")
                    {
                        newusser.gender = "male";
                    }
                    else
                    {
                        newusser.gender = "female";
                    }
                    #endregion
                    newusser.birthdate = worksheets.Cells[a, 7].Value.ToString();
                    newusser.phone = worksheets.Cells[a, 8].Value.ToString();
                    newusser.employer = worksheets.Cells[a, 9].Value.ToString();
                    newusser.nationalId = worksheets.Cells[a, 10].Value.ToString();
                    newusser.addr = worksheets.Cells[a, 11].Value.ToString();
                    newusser.iprange = worksheets.Cells[a, 12].Value.ToString();
                    if (worksheets.Cells[a, 13].Value.ToString() == "true")
                        newusser.state = true;
                    else
                        newusser.state = false;
                    newusser.createdAt = DateTime.Parse(DateTime.Now.ToShortDateString());
                    newusser.updateAt = DateTime.Parse(DateTime.Now.ToShortDateString());
                    list.Add(newusser);
                }

            }
            listcount = list.Count;
            if (WrongList.Count != 0)
            {
                CreateEXCEL(WrongList, yuanPassword);
            }
            return list;
        }
        #endregion
        #region 增加和修改用户信息
        /// <summary>
        /// 增加和修改用户信息
        /// </summary>
        /// <param name="context">前台参数</param>
        public void insert(HttpContext context)
        {
            List<users> WrongUsers = new List<users>();
            users user = new users();
            user.id = context.Request["id"];
            user.username = context.Request["username"];
            user.addr = context.Request["addr"];
            user.birthdate = context.Request["birthdate"];
            user.cardId = context.Request["cardId"];
            user.createdAt = DateTime.Now;
            user.employer = context.Request["employer"];
            user.gflag = context.Request["gflag"];
            user.iprange = context.Request["iprange"];
            user.nationalId = context.Request["nationalId"];
            user.password = context.Request["password"];
            user.salt = salt();
            user.password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.password + user.salt, "MD5");
            user.phone = context.Request["phone"];
            user.realname = context.Request["realname"];
            user.gender = context.Request["gender"];
            string state = context.Request["state"];
            if (state == null)
            {
                user.state = false;
            }
            else
            {
                user.state = true;
            }
            yuanPassword = new List<string>();
            MySqlConnection my = new MySqlConnection(DBContent.ConnectSting);
            my.Open();
            if (user.id == "")
            {
                int existNum = SingleUser(user.username);
                if (existNum > 0)
                {
                    yuanPassword.Add(user.password);
                    WrongUsers.Add(user);
                    CreateEXCEL(WrongUsers,yuanPassword);
                    context.Response.Write(JsonConvert.SerializeObject(new { result = "3", msg = "存在错误用户信息，请下载用户信息表进行更新！", file = FilePath }));
                    return;
                }
                string sql = $"insert into users(username,PASSWORD,salt,gflag,cardId,realname,gender,birthdate,phone,employer,nationalId,addr,iprange,state,createdAt,updatedAt)values('{user.username}','{user.password}','{user.salt}','{user.gflag}','{user.cardId}','{user.realname}','{user.gender}','{user.birthdate}','{user.phone}','{user.employer}','{user.nationalId}','{user.addr}','{user.iprange}',{user.state},'{DateTime.Now.ToShortDateString()}','{user.updateAt}')";
                MySqlCommand cmd = new MySqlCommand(sql, my);
                int num = cmd.ExecuteNonQuery();
                my.Close();
                context.Response.Write(JsonConvert.SerializeObject(new { result = num }));
            }
            else
            {
                string sqls = $"UPDATE users SET username='{user.username}',gflag='{user.gflag}',cardId='{user.cardId}',realname='{user.realname}',gender='{user.gender}',birthdate='{user.birthdate}',phone='{user.phone}',employer='{user.employer}',nationalId='{user.nationalId}',addr='{user.addr}',state={user.state},updatedAt='{DateTime.Now.ToShortDateString()}' WHERE id={user.id}";
                MySqlCommand cmd = new MySqlCommand(sqls, my);
                int num = cmd.ExecuteNonQuery();
                num++;
                my.Close();
                context.Response.Write(JsonConvert.SerializeObject(new { result = num }));
            }

        }
        #endregion
        #region 伪随机数
        /// <summary>
        /// 伪随机数
        /// </summary>
        /// <returns></returns>
        private static string salt()
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random randrom = new Random((int)DateTime.Now.Ticks);
            string str = "";
            for (int i = 0; i < 4; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }
            return str;
        }
        #endregion
    }
}

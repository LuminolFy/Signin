using System.Data.SQLite;

namespace Signin
{
    public static class SigninHandler
    {
        /// <summary>
        /// 签到主方法
        /// </summary>
        /// <param name="qqId">请求签到的QQ号</param>
        /// <returns></returns>
        public static string Signin(string dbPath, long qqId)
        {
            string dbFileName = Path.Combine(dbPath, "record.db");  //数据库位置(此位置为Signin.dll相同目录下的record.db文件)
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($@"Data Source={dbFileName};Version=3"))  //连接数据库
                {
                    conn.Open();  //打开数据库
                    using (SQLiteCommand commandSelect = new SQLiteCommand($"SELECT * FROM Signined WHERE Id = {qqId}", conn))  //查询传入的QQ号是否有记录
                    {
                        using (SQLiteDataReader reader = commandSelect.ExecuteReader())  //读取查询内容
                        {
                            if (reader.Read())  //识别是否有这条记录
                            {
                                DateTime lastSignin = Convert.ToDateTime(reader["LastTime"]);  //上次签到日期
                                int lastPoint = Convert.ToInt32(reader["Points"]);  //获取上次的积分
                                if (lastSignin < DateTime.Today)  //如果上次签到早于今天
                                {
                                    

                                    //把积分+1并更新回进数据库
                                    using (SQLiteCommand commandInsert = new SQLiteCommand($"UPDATE Signined SET LastTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', Points={++lastPoint} WHERE Id={qqId}", conn))
                                    {
                                        if (commandInsert.ExecuteNonQuery() > 0)  //更新成功
                                            return $"签到成功，当前积分为:{lastPoint}";  //返回签到成功
                                        else
                                            return "数据库异常，签到失败";
                                    }
                                }
                                else  //签到日期等于今天
                                {
                                    return $"你今天已经签到过啦! 当前积分为:{lastPoint}";
                                }
                            }
                            else  //没有这个QQ号, 添加
                            {
                                //往数据库插入一条数据, qq号, 日期, 积分1
                                using (SQLiteCommand commandInsert = new SQLiteCommand($"INSERT INTO Signined (Id, LastTime, Points) VALUES ({qqId},'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',{1})", conn))
                                {
                                    if (commandInsert.ExecuteNonQuery() > 0)  //插入成功
                                        return "首次签到，当前积分为:1 （可凭借之前的积分截图来更新积分，请联系QQ:1291423701进行更新。直接发QQ号+截图再@一下我 看到就会回复）";  //返回首次签到成功
                                    else
                                        return "数据库异常，签到失败";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)  //异常(例如数据库文件不存在, 表不存在等)
            {
                return "数据库异常，签到失败。" + ex.Message;
            }
        }
    }
}
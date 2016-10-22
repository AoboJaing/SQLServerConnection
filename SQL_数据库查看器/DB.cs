using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;//DataTable用到
using System.Data.SqlClient;//一系列的数据库操作类用到

namespace SQL_数据库查看器
{
    class DB : IDisposable
    {
        private SqlConnection sqlConnection;

        // 以下代码，保证该类只能有一个实例        

        // 在自己内部定义自己的一个实例，只供内部调用  
        private static DB db = null;

        // 这个类必须自动向整个系统提供这个实例对象  
        // 这里提供了一个供外部访问本class的静态方法，可以直接访问  
        public static DB getInstance()
        {
            if (db == null)
            {
                db = new DB();
            }
            return db;
        }

        private DB()// 私有无参构造函数
        {
            sqlConnection = new SqlConnection("server=.\\SQLEXPRESS;uid=aobosir;pwd=123456;database=scott");
            sqlConnection.Open();
        }
        public DB(string str)
        {
            sqlConnection = new SqlConnection(str);
            sqlConnection.Open();         
        }

        //单例化结束

        public DataTable getBySql(string sql, Object[] param)
        {//查询
            sql = String.Format(sql, param);//用字符串参数替换的形式防止注入
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand(sql, sqlConnection));
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public DataTable getBySql(string sql)
        {//无参数的查询
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand(sql, sqlConnection));
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public void setBySql(string sql, Object[] param)
        { //无查询结果的修改
            sql = String.Format(sql, param);//用字符串参数替换的形式防止注入
            new SqlCommand(sql, sqlConnection).ExecuteNonQuery();
        }
        public void setBySql(string sql)
        { //无参数，无查询结果的修改
            new SqlCommand(sql, sqlConnection).ExecuteNonQuery();
        }

        public void Dispose()
        {//相当于析构函数
            sqlConnection.Close();
            //在C#中关闭数据库连接不能在类的析构函数中关，否则会抛“内部 .Net Framework 数据提供程序错误 1”的异常
            //通过实现C#中IDisposable接口中的Dispose()方法主要用途是释放非托管资源。
        }
    }
}

// 参考网站：http://lib.csdn.net/article/csharp/35999

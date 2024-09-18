using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

namespace Assets.Library.DAL
{
    public class SqlLiteHelper
    {
        public string ConnectString { get; private set; }

        private SqliteConnection Connection { get;set; }

        public SqlLiteHelper(string DataBaseName)
        {
            string[] parts = DataBaseName.Split('.').ToArray();
            if (string.Compare(parts.Last(), "sqlite") != 0)
            {
                DataBaseName += ".sqlite";
            }
            ConnectString = "Data Source = " + Application.dataPath+ DataBaseName;
            Connection = new SqliteConnection(ConnectString);
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Close()
        {
            Connection.Close();
        }

        public int ExecuteNonQuery(string command)
        {
            if (Connection==null||Connection.State != System.Data.ConnectionState.Open)
            {
                Debug.LogException(new Exception("连接未打开"));
                return -1;
            }
            try
            {
                SqliteCommand cmd = Connection.CreateCommand();
                cmd.CommandText = command;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return -1;
            }
        }
        public object ExecuteSingleQuery(string command)
        {
            if (Connection == null || Connection.State != System.Data.ConnectionState.Open)
            {
                Debug.LogException(new Exception("连接未打开"));
                return null;
            }
            try
            {
                SqliteCommand cmd = Connection.CreateCommand();
                cmd.CommandText = command;
                return cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        public List<object[]> ExecuteMutipleQuery(string command)
        {
            if (Connection == null || Connection.State != System.Data.ConnectionState.Open)
            {
                Debug.LogException(new Exception("连接未打开"));
                return null;
            }
            try
            {
                SqliteCommand cmd = Connection.CreateCommand();
                cmd.CommandText = command;
                SqliteDataReader reader = cmd.ExecuteReader();
                List<object[]> result=new List<object[]>();
                int FieldCount=reader.FieldCount;
                while (reader.Read())
                {
                    object[] row = new object[FieldCount];
                    for(int i=0;i<FieldCount; i++)
                    {
                        row[i]= reader.GetValue(i);
                    }
                    result.Add(row);
                }
                return result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }
    }
}

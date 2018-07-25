using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace SistemaPontuacaoEntrega
{
    public class DbAccess<T> : IDisposable
    {
        public static SqlConnection sqlConnection;

        public DbAccess()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["stringConexaoSQL"].ConnectionString);
            sqlConnection.Open();
        }

        public List<T> Get()
        {
            return new List<T>();
        }

        public List<T> GetById(int id)
        {
            return new List<T>();
        }

        public List<U> GetByQuery<U>(string query)
        {
            List<U> list = null;
            using (var command = new SqlCommand(query, sqlConnection))
            {
                var dr = command.ExecuteReader();
                list = new List<U>();
                U obj = default(U);
                while (dr.Read())
                {
                    obj = Activator.CreateInstance<U>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    list.Add(obj);
                }

                dr.Close();
                dr = null;
            }
            return list;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~DbAccess()
        {
            sqlConnection.Dispose();
            sqlConnection = null;
            Dispose();            
        }
    }
}
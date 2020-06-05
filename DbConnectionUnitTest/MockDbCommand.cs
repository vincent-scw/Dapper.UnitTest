using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DbConnectionUnitTest
{
    class MockDbCommand : IDbCommand
    {
        public int ExecuteCount { get; private set; }

        public MockDbCommand(MockDbConnection connection)
        {
            Connection = connection;

            Parameters = new MockDbDataParameterCollection();
        }

        private MockDbConnection _mockDbConnection => Connection as MockDbConnection;

        #region Implement IDbCommand
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        public string CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }

        public IDataParameterCollection Parameters { get; private set; }

        public UpdateRowSource UpdatedRowSource { get; set; }

        public void Cancel()
        {

        }

        public IDbDataParameter CreateParameter()
        {
            return new MockDbDataParameter();
        }

        public void Dispose()
        {

        }

        public int ExecuteNonQuery()
        {
            ExecuteCount++;
            return 1;
        }

        public IDataReader ExecuteReader()
        {
            ExecuteCount++;
            return new DataTableReader(ToDataTable(_mockDbConnection.ReturnValue));
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            ExecuteCount++;
            return new DataTableReader(ToDataTable(_mockDbConnection.ReturnValue));
        }

        public object ExecuteScalar()
        {
            ExecuteCount++;
            return _mockDbConnection.ReturnValue;
        }

        public void Prepare()
        {

        }
        #endregion

        private DataTable ToDataTable(object retValues)
        {
            if (retValues == null)
            {
                throw new ArgumentException("ReturnValue is null.");
            }

            var items = new List<object> { retValues };

            var tb = new DataTable();
            
            PropertyInfo[] props = retValues.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }
    }
}
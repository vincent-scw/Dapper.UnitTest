using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;

namespace DbConnectionUnitTest
{
    class MockDbDataReader : DbDataReader
    {
        private int _index = -1;
        private List<object> _items = new List<object>();

        private object _currentItem => _items[_index];
        private List<PropertyInfo> _properties;

        private object _returnValue;
        public object ReturnValue 
        {
            get { return _returnValue; }
            set
            {
                var enumerable = value as IEnumerable;
                if (enumerable == null)
                {
                    throw new ArgumentException("ReturnValue for DataReader should be enumerable.");
                }

                _returnValue = value;

                foreach (var item in enumerable)
                {
                    if (_properties == null)
                    {
                        _properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
                    }

                    _items.Add(item);
                }
            }
        }

        private object GetObjectValue(object obj, string key)
        {
            var prop = obj.GetType().GetProperty(key);
            return prop.GetValue(obj);
        }

        #region DbDataReader
        public override object this[int ordinal] => GetObjectValue(_currentItem, _properties[ordinal].Name);

        public override object this[string name] => GetObjectValue(_currentItem, name);

        public override int Depth => 1;

        public override int FieldCount => _properties.Count;

        public override bool HasRows => _items.Any();

        public override bool IsClosed => false;

        public override int RecordsAffected => _items.Count;

        public override bool GetBoolean(int ordinal)
        {
            return Convert.ToBoolean(this[ordinal]);
        }

        public override byte GetByte(int ordinal)
        {
            return Convert.ToByte(this[ordinal]);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return Convert.ToInt64(this[ordinal]);
        }

        public override char GetChar(int ordinal)
        {
            return Convert.ToChar(this[ordinal]);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return Convert.ToInt64(this[ordinal]);
        }

        public override string GetDataTypeName(int ordinal)
        {
            return _properties[ordinal].Name;
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return Convert.ToDateTime(this[ordinal]);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return Convert.ToDecimal(this[ordinal]);
        }

        public override double GetDouble(int ordinal)
        {
            return Convert.ToDouble(this[ordinal]);
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int ordinal)
        {
            return _properties[ordinal].PropertyType;
        }

        public override float GetFloat(int ordinal)
        {
            return Convert.ToInt32(this[ordinal]);
        }

        public override Guid GetGuid(int ordinal)
        {
            return (Guid)this[ordinal];
        }

        public override short GetInt16(int ordinal)
        {
            return Convert.ToInt16(this[ordinal]);
        }

        public override int GetInt32(int ordinal)
        {
            return Convert.ToInt32(this[ordinal]);
        }

        public override long GetInt64(int ordinal)
        {
            return Convert.ToInt64(this[ordinal]);
        }

        public override string GetName(int ordinal)
        {
            return _properties[ordinal].Name;
        }

        public override int GetOrdinal(string name)
        {
            return Convert.ToInt32(this[name]);
        }

        public override string GetString(int ordinal)
        {
            return Convert.ToString(this[ordinal]);
        }

        public override object GetValue(int ordinal)
        {
            return this[ordinal];
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            return this[ordinal] == null;
        }

        public override bool NextResult()
        {
            return _items.Count > _index + 1;
        }

        public override bool Read()
        {
            if (_items.Count == 0)
            {
                return false;
            }

            if (_items.Count > _index + 1)
            {
                _index++;
                return true;
            }

            return false;
        }
        #endregion
    }
}

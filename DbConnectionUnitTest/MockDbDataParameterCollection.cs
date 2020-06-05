using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbConnectionUnitTest
{
    class MockDbDataParameterCollection : List<MockDbDataParameter>, IDataParameterCollection
    {
        private List<MockDbDataParameter> list;
        public MockDbDataParameterCollection()
        {
            list = new List<MockDbDataParameter>();
        }

        public object this[string parameterName]
        {
            get { return list.FirstOrDefault(x => x.ParameterName == parameterName); }
            set { }
        }

        public bool Contains(string parameterName)
        {
            return list.Any(x => x.ParameterName == parameterName);
        }

        public int IndexOf(string parameterName)
        {
            return 1;
        }

        public void RemoveAt(string parameterName)
        {

        }
    }
}

using System.Collections.Generic;

namespace lunge.Library.Debugging
{
    public class DebugData
    {
        private Dictionary<string, object> _dataMap = new Dictionary<string, object>();

        internal Dictionary<string, object> Map => _dataMap;
        
        public void Set(string label, object value)
        {
            _dataMap[label] = value;
        }

        public object Get(string label)
        {
            return _dataMap[label];
        }
    }
}
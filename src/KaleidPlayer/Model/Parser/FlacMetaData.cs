using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaleidot725.Model
{
    /// <summary>
    /// メタデータクラス
    /// </summary>
    public class FlacMetaData
    {
        private FlacMetaType type;
        private long index;
        private long size;

        public FlacMetaType Type
        {
            get { return type; }
            set { type = value; }
        }

        public long Index
        {
            get { return index; }
            set { index = value; }
        }

        public long Size
        {
            get { return size; }
            set { size = value; }
        }

        public FlacMetaData(FlacMetaType type, long index, long size)
        {
            this.type = type;
            this.index = index;
            this.size = size;
        }
    }
}

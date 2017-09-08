using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileQuery.Tests.Core
{
    class MockFileInfo : FileSystemInfo
    {
        string _name;

        public MockFileInfo(string name, bool readOnly = false)
        {
            _name = name;
            if (readOnly)
            {
                Attributes = System.IO.FileAttributes.ReadOnly;
            }
        }

        public override bool Exists
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return _name;
            }
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}

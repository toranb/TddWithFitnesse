using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fit;

namespace TddWithFitnesse.Acceptance
{
    public class RetrievePostByUriColumnFixture : ColumnFixture
    {
        private string title, content, uri;

        public bool Found()
        {
            return false;
        }
    }
}

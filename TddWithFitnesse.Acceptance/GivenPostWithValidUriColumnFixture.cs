using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fit;

namespace TddWithFitnesse.Acceptance
{
    public class GivenPostWithValidUriColumnFixture : ColumnFixture
    {
        private string title, content, uri;

        public bool Create()
        {
            return false;
        }
    }
}

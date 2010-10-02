using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fit;
using TddWithFitnesse.Acceptance;

public class RetrievePost
{
    public ColumnFixture GivenPostWithValidUri()
    {
        return new GivenPostWithValidUriColumnFixture();
    }

    public ColumnFixture RetrievePostByUri()
    {
        return new RetrievePostByUriColumnFixture();
    }
}

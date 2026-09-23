using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Application.Contract.DatabaseProvider
{
    public interface IDatabaseProvider
    {
        string Name { get; }
        string ConnectionString { get; }
    }
}

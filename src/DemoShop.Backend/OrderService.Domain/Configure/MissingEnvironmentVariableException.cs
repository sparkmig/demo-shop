using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Configure
{
    public class MissingEnvironmentVariableException(string variableName) : Exception($"Missing environment variable: {variableName}");
}

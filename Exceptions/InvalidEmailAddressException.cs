using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_project.Exceptions
{
    internal class InvalidEmailAddressException(string message) : Exception(message)
    {
    }
}

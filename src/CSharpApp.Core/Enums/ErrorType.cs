using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Enums
{
    public enum ErrorType
    {
        InternalServer,
        ModelBinding,
        Validation,
        Authentication,
        Authorization,
        NotFound,
        UnprocessableEntity,
        Conflict
    }
}

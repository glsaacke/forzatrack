using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.models.responses
{
    public class DefaultResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
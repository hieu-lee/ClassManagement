using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassManagement.Services
{
    public class TestService
    {
        public Guid RandomId { get; set; } = Guid.NewGuid();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit_management_portal.Models;

namespace Audit_management_portal
{
    public static class Global
    {
        private static AuditRequest REQ= new AuditRequest() { AuditDetails = new AuditDetails() };
    }
}

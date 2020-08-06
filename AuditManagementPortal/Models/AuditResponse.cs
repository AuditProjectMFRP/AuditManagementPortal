using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Audit_management_portal
{
    public class AuditResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AuditId { get; set; }
        public string ProjectExecutionStatus { get; set; }
        public string RemedialActionDuration { get; set; }
    }
}

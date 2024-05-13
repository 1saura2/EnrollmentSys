using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class ErrorViewModel
    {
        public String? RequestId { get; set; }
        /* public int? ShowRequestId { get; set; }*/
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

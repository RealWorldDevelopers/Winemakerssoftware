using System;
using System.Collections.Generic;

namespace WMS.Data.SQL.Entities
{
    public partial class LogError
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? Level { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? Exception { get; set; }
        public string? SourceContext { get; set; }
        public string? ActionId { get; set; }
        public string? ActionName { get; set; }
        public string? RequestId { get; set; }
        public string? RequestPath { get; set; }
        public string? SpanId { get; set; }
        public string? TraceId { get; set; }
        public string? ParentId { get; set; }
        public string? MachineName { get; set; }
        public string? EnvironmentName { get; set; }
        public string? EnvironmentUserName { get; set; }
        public string? Assembly { get; set; }
        public string? Version { get; set; }
    }
}

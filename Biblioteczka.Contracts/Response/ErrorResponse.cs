using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Biblioteczka.Contracts.Response
{
    public class ErrorResponse()
    {
        public int Status { get; set; }
        public Guid? ErrorId { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}

﻿
using System.Text.Json.Serialization;

namespace Octarine_Core.Models
{
    internal class UserDto
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}

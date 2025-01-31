using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Dtos;

public class AuthResponse
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; }
}

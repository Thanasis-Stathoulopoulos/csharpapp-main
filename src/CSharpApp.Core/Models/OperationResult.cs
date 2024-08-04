using CSharpApp.Core.Models;
using System.Text.Json.Serialization;

namespace CSharpApp.Core.Models;
public abstract class OperationResult
{
    public bool OperationSucceeded { get; set; }
    public List<ErrorResult>? ErrorResults { get; set; }
}
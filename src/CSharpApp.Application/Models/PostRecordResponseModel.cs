using CSharpApp.Application.Dtos;
using CSharpApp.Core.Models;

namespace CSharpApp.Application.Models
{
    public class PostRecordResponseModel : OperationResult
    {
        public IEnumerable<PostRecord>? PostRecords { get; set; }
    }
}
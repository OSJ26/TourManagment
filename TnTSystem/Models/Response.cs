
namespace TnTSystem.Models
{
    public class Response
    {
        public bool IsError { get; set; }

        public string Message { get; set; }

        public object DataModel { get; set; }
    }
}
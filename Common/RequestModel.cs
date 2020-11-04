using Common.Interfaces;

namespace Common
{
    public class RequestModel: IRequestModel
    {
        public string Query { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
    }
}
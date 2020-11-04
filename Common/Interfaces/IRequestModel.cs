namespace Common.Interfaces
{
    public interface IRequestModel
    {
        public string Query { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
    }
}
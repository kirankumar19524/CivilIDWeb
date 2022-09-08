namespace CivilIDWeb.Models
{
    public class RequestLog
    {
        public int LogId { get; set; }
        public string RequestId { get; set; }
        public string CivilId { get; set; }
        public DateTime RequestDateTime { get; set; }
        public bool IsRequestSuccess { get; set; }
        public string FailedReason { get; set; }
        public string ErrorCode { get; set; }
    }
}

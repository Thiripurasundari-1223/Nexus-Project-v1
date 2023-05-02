namespace APIGateWay.API.Model
{
    public class SuccessData
    {
        public string StatusCode { get; set; }
        public string StatusText { get; set; }
        public dynamic Data { get; set; }
        public dynamic Resource { get; set; }
    }
}
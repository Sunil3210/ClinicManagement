namespace ClinicManagement.Response
{
    public class BLLResponse
    {
        public object Response { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public bool Status { get; set; }
        public int StatusCode { get; set; }
    }
}

namespace ClinicManagement.Request
{
    public class SortWithPageParametersRequest
    {
        public string? SortParameter { get; set; }
        public string? SortDirection { get; set; }
        public string? SearchString { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}

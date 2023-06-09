namespace FlightDocumentManagementSystem.Data
{
    public class PagingDTO<TEntity>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<TEntity>? Result { get; set; }
    }
}

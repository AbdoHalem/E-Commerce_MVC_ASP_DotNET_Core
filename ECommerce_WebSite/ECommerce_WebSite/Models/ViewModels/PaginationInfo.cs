namespace ECommerce_WebSite.Models.ViewModels
{
    /**
     * // Holds the calculation for our page numbers
     */
    public class PaginationInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        // Calculate total pages dynamically
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}

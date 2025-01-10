namespace ShopWithMe.Tools.Models
{
    public class Pager
    {
        #region PageIndex
        public int PageIndex { get; set; }
        #endregion

        #region PageSize
        public int PageSize { get; set; }
        #endregion

        #region TotalRows
        public int TotalRows { get; set; }
        #endregion

        #region Skip
        public int Skip
        {
            get
            {
                return (PageIndex - 1) * PageSize;
            }
        }
        #endregion

        #region TotalPages
        public int TotalPages
        {
            get
            {
                if (PageSize <= 0)
                    return 0;

                var pages = TotalRows / PageSize;

                if (TotalRows % PageSize != 0)
                {
                    pages += 1;
                }

                return pages;
            }
        }
        #endregion
    }
}

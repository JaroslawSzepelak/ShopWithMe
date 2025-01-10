namespace ShopWithMe.Tools.Models
{
    public class ResultModel<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> Result { get; set; }
        public Pager Pager { get; set; }

        #region ResultModel()
        public ResultModel() { }

        public ResultModel(IEnumerable<TEntity> result, Pager pager)
        {
            Result = result;
            Pager = pager;
        }
        #endregion
    }
}

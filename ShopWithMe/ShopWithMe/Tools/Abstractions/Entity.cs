namespace ShopWithMe.Tools.Abstractions
{
    public abstract class BaseEntity : IEntity
    {
        public long Id {  get; set; }

        public virtual void OnBeforeInsert() { }
        public virtual void OnBeforeUpdate() { }
    }
}

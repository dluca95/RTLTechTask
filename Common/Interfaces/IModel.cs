namespace Common.Interfaces
{
    public interface IAppModel
    {
        int Id { get; set; }
        int? ParentId { get; set; }
    }
    
    public interface IEntityModel
    {
        // public int Id { get; set; }
    }
    
}
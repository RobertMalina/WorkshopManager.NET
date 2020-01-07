namespace WorkshopManagerNET.Model.Auth
{
  public class AppUserToAppRole
  {
    public AppUser User { get; set; }
    public long UserId { get; set; }
    public AppRole Role { get; set; }
    public int RoleId { get; set; }
  }
}



public interface ISetPlayerRoleProps 
{
    int GenderRandomNumber { get; set; }
    int RoleNumber { get; set; }
    int AvatarButtonIndex { get; set;  }   
    string RoleName { get; set; }
    bool TakeAvatarButtonOwnership { get; set; }
    bool SetOwnedAvatarButtonSprite { get; set; }
}

namespace Domain.User;

///<author>Niels Van Steen</author>
/// <summary>
/// Enum containing the different type of extra properties a user can have.
/// Each project allows a project-manager to specify extra information a user must give upon registration.
/// This extra information can be of these types.
/// </summary>
public enum UserPropertyType : byte
{
        String = 0, 
        Integer = 1, 
        Double = 2, 
        Date = 3
}
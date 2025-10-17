using Domain.Extension;

namespace Domain.constants;

public class Roles
{
    public const string SupperAdmin = "SupperAdmin";
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Customer = "Customer";
    public const string Author = "Author";

    public static List<string> GetRoles() => LoadVariableValue.GetAllConstValue(typeof(Roles));
}
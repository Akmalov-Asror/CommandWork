using Microsoft.AspNetCore.Identity;

namespace TestProject.Domains;
/// <summary>
/// Represents a user entity in the system, inheriting from IdentityUser.
/// </summary>
/// <remarks>
///     The User class extends the functionality provided by
///     <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.identityuser">IdentityUser</see>,
///     which is part of ASP.NET Core Identity and includes properties and methods
///     for managing user authentication, authorization, and user-specific data.
/// </remarks>
public class User : IdentityUser { }
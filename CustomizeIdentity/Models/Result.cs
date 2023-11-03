namespace CustomizeIdentity.Models;

public record Result(
    User user,
    bool IsSuccess = true,
    string ErrorMessage = ""
);
using Microsoft.AspNetCore.Http;

namespace Digital.Wallet.Exceptions;

public class BadRequestException(string message) : BaseException(message, StatusCodes.Status400BadRequest);

public class NotFoundException(string message) : BaseException(message, StatusCodes.Status404NotFound);

public class ForbiddenException(string message) : BaseException(message, StatusCodes.Status403Forbidden);

public class UnauthorizedException(string message) : BaseException(message, StatusCodes.Status401Unauthorized);
﻿namespace NightTasker.UserHub.Core.Application.Features.UserImages.Models;

/// <summary>
/// DTO для создания <see cref="UserImages"/>.
/// </summary>
/// <param name="UserInfoId">ИД пользователя.</param>
/// <param name="FileName">Название файла.</param>
/// <param name="ContentType">Тип содержимого.</param>
/// <param name="FileSize">Размер файла.</param>
public record CreateUserImageDto(
    Guid UserInfoId, 
    string FileName,
    string Extension,
    string ContentType, 
    long FileSize);
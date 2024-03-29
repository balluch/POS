﻿using System;
using System.ComponentModel.DataAnnotations;

namespace StoreManager.Application.Dto
{
    public record OfferDto(
        Guid Guid,
        [Range(0, 1000000, ErrorMessage = "Ungültiger Preis")]
        decimal Price,
        Guid ProductGuid,
        Guid StoreGuid);
}

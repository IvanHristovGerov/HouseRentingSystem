﻿using HouseRentSystem.Core.Contracts;

namespace HouseRentSystem.Core.Models.House
{
    public class HouseDetailsViewModel : IHouseModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Address { get; set; } = string.Empty;

        public string ImageUrl { get; set; }=string.Empty;
    }
}
